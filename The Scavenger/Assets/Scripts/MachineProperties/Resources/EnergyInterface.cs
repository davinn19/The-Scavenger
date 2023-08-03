using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Conduit interface for energy buffers.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer)), DisallowMultipleComponent]// TODO add docs
    public class EnergyInterface : ConduitInterface<EnergyBuffer>
    {
        private GridObject gridObject;

        [SerializeField, Min(0)] private int insertRateLimit;
        [SerializeField, Min(0)] private int extractRateLimit;

        private int energyInsertedThisTick = 0;
        private int energyExtractedThisTick = 0;


        private void Start()
        {
            gridObject = GetComponent<GridObject>();
            gridObject.QueueTickUpdate(ResetEnergyTransferCount);
        }

        private void ResetEnergyTransferCount()
        {
            energyInsertedThisTick = 0;
            energyExtractedThisTick = 0;
            gridObject.QueueTickUpdate(ResetEnergyTransferCount);
        }

        public int Insert(int requestedAmount, bool simulate)
        {
            int limit = Mathf.Max(0, insertRateLimit - energyInsertedThisTick);
            int amountInserted = Buffer.Insert(Mathf.Min(requestedAmount, limit), simulate);

            energyInsertedThisTick += amountInserted;
            return amountInserted;
        }

        public int Extract(int requestedAmount, bool simulate)
        {
            int limit = Mathf.Max(0, extractRateLimit - energyExtractedThisTick);
            int amountExtracted = Buffer.Extract(Mathf.Min(requestedAmount, limit), simulate);

            energyInsertedThisTick += amountExtracted;
            return amountExtracted;
        }
    }
}
