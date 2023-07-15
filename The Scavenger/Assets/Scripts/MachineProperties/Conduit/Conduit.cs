using System;
using System.Collections.Generic;
using UnityEngine;


namespace Scavenger
{
    /// <summary>
    /// GridObject that houses Cables and defines their connectivity/transport modes.
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

        // If next to conduit, connect. If next to interface, extract. Otherwise, disconnect.
        private void OnPlaced()
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(side);
                ConduitInterface adjInterface = gridObject.GetAdjacentObject<ConduitInterface>(side);

                if (adjConduit)
                {
                    SetTransportMode(side, TransportMode.CONNECT);
                }
                else if (adjInterface)
                {
                    SetTransportMode(side, TransportMode.EXTRACT);
                }
                else
                {
                    SetTransportMode(side, TransportMode.DISCONNECT);
                }
            }

            gridObject.OnSelfChanged();
        }

        // Disconnect all adjacent conduits before removing.
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

        // If the adjacent object is removed or does not accept conduits, set to disconnect
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

        // Makes sure its side modes align with adjacent conduits
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
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is connected.</returns>
        public bool IsSideConnected(Vector2Int side)
        {
            return sideConfigs[side].Item1 == TransportMode.CONNECT;
        }


        /// <summary>
        /// Checks if a side is disconnected.
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is disconnected.</returns>
        public bool IsSideDisconnected(Vector2Int side)
        {
            return GetTransportMode(side) == TransportMode.DISCONNECT;
        }


        /// <summary>
        /// Checks if a side is in extract mode.
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
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


        private void Interact(ItemStack itemStack, Vector2Int sidePressed)
        {
            if (!TryEditSide(itemStack.item, sidePressed))
            {
                TryAddCable(itemStack);
            }
        }

        private bool TryEditSide(Item item, Vector2Int sidePressed)
        {
            PlacedObject property;
            if (item.TryGetProperty(out property) && property.Object.GetComponent<Conduit>())
            {
                RotateTransportMode(sidePressed);
                return true;
            }

            return false;
        }

        private bool TryAddCable(ItemStack itemStack)
        {
            CableSpec cableSpec;

            if (itemStack.item.TryGetProperty(out cableSpec) && CanAddCable(cableSpec))
            {
                cableSpec.AddCable(this);
                itemStack.Remove();
                gridObject.OnSelfChanged();
                return true;
            }

            return false;
        }

        private bool CanAddCable(CableSpec cableSpec)
        {
            return !GetComponent(cableSpec.GetCableType());
        }

        // Cycles the transport mode for a specific side
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

        // Gets possible modes for a specific side.
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
