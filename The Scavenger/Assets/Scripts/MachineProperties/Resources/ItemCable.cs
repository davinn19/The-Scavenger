using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Cable for transporting items.
    /// </summary>
    public class ItemCable : Cable<ItemBuffer>
    {
        /// <summary>
        /// Persistent tracking of the next destination buffer to output to when in round robin mode.   // TODO adjust for multiple round robin sources
        /// </summary>
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


        /// <summary>
        /// Distributes items from an input item buffer to all output item buffers, closer destinations getting filled first.
        /// </summary>
        /// <param name="source">Buffer to extract items from.</param>
        /// <param name="destinations">List of buffers to distribute items to.</param>
        private void ClosestFirst(ItemBuffer source, List<ItemBuffer> destinations)
        {
            int itemsToTake = Spec.TransferRate;

            foreach (ItemBuffer destination in destinations)    // Destinations list is ordered by proximity already
            {
                for (int sourceSlot = 0; sourceSlot < source.NumSlots; sourceSlot++)
                {
                    ItemStack sourceStack = source.GetItemInSlot(sourceSlot);
                    if (!sourceStack)
                    {
                        continue;
                    }

                    int remainder = destination.Insert(sourceStack, itemsToTake);
                    int amountTaken = sourceStack.Amount - remainder;

                    source.Extract(sourceSlot, amountTaken);
                    itemsToTake -= amountTaken;
                    
                    if (itemsToTake <= 0)
                    {
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// Distributes items from an input item buffer to all output item buffers, distributed evenly.
        /// </summary>
        /// <param name="source">Buffer to extract items from.</param>
        /// <param name="destinations">List of buffers to distribute items to.</param>
        private void RoundRobin(ItemBuffer source, List<ItemBuffer> destinations)
        {
            // TODO test
            // Create a copy list so destinations that cannot be inserted can be removed to prevent unnecessary looping
            destinations = new List<ItemBuffer>(destinations);
            int itemsToTake = Spec.TransferRate;

            while (itemsToTake > 0 && destinations.Count > 0)       // Stop once item limit is reached or all destinations are full
            {
                // Ensure round-robin index is always in bounds as the number of destinations changes
                if (roundRobinIndex < destinations.Count)
                {
                    roundRobinIndex = 0;
                }

                ItemBuffer destination = destinations[roundRobinIndex];

                // Loop through source slot until an item is inserted
                bool itemTaken = false;
                for (int sourceSlot = 0; sourceSlot < source.NumSlots; sourceSlot++)
                {
                    ItemStack sourceStack = source.GetItemInSlot(sourceSlot);
                    if (!sourceStack)
                    {
                        continue;
                    }

                    // Only inserts 1 item at a time
                    int remainder = destination.Insert(sourceStack, 1); 
                    if (remainder != sourceStack.Amount)
                    {
                        itemTaken = true;
                        break;
                    }
                }

                if (itemTaken)  // Continue looping if item was taken
                {
                    itemsToTake--;
                    roundRobinIndex++;
                }
                else            // Otherwise, remove destination from looping
                {
                    destinations.RemoveAt(roundRobinIndex);
                }
            }
        }
    }
}
