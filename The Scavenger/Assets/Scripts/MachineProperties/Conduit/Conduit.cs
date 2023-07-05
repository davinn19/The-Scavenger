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
        private Dictionary<Vector2Int, SideConfig> sideConfigs = new Dictionary<Vector2Int, SideConfig>();


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
                sideConfigs.Add(side, new SideConfig());
            }
        }

        // If next to conduit, auto connect. If next to interface, auto extract. Otherwise, disconnect.
        private void OnPlaced()
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                Vector2Int oppositeSide = side * -1;
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
        private bool OnNeighborPlaced(Vector2Int sideUpdated)
        {
            Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(sideUpdated);
            ConduitInterface adjInterface = gridObject.GetAdjacentObject<ConduitInterface>(sideUpdated);

            SideConfig oldSideConfig = GetSideConfig(sideUpdated);

            if (adjConduit)
            {
                SetTransportMode(sideUpdated, TransportMode.CONNECT);
            }
            else if (adjInterface)
            {
                SetTransportMode(sideUpdated, TransportMode.EXTRACT);
            }
            else
            {
                SetTransportMode(sideUpdated, TransportMode.DISCONNECT);
            }

            return oldSideConfig != GetSideConfig(sideUpdated);
        }

        // Makes sure its side modes align with adjacent conduits
        private bool OnNeighborChanged()
        {
            bool changed = false;

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                TransportMode oldTransportMode = GetSideConfig(side).transportMode;
                TransportMode newTransportMode = oldTransportMode;

                Conduit adjConduit = gridObject.GetAdjacentObject<Conduit>(side);

                if (!adjConduit)
                {
                    continue;
                }

                newTransportMode = adjConduit.GetSideConfig(side * -1).transportMode;
                SetTransportMode(side, newTransportMode);

                if (oldTransportMode != newTransportMode)
                {
                    changed = true;
                }
            }
            return changed;
        }


        /// <summary>
        /// Checks if a side is connected.
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is connected.</returns>
        public bool IsSideConnected(Vector2Int side)
        {
            return sideConfigs[side].transportMode == TransportMode.CONNECT;
        }


        /// <summary>
        /// Checks if a side is disconnected.
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is disconnected.</returns>
        public bool IsSideDisconnected(Vector2Int side)
        {
            return sideConfigs[side].transportMode == TransportMode.DISCONNECT;
        }


        /// <summary>
        /// Checks if a side is in extract mode.
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is in extract mode.</returns>
        public bool IsSideExtracting(Vector2Int side)
        {
            return sideConfigs[side].transportMode == TransportMode.EXTRACT;
        }


        public SideConfig GetSideConfig(Vector2Int side)
        {
            return sideConfigs[side];
        }


        public void SetTransportMode(Vector2Int side, TransportMode mode)
        {
            sideConfigs[side].transportMode = mode;
            Debug.Log(mode);
        }


        public void SetDistributeMode(Vector2Int side, DistributeMode mode)
        {
            sideConfigs[side].distributeMode = mode;
        }


        private bool Interact(ItemStack itemStack, Vector2Int sidePressed)
        {
            return RotateTransportMode(sidePressed);
            // TODO implement adding cables
        }


        // Cycles the transport mode for a specific side
        private bool RotateTransportMode(Vector2Int side)
        {
            TransportMode oldTransportMode = GetSideConfig(side).transportMode;

            List<TransportMode> rotation = GetTransportModeRotation(side);

            int index = rotation.IndexOf(oldTransportMode) + 1;

            if (index >= rotation.Count)
            {
                index = 0;
            }

            TransportMode newTransportMode = rotation[index];

            SetTransportMode(side, newTransportMode);

            return oldTransportMode != newTransportMode;
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
