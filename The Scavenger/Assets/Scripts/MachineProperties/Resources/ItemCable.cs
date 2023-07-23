using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemCable : Cable<ItemBuffer>
    {
        private int roundRobinIndex = 0;

        protected override void TransportResource()
        {
            Dictionary<ItemBuffer, DistributeMode> sources = GetSourcesAndDistributeModes();
            if (sources.Count == 0)
            {
                return;
            }

            List<ItemBuffer> destinations = GetDestinations();
            if (destinations.Count == 0)
            {
                return;
            }

            foreach (ItemBuffer source in sources.Keys)
            {
                switch (sources[source])
                {
                    case DistributeMode.CLOSEST_FIRST:
                        ClosestFirst(source, destinations);
                        break;
                    case DistributeMode.ROUND_ROBIN:
                        RoundRobin(source, destinations);
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
            // TODO test
            int itemsToTake = Spec.TransferRate;

            foreach (ItemBuffer destination in destinations)
            {
                itemsToTake -= destination.TakeFromBuffer(source, itemsToTake);
                if (itemsToTake <= 0)
                {
                    return;
                }
            }
        }


        // Distributes items from an input item buffer to all output item buffers, distributed evenly
        private void RoundRobin(ItemBuffer source, List<ItemBuffer> destinations)
        {
            // TODO test
            destinations = new List<ItemBuffer>(destinations);
            int itemsToTake = Spec.TransferRate;

            while (itemsToTake > 0 && destinations.Count > 0)
            {
                if (roundRobinIndex < destinations.Count)
                {
                    roundRobinIndex = 0;
                }

                ItemBuffer destination = destinations[roundRobinIndex];
                int itemsTaken = destination.TakeFromBuffer(source, 1);

                if (itemsTaken == 0)
                {
                    destinations.RemoveAt(roundRobinIndex);
                }
                else
                {
                    roundRobinIndex++;
                }
            }
        }
    }
}
