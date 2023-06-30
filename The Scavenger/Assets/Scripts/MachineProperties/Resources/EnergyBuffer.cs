using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class EnergyBuffer : Buffer
    {
        [SerializeField] private int capacity;
        private int energy = 0;


        public int InsertEnergy(int amount, bool simulate = false)
        {
            int remainingCapacity = GetRemainingCapacity();
            int amountToInsert = Mathf.Min(remainingCapacity, amount);

            if (!simulate)
            {
                energy += amountToInsert;
            }

            return amountToInsert;
        }

        public int ExtractEnergy(int amount, bool simulate = false)
        {
            int amountToExtract = Mathf.Min(energy, amount);

            if (!simulate)
            {
                energy -= amountToExtract;
            }

            return amountToExtract;
        }

        public int GetEnergy()
        {
            return energy;
        }

        public int GetCapacity()
        {
            return capacity;
        }

        public int GetRemainingCapacity()
        {
            return capacity - energy;
        }
    }
}
