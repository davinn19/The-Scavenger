using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, implement

    [RequireComponent(typeof(EnergyBuffer))]
    public class EnergyCell : GridObjectBehavior
    {
        protected EnergyBuffer energyBuffer;

        [SerializeField, Min(0)] 
        private int maxInsertLimit;

        [SerializeField, Min(0)]
        protected int maxExtractLimit;

        // TODO add docs
        protected override void Init()
        {
            base.Init();

            energyBuffer = GetComponent<EnergyBuffer>();
            energyBuffer.InsertLimit = maxInsertLimit;
            energyBuffer.ExtractLimit = maxExtractLimit;
        }

        protected override void TickUpdate()
        {
            base.TickUpdate();
            energyBuffer.ResetEnergyTransferCount();
        }

        public override void ReadPersistentData(JSON data)
        {
            base.ReadPersistentData(data);

            if (data.ContainsKey("EnergyBuffer"))
            {
                energyBuffer.ReadPersistentData(data.GetJSON("EnergyBuffer"));
            }
            
            if (data.ContainsKey("InsertLimit"))
            {
                energyBuffer.InsertLimit = data.GetInt("InsertLimit");
            }

            if (data.ContainsKey("ExtractLimit"))
            {
                energyBuffer.ExtractLimit = data.GetInt("ExtractLimit");
            }
        }

        public override JSON WritePersistentData()
        {
            JSON data = base.WritePersistentData();

            JSONHelper.TryAdd(data, "EnergyBuffer", energyBuffer.WritePersistentData());

            if (energyBuffer.InsertLimit != maxInsertLimit)
            {
                data.Add("InsertLimit", energyBuffer.InsertLimit);
            }

            if (energyBuffer.ExtractLimit != maxExtractLimit)
            {
                data.Add("ExtractLimit", energyBuffer.ExtractLimit);
            }

            return data;
        }

    }
}
