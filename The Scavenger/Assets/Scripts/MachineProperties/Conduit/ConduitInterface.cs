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

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

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
        INPUT,
        OUTPUT
    }

    public class SideConfig
    {
        TransportMode transportMode;
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
