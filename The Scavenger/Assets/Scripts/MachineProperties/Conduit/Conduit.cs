using System;
using System.Collections.Generic;
using UnityEngine;


namespace Scavenger
{
    /// <summary>
    /// GridObject that houses cables and defines their connectivity/transport modes.
    /// </summary>
    [RequireComponent(typeof(GridObject))]
    public class Conduit : MonoBehaviour
    {
        private GridObject gridObject;
        private Dictionary<Vector2Int, (TransportMode, DistributeMode)> sideConfigs = new Dictionary<Vector2Int, (TransportMode, DistributeMode)>();


        private void Awake()
        {
            gridObject = GetComponent<GridObject>();

            gridObject.OnPlaced.Add(OnPlaced);
            gridObject.OnRemoved.Add(OnRemoved);

            gridObject.OnNeighborPlaced.Add(OnNeighborPlaced);
            gridObject.OnNeighborChanged.Add(OnNeighborChanged);
            gridObject.Interact = Interact;

            InitSideConfigs();
        }


        /// <summary>
        /// Initializes the conduit's side configs to default values.
        /// </summary>
        private void InitSideConfigs()
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                sideConfigs.Add(side, (TransportMode.DISCONNECT, DistributeMode.CLOSEST_FIRST));
            }
        }

        
        /// <summary>
        /// Sets side configs to default values based on the adjacent GridObject.
        /// </summary>
        private void OnPlaced()
        {
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
                else                        //Otherwise, disconnect.
                {
                    SetTransportMode(side, TransportMode.DISCONNECT);
                }
            }

            gridObject.OnSelfChanged();
        }

        /// <summary>
        /// Disconnects all adjacent conduits before removing.
        /// </summary>
        private void OnRemoved()
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
        private void OnNeighborPlaced(Vector2Int sideUpdated)
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
        private void OnNeighborChanged()
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
        /// Tries to rotate the pressed side's transport mode, or add the held cable.
        /// </summary>
        /// <param name="itemStack">The itemStack used to interact with the conduit.</param>
        /// <param name="sidePressed">The side pressed when interacting.</param>
        private void Interact(ItemStack itemStack, Vector2Int sidePressed)
        {
            bool editSuccessful = TryEditSide(itemStack.Item, sidePressed);

            if (editSuccessful)
            {
                return;
            }

            TryAddCable(itemStack);
        }

        /// <summary>
        /// Tries to rotate a side's transport mode, successful if held item is a conduit.
        /// </summary>
        /// <param name="item">The held item.</param>
        /// <param name="side">The side to attempt changing transport mode.</param>
        /// <returns>True if the edit is successful.</returns>
        private bool TryEditSide(Item item, Vector2Int side)
        {
            PlacedObject property;
            if (item.TryGetProperty(out property) && property.Object.GetComponent<Conduit>())
            {
                RotateTransportMode(side);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to add the held item as a cable to the conduit.
        /// </summary>
        /// <param name="itemStack">The held item.</param>
        /// <returns>True if the held item is added.</returns>
        private bool TryAddCable(ItemStack itemStack)
        {
            CableSpec cableSpec;

            if (itemStack.Item.TryGetProperty(out cableSpec) && CanAddCable(cableSpec))
            {
                cableSpec.AddCable(this);
                itemStack.amount--;     // TODO make proper way to consume items
                gridObject.OnSelfChanged();
                return true;
            }

            return false;
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
        /// Cycles the transport mode for a specific side.
        /// </summary>
        /// <param name="side">The side to edit the transport mode.</param>
        private void RotateTransportMode(Vector2Int side)
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
