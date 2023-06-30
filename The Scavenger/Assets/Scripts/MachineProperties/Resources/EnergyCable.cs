using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(Conduit))]
    public class EnergyCable : Cable
    {
        [SerializeField] private int transferRate;

        public void Load(Item cableSpecs)
        {
            // TODO implement
        }

        private void TickUpdate()
        {
            List<EnergyBuffer> destinations = GetConnectedOutputs<EnergyCable, EnergyBuffer>();

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

        // Distributes energy from an input energy buffer to all output energy buffers, closer destinations getting filled first
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

        public bool Equals(EnergyCable other)
        {
            return other.transferRate == transferRate;
        }
        
    }
}
