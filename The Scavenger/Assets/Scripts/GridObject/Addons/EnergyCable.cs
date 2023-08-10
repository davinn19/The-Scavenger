using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Cable for transporting energy.
    /// </summary>
    public class EnergyCable : Cable<EnergyBuffer>
    {
        public override void TransportResource()
        {
            List<EnergyBuffer> sources = GetSources();
            if (sources.Count == 0)
            {
                return;
            }

            List<EnergyBuffer> destinations = GetDestinations();
            if (destinations.Count == 0)
            {
                return;
            }

            foreach (EnergyBuffer source in sources)
            {
                Distribute(source, destinations);
            }
        }

        /// <summary>
        /// Distributes energy from source buffer to destination buffers, capped at the cables transfer rate.
        /// Always closest-first, ignores conduit's distribution mode.
        /// </summary>
        /// <param name="source">Buffer to extract energy from.</param>
        /// <param name="destinations">Buffer to insert energy into.</param>
        private void Distribute(EnergyBuffer source, List<EnergyBuffer> destinations)
        {
            int availableEnergy = source.Extract(Spec.TransferRate, true);
            int energyTaken = 0;

            foreach (EnergyBuffer destination in destinations)
            {
                if (availableEnergy == energyTaken)
                {
                    break;
                }

                if (destination == source)
                {
                    continue;
                }

                energyTaken += destination.Insert(availableEnergy - energyTaken, false);
            }

            source.Extract(energyTaken, false);
        }
    }
}
