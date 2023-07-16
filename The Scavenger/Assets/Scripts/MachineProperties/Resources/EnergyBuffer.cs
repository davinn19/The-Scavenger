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
        [SerializeField] public int Capacity { get; private set; }
        [SerializeField] public int Energy { get; private set; }


        /// <summary>
        /// Inserts energy into buffer, respecting the buffer's capacity.
        /// </summary>
        /// <param name="amount">Requested amount of energy to insert.</param>
        /// <returns>Amount of energy inserted into the buffer.</returns>
        public int InsertEnergy(int amount)
        {
            int remainingCapacity = GetRemainingCapacity();
            int amountToInsert = Mathf.Min(remainingCapacity, amount);

            Energy += amountToInsert;

            return amountToInsert;
        }

        /// <summary>
        /// Extracts energy from buffer, respecting the buffer's current energy amount.
        /// </summary>
        /// <param name="amount">Requested amount of energy to extract.</param>
        /// <returns>Amount of energy extracted from the buffer.</returns>
        public int ExtractEnergy(int amount)
        {
            int amountToExtract = Mathf.Min(Energy, amount);

            Energy -= amountToExtract;

            return amountToExtract;
        }


        public int GetRemainingCapacity()
        {
            return Capacity - Energy;
        }
    }
}
