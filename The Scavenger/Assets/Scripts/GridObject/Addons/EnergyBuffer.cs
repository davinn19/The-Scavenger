using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Buffer that stores energy.
    /// </summary>
    /// <remarks>If being used by a GridObjectBehavior, make sure to call ResetEnergyTransferCount every tick update.</remarks>
    public class EnergyBuffer : Buffer
    {
        [field: SerializeField, Min(0)] public int Capacity { get; set; }
        [field: SerializeField, Min(0), Readonly] public int Energy { get; set; }

        [field: SerializeField, Min(0)]
        [Tooltip("Setting this in the inspector will be ignored if controlled by an energy cell.")]
        public int InsertLimit { get; set; }

        [field: SerializeField, Min(0)]
        [Tooltip("Setting this in the inspector will be ignored if controlled by an energy cell.")]
        public int ExtractLimit { get; set; }

        private int extractedThisTick = 0;
        private int insertedThisTick = 0;

        /// <summary>
        /// Inserts energy into buffer, respecting the buffer's capacity.
        /// </summary>
        /// <param name="amount">Requested amount of energy to insert.</param>
        /// <param name="simulate">If true, will not changed the buffer's energy.</param>
        /// <returns>Amount of energy inserted into the buffer.</returns>
        public virtual int Insert(int amount, bool simulate)
        {
            int remainingCapacity = GetRemainingCapacity();
            int amountToInsert = Mathf.Min(new int[] { remainingCapacity, amount, GetInsertableAmount() });

            if (!simulate)
            {
                Energy += amountToInsert;
                insertedThisTick += amountToInsert;
            }

            return amountToInsert;
        }

        /// <summary>
        /// Extracts energy from buffer, respecting the buffer's current energy amount.
        /// </summary>
        /// <param name="amount">Requested amount of energy to extract.</param>
        /// /// <param name="simulate">If true, will not changed the buffer's energy.</param>
        /// <returns>Amount of energy extracted from the buffer.</returns>
        public virtual int Extract(int amount, bool simulate)
        {
            int amountToExtract = Mathf.Min(new int[] { Energy, amount, GetExtractableAmount() });

            if (!simulate)
            {
                Energy -= amountToExtract;
                extractedThisTick += amountToExtract;
            }

            return amountToExtract;
        }

        /// <summary>
        /// Removes energy from buffer, ignoring current energy amount and not counting towards transfer limits.
        /// </summary>
        /// <remarks>Intended to be used by machines spending energy internally.</remarks>
        /// <param name="amount">Amount of energy to spend.</param>
        public void Spend(int amount)
        {
            Energy -= Mathf.Min(Energy, amount);
        }

        public void ResetEnergyTransferCount()
        {
            extractedThisTick = 0;
            insertedThisTick = 0;
        }

        public int GetRemainingCapacity()
        {
            return Capacity - Energy;
        }

        // TODO add docs
        private int GetInsertableAmount()
        {
            return Mathf.Max(0, InsertLimit - insertedThisTick);
        }

        private int GetExtractableAmount()
        {
            return Mathf.Max(0, ExtractLimit - extractedThisTick);
        }

        public override JSON WritePersistentData()
        {
            if (Energy > 0)
            {
                JSON data = new JSON();
                data.Add("Energy", Energy);
                return data;
            }

            return null;
        }

        public override void ReadPersistentData(JSON data)
        {
            Energy = data.GetInt("Energy");
        }

    }
}
