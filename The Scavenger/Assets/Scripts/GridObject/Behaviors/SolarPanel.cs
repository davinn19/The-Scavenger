using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Passively generates energy into a buffer.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer))]
    public class SolarPanel : GridObjectBehavior
    {
        [SerializeField] private int energyGainedPerTick;

        private EnergyBuffer energyBuffer;

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            energyBuffer = GetComponent<EnergyBuffer>();
        }

        /// <summary>
        /// Generates some amount of energy per tick.
        /// </summary>
        protected override void TickUpdate()
        {
            int energyAdded = energyBuffer.Insert(energyGainedPerTick, false);
            if (energyAdded > 0)
            {
                gridObject.OnSelfChanged();
            }
        }

        public override void ReadPersistentData(JSON data)
        {
            base.ReadPersistentData(data);
            if (data.ContainsKey("EnergyBuffer"))
            {
                energyBuffer.ReadPersistentData(data.GetJSON("EnergyBuffer"));
            }
        }

        public override JSON WritePersistentData()
        {
            JSON data = base.WritePersistentData();

            JSONHelper.TryAdd(data, "EnergyBuffer", energyBuffer.WritePersistentData());

            return data;
        }
    }
}
