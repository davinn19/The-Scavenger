using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemCable : Cable<ItemBuffer>
    {
        protected override void TransportResource()
        {
            Dictionary<ItemBuffer, DistributeMode> sources = GetSourcesAndDistributeModes();

            if (sources.Count == 0)
            {
                return;
            }

            List<ItemBuffer> destinations = GetDestinations();

            foreach (ItemBuffer source in sources.Keys)
            {
                switch (sources[source])
                {
                    case DistributeMode.CLOSEST_FIRST:
                        ClosestFirst(source, destinations);
                        break;
                    case DistributeMode.ROUND_ROBIN:
                        ClosestFirst(source, destinations);
                        break;
                    default:
                        Debug.LogError("Something went wrong!!!");
                        break;
                }
            }            
        }

        // Distributes items from an input item buffer to all output item buffers, closer destinations getting filled first
        private void ClosestFirst(ItemBuffer source, List<ItemBuffer> destinations)
        {
            // TODO implement
        }


        // Distributes items from an input item buffer to all output item buffers, distributed evenly
        private void RoundRobin(ItemBuffer source, List<ItemBuffer> destinations)
        {
            // TODO implement
        }

    }
}
