using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Cable for transporting items.
    /// </summary>
    public class ItemCable : Cable<ItemInterface>
    {
        /// <summary>
        /// Persistent tracking of the next destination buffer to output to when in round robin mode.   // TODO adjust for multiple round robin sources
        /// </summary>
        private int roundRobinIndex = 0;

        public override void TransportResource()
        {
            Dictionary<ItemInterface, DistributeMode> sources = GetSourcesAndDistributeModes();
            if (sources.Count == 0)
            {
                return;
            }

            List<ItemInterface> destinations = GetDestinations();
            if (destinations.Count == 0)
            {
                return;
            }

            foreach (ItemInterface source in sources.Keys)
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
        /// <param name="source">Interface to extract items from.</param>
        /// <param name="destinations">List of interfaces to distribute items to.</param>
        private void ClosestFirst(ItemInterface source, List<ItemInterface> destinations)
        {
            int itemsToTake = Spec.TransferRate;

            foreach (ItemInterface destination in destinations)    // Destinations list is ordered by proximity already
            {
                itemsToTake -= source.MoveTo(itemsToTake, destination);

                if (itemsToTake == 0)
                {
                    return;
                }
            }
        }


        /// <summary>
        /// Distributes items from an input item buffer to all output item buffers, distributed evenly.
        /// </summary>
        /// <param name="source">Interface to extract items from.</param>
        /// <param name="destinations">List of interfaces to distribute items to.</param>
        private void RoundRobin(ItemInterface source, List<ItemInterface> destinations)
        {
            // TODO test
            // Create a copy list so destinations that cannot be inserted can be removed to prevent unnecessary looping
            destinations = new List<ItemInterface>(destinations);
            int itemsToTake = Spec.TransferRate;

            while (itemsToTake > 0 && destinations.Count > 0)       // Stop once item limit is reached or all destinations are full
            {
                // Ensure round-robin index is always in bounds as the number of destinations changes
                if (roundRobinIndex < destinations.Count)
                {
                    roundRobinIndex = 0;
                }

                ItemInterface destination = destinations[roundRobinIndex];

                // Try inserting 1 item
                bool itemTaken = source.MoveTo(1, destination) == 1;

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
