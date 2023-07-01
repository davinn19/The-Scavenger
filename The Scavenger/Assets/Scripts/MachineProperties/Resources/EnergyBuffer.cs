using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Buffer that stores energy.
    /// </summary>
    public class EnergyBuffer : Buffer
    {
        [SerializeField] private int capacity;
        private int energy = 0;


        /// <summary>
        /// Inserts energy into buffer, respecting the buffer's capacity.
        /// </summary>
        /// <param name="amount">Requested amount of energy to insert.</param>
        /// <returns>Amount of energy inserted into the buffer.</returns>
        public int InsertEnergy(int amount)
        {
            int remainingCapacity = GetRemainingCapacity();
            int amountToInsert = Mathf.Min(remainingCapacity, amount);

            energy += amountToInsert;

            return amountToInsert;
        }

        /// <summary>
        /// Extracts energy from buffer, respecting the buffer's current energy amount.
        /// </summary>
        /// <param name="amount">Requested amount of energy to extract.</param>
        /// <returns>Amount of energy extracted from the buffer.</returns>
        public int ExtractEnergy(int amount)
        {
            int amountToExtract = Mathf.Min(energy, amount);

            energy -= amountToExtract;

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
