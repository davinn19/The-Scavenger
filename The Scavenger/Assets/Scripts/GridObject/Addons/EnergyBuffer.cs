using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Buffer that stores energy.
    /// </summary>
    public class EnergyBuffer : Buffer
    {
        [field: SerializeField] public int Capacity { get; set; }
        [field: SerializeField, HideInInspector] public int Energy { get; set; }


        // TODO add docs
        public override void ReadPersistentData(PersistentData data)
        {
            if (data.ContainsKey("Energy"))
            {
                Energy = data.GetInt("Energy");
            }
        }

        // TODO add docs
        public override PersistentData WritePersistentData(PersistentData data)
        {
            data.Add("Energy", Energy);
            return data;
        }

        /// <summary>
        /// Inserts energy into buffer, respecting the buffer's capacity.
        /// </summary>
        /// <param name="amount">Requested amount of energy to insert.</param>
        /// <param name="simulate">If true, will not changed the buffer's energy.</param>
        /// <returns>Amount of energy inserted into the buffer.</returns>
        public int Insert(int amount, bool simulate)
        {
            int remainingCapacity = GetRemainingCapacity();
            int amountToInsert = Mathf.Min(remainingCapacity, amount);

            if (!simulate)
            {
                Energy += amountToInsert;
            }
            

            return amountToInsert;
        }

        /// <summary>
        /// Extracts energy from buffer, respecting the buffer's current energy amount.
        /// </summary>
        /// <param name="amount">Requested amount of energy to extract.</param>
        /// /// <param name="simulate">If true, will not changed the buffer's energy.</param>
        /// <returns>Amount of energy extracted from the buffer.</returns>
        public int Extract(int amount, bool simulate)
        {
            int amountToExtract = Mathf.Min(Energy, amount);

            if (!simulate)
            {
                Energy -= amountToExtract;
            }

            return amountToExtract;
        }

        public int GetRemainingCapacity()
        {
            return Capacity - Energy;
        }
    }
}
