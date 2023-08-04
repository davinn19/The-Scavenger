using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Cable for transporting energy.
    /// </summary>
    public class EnergyCable : Cable<EnergyInterface>
    {
        public override void TransportResource()
        {
            List<EnergyInterface> sources = GetSources();
            if (sources.Count == 0)
            {
                return;
            }

            List<EnergyInterface> destinations = GetDestinations();
            if (destinations.Count == 0)
            {
                return;
            }

            foreach (EnergyInterface source in sources)
            {
                Distribute(source, destinations);
            }
        }


        /// <summary>
        /// Distributes energy from source buffer to destination buffers, capped at the cables transfer rate.
        /// Always closest-first, ignores conduit's distribution mode.
        /// </summary>
        /// <param name="source">Interface to extract energy from.</param>
        /// <param name="destinations">Interfaces to insert energy into.</param>
        private void Distribute(EnergyInterface source, List<EnergyInterface> destinations)
        {
            int availableEnergy = source.Extract(Spec.TransferRate, true);
            int energyTaken = 0;

            foreach (EnergyInterface destination in destinations)
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
