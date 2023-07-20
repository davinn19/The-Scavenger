using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Cable for transporting energy.
    /// </summary>
    public class EnergyCable : Cable<EnergyBuffer>
    {
        protected override void TransportResource()
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
        /// <param name="destinations">Buffers to insert energy into.</param>
        private void Distribute(EnergyBuffer source, List<EnergyBuffer> destinations)
        {
            int availableEnergy = Mathf.Min(source.Energy, Spec.TransferRate);
            int energyTaken = 0;

            foreach (EnergyBuffer destination in destinations)
            {
                if (availableEnergy == energyTaken)
                {
                    break;
                }

                energyTaken += destination.InsertEnergy(availableEnergy - energyTaken);
            }

            source.ExtractEnergy(energyTaken);
        }
    }
}
