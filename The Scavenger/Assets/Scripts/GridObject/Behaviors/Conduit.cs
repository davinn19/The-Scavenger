using System.Collections.Generic;
using UnityEngine;


namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// GridObject that houses cables and defines their connectivity/transport modes.
    /// </summary>
    public class Conduit : GridObjectBehavior
    {
        private readonly Dictionary<Vector2Int, (TransportMode, DistributeMode)> sideConfigs = new();   // TODO serialize

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                sideConfigs.Add(side, (TransportMode.DISCONNECT, DistributeMode.CLOSEST_FIRST));
            }
        }

        /// <summary>
        /// Sets side configs to default values based on the adjacent GridObject.
        /// </summary>
        public override void OnPlace()
        {
            base.OnPlace();
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(side);
                ConduitInterface adjInterface = gridObject.GetAdjacentObject<ConduitInterface>(side);

                if (adjConduit)             // If next to conduit, connect.
                {
                    SetTransportMode(side, TransportMode.CONNECT);
                }
                else if (adjInterface)      // If next to interface, extract.
                {
                    SetTransportMode(side, TransportMode.EXTRACT);
                }
                else                        // Otherwise, disconnect.
                {
                    SetTransportMode(side, TransportMode.DISCONNECT);
                }
            }

            gridObject.OnSelfChanged();
        }

        /// <summary>
        /// Disconnects all adjacent conduits before removing.
        /// </summary>
        public override void OnRemove()
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                SetTransportMode(side, TransportMode.DISCONNECT);

                Vector2Int oppositeSide = side * -1;
                Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(side);

                if (adjConduit)
                {
                    adjConduit.SetTransportMode(oppositeSide, TransportMode.DISCONNECT);
                }
            }
        }

        /// <summary>
        /// If the adjacent object is removed or does not accept conduits, set to disconnect.
        /// </summary>
        /// <param name="sideUpdated">Side which the neighbor was placed on.</param>
        public override void OnNeighborPlaced(Vector2Int sideUpdated)
        {
            Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(sideUpdated);
            ConduitInterface adjInterface = gridObject.GetAdjacentObject<ConduitInterface>(sideUpdated);

            TransportMode oldTransportMode = GetTransportMode(sideUpdated);
            TransportMode transportMode;

            if (adjConduit)
            {
                transportMode = TransportMode.CONNECT;
            }
            else if (adjInterface)
            {
                transportMode = TransportMode.EXTRACT;
            }
            else
            {
                transportMode = TransportMode.DISCONNECT;
            }

            if (oldTransportMode != transportMode)
            {
                SetTransportMode(sideUpdated, transportMode);
                gridObject.OnSelfChanged();
            }
        }

        /// <summary>
        /// Ensures sure its side modes align with adjacent conduits.
        /// </summary>
        public override void OnNeighborChanged()
        {
            bool changed = false;

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                TransportMode oldTransportMode = GetTransportMode(side);
                TransportMode newTransportMode = oldTransportMode;

                Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(side);

                if (!adjConduit)
                {
                    continue;
                }

                newTransportMode = adjConduit.GetTransportMode(side * -1);

                if (oldTransportMode != newTransportMode)
                {
                    SetTransportMode(side, newTransportMode);
                    changed = true;
                }
            }

            if (changed)
            {
                gridObject.OnSelfChanged();
            }
        }

        /// <summary>
        /// Tries to add the held item as a cable to the conduit.
        /// </summary>
        /// <param name="inventory">The player's inventory.</param>
        /// <param name="heldItemHandler">The held item handler.</param>
        /// <param name="sidePressed">The side pressed when interacting.</param>
        /// <returns>True if the cable was successfully added.</returns>
        public override bool TryInteract(ItemBuffer inventory, HeldItemHandler heldItemHandler, Vector2Int sidePressed)
        {
            // Ignore if no item is held
            if (heldItemHandler.IsEmpty())
            {
                return false;
            }

            ItemStack heldItem = heldItemHandler.GetHeldItem();
            CableSpec cableSpec = heldItem.Item.GetProperty<CableSpec>();

            if (cableSpec && CanAddCable(cableSpec))
            {
                cableSpec.AddCable(this);

                gridObject.OnSelfChanged();
                heldItemHandler.Use();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Cycles the transport mode for a specific side.
        /// </summary>
        /// <param name="side">The side to edit the transport mode.</param>
        /// <returns>Always returns true.</returns>
        public override bool TryEdit(Vector2Int side)
        {
            TransportMode oldTransportMode = GetTransportMode(side);

            List<TransportMode> rotation = GetTransportModeRotation(side);

            int index = rotation.IndexOf(oldTransportMode) + 1;
            if (index >= rotation.Count)
            {
                index = 0;
            }

            TransportMode newTransportMode = rotation[index];

            SetTransportMode(side, newTransportMode);

            if (oldTransportMode != newTransportMode)
            {
                gridObject.OnSelfChanged();
            }
            return true;
        }

        // TODO add docs, make more efficient???
        public override void TickUpdate()
        {
            foreach (Cable cable in GetComponents<Cable>())
            {
                cable.TransportResource();
            }
        }

        /// <summary>
        /// Determines if a cable can be added (only when the conduit does not already have a cable of the same type).
        /// </summary>
        /// <param name="cableSpec">The cable tested.</param>
        /// <returns>True if the cable can be added.</returns>
        private bool CanAddCable(CableSpec cableSpec)
        {
            return !GetComponent(cableSpec.GetCableType());
        }

        /// <summary>
        /// Checks if a side is connected.
        /// </summary>
        /// <param name="side">Which side of the conduit to check.</param>
        /// <returns>True if the side is connected.</returns>
        public bool IsSideConnected(Vector2Int side)
        {
            return sideConfigs[side].Item1 == TransportMode.CONNECT;
        }

        /// <summary>
        /// Checks if a side is disconnected.
        /// </summary>
        /// <param name="side">Which side of the conduit to check.</param>
        /// <returns>True if the side is disconnected.</returns>
        public bool IsSideDisconnected(Vector2Int side)
        {
            return GetTransportMode(side) == TransportMode.DISCONNECT;
        }

        /// <summary>
        /// Checks if a side is in extract mode.
        /// </summary>
        /// <param name="side">Which side of the conduit to check.</param>
        /// <returns>True if the side is in extract mode.</returns>
        public bool IsSideExtracting(Vector2Int side)
        {
            return GetTransportMode(side) == TransportMode.EXTRACT;
        }

        public TransportMode GetTransportMode(Vector2Int side)
        {
            return sideConfigs[side].Item1;
        }

        public DistributeMode GetDistributeMode(Vector2Int side)
        {
            return sideConfigs[side].Item2;
        }

        public void SetTransportMode(Vector2Int side, TransportMode mode)
        {
            sideConfigs[side] = (mode, GetDistributeMode(side));
        }

        public void SetDistributeMode(Vector2Int side, DistributeMode mode)
        {
            sideConfigs[side] = (GetTransportMode(side), mode);
        }

        /// <summary>
        /// Gets possible modes for a specific side based on what gridObject is adjacent.
        /// </summary>
        /// <param name="side">The side to check.</param>
        /// <returns>A list of possible transport modes.</returns>
        private List<TransportMode> GetTransportModeRotation(Vector2Int side)
        {
            if (gridObject.GetAdjacentObject<ConduitInterface>(side))
            {
                return new() { TransportMode.CONNECT, TransportMode.DISCONNECT, TransportMode.EXTRACT };
            }
            else if (gridObject.GetAdjacentObject<Conduit>(side))
            {
                return new() { TransportMode.CONNECT, TransportMode.DISCONNECT };
            }
            else
            {
                return new() { TransportMode.DISCONNECT };
            }
        }

    }

    /// <summary>
    /// Stores transport settings for one side of a Conduit.
    /// </summary>
    public class SideConfig
    {
        public TransportMode transportMode;
        public DistributeMode distributeMode;

        public SideConfig()
        {
            transportMode = TransportMode.DISCONNECT;
            distributeMode = DistributeMode.CLOSEST_FIRST;
        }
    }

    /// <summary>
    /// Defines how Cables in this Conduit should interact with an adjacent GridObject
    /// </summary>
    public enum TransportMode
    {
        CONNECT,
        DISCONNECT,
        EXTRACT
    }

    /// <summary>
    /// Defines how Cables in this Conduit distribute resources to multiple outputs.
    /// </summary>
    public enum DistributeMode
    {
        CLOSEST_FIRST,
        ROUND_ROBIN
    }

}
