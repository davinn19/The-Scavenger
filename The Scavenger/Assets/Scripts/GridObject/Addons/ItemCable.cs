using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
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

        public override void TransportResource()
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

        // TODO redo

        /// <summary>
        /// Distributes items from an input item buffer to all output item buffers, closer destinations getting filled first.
        /// </summary>
        /// <param name="source">Buffer to extract items from.</param>
        /// <param name="destinations">List of buffers to distribute items to.</param>
        private void ClosestFirst(ItemBuffer source, List<ItemBuffer> destinations)
        {
            int itemsTaken = 0;
            List<ItemStack> sourceSlots = source.GetOutputSlots();

            foreach (ItemBuffer destination in destinations)    // Destinations list is ordered by proximity already
            {
                List<ItemStack> destinationSlots = destination.GetInputSlots();
                itemsTaken += ItemTransfer.MoveBufferToBuffer(sourceSlots, destinationSlots, Spec.TransferRate - itemsTaken, destination.AcceptsItemStack);

                if (itemsTaken == Spec.TransferRate)
                {
                    return;
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
            List<ItemStack> sourceSlots = source.GetOutputSlots();

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
                List<ItemStack> destinationSlots = destination.GetInputSlots();

                // Try inserting 1 item
                bool itemTaken = ItemTransfer.MoveBufferToBuffer(sourceSlots, destinationSlots, 1, destination.AcceptsItemStack) == 1;     // TODO Consider making a more efficient function for moving just 1 item

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
