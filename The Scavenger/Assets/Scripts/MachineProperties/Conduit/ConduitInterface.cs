using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ConduitInterface : MonoBehaviour
    {
        private Dictionary<Vector2Int, SideConfig> sideConfigurations = new Dictionary<Vector2Int, SideConfig>();

        private void Awake()
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                sideConfigurations.Add(side, new SideConfig());
            }
        }

        public SideConfig GetSideConfig(Vector2Int side)
        {
            return sideConfigurations[side];
        }
    }

    public enum FilterMode
    { 
        BLACKLIST,
        WHITELIST
    }

    public enum TransportMode
    {
        NONE,
        INSERT,
        EXTRACT
    }

    public class SideConfig
    {
        public TransportMode transportMode { get; private set; }
        Buffer buffer;
        FilterMode filterMode;
        List<ItemStack> filterItems;

        public SideConfig()
        {
            transportMode = TransportMode.NONE;
            buffer = null;
            filterMode = FilterMode.BLACKLIST;
        }
    }

}
