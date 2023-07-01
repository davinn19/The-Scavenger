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

            gridObject.OnNeighborPlaced.Add(OnNeighborPlaced);
            gridObject.OnNeighborChanged.Add(OnNeighborChanged);

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

        
        private bool OnNeighborPlaced(Vector2Int sideUpdated)
        {
            // If the adjacent object is removed or accepts conduits, reset the side's mode

            GridObject adjObject = gridObject.GetAdjacentObject(sideUpdated);

            SideConfig oldSideConfig = GetSideConfig(sideUpdated);

            if (!adjObject || adjObject.GetComponent<ConduitInterface>() || adjObject.GetComponent<Conduit>())
            {
                SetTransportMode(sideUpdated, TransportMode.NORMAL);
            }

            return oldSideConfig != GetSideConfig(sideUpdated);
        }

        
        private bool OnNeighborChanged()
        {
            // Makes sure its side modes align with adjacent conduits

            bool changed = false;

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                TransportMode oldTransportMode = GetSideConfig(side).transportMode;
                TransportMode newTransportMode = oldTransportMode;

                GridObject adjObject = gridObject.GetAdjacentObject(side);

                if (!adjObject)
                {
                    newTransportMode = TransportMode.NORMAL;
                }
                else
                {
                    Conduit otherConduit = adjObject.GetComponent<Conduit>();

                    if (otherConduit)
                    {
                        Vector2Int oppositeSide = side * -1;
                        newTransportMode = otherConduit.GetSideConfig(oppositeSide).transportMode;
                    }
                }

                SetTransportMode(side, newTransportMode);

                if (oldTransportMode != newTransportMode)
                {
                    changed = true;
                }
            }
            return changed;
        }


        /// <summary>
        /// Checks if a side is next to something connectible (ConduitInterface, Conduit).
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is next to something connectible.</returns>
        public bool IsSideConnected(Vector2Int side)
        {
            if (IsSideDisabled(side))
            {
                return false;
            }

            GridObject adjObject = gridObject.GetAdjacentObject(side);

            if (!adjObject)
            {
                return false;
            }

            return adjObject.GetComponent<ConduitInterface>() || adjObject.GetComponent<Conduit>();
        }


        /// <summary>
        /// Checks if a side is disabled.
        /// </summary>
        /// <param name="side">Which side of the Conduit to check.</param>
        /// <returns>True if the side is disabled.</returns>
        public bool IsSideDisabled(Vector2Int side)
        {
            return sideConfigs[side].transportMode == TransportMode.DISABLED;
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

        }


        public void SetDistributeMode(Vector2Int side, DistributeMode mode)
        {
            sideConfigs[side].distributeMode = mode;
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
            transportMode = TransportMode.NORMAL;
            distributeMode = DistributeMode.CLOSEST_FIRST;
        }
    }

    /// <summary>
    /// Defines how Cables in this Conduit should interact with an adjacent GridObject
    /// </summary>
    public enum TransportMode
    {
        NORMAL,
        EXTRACT,
        DISABLED
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
