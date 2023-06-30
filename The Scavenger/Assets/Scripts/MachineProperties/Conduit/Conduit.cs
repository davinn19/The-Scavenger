using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
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

        private void InitSideConfigs()
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                sideConfigs.Add(side, new SideConfig());
            }
        }

        // If the adjacent object is removed or accepts conduits, reset the side's mode
        private bool OnNeighborPlaced(Vector2Int sideUpdated)
        {
            GridObject adjObject = gridObject.GetAdjacentObject(sideUpdated);

            SideConfig oldSideConfig = GetSideConfig(sideUpdated);

            if (!adjObject || adjObject.GetComponent<ConduitInterface>() || adjObject.GetComponent<Conduit>())
            {
                SetTransportMode(sideUpdated, TransportMode.NORMAL);
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

        // Checks if a side is connected to something
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

        public bool IsSideDisabled(Vector2Int side)
        {
            return sideConfigs[side].transportMode == TransportMode.DISABLED;
        }

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

    public enum TransportMode
    {
        NORMAL,
        EXTRACT,
        DISABLED
    }

    public enum DistributeMode
    {
        CLOSEST_FIRST,
        ROUND_ROBIN
    }

}
