using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Cable for transporting energy.
    /// </summary>
    [RequireComponent(typeof(Conduit))]
    public class EnergyCable : Cable<EnergyBuffer>
    {
        public void Load(Item cableSpecs)
        {
            // TODO implement
        }

        private void TickUpdate()
        {
            List<EnergyBuffer> destinations = GetConnectedOutputs();

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                EnergyBuffer source = gridObject.GetAdjacentObject<EnergyBuffer>(side);

                if (!source || !conduit.IsSideExtracting(side))
                {
                    continue;
                }

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
            int availableEnergy = Mathf.Min(source.GetEnergy(), transferRate);
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
