using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Energy cell that starts with some amount of energy.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer))]
    public class PrefilledEnergyCell : EnergyCell
    {
        public override void ReadPersistentData(JSON data)
        {
            base.ReadPersistentData(data);

            if (!data.ContainsKey("InitialEnergySet"))
            {
                InitEnergy();
            }
        }

        public override JSON WritePersistentData()
        {
            JSON data =  base.WritePersistentData();
            data.Add("InitialEnergySet", true);

            return data;
        }

        /// <summary>
        /// Sets the energy cell's initial amount after being placed for the first time.
        /// </summary>
        private void InitEnergy()
        {
            energyBuffer.Energy = (int)(Random.Range(0.3f, 0.6f) * energyBuffer.Capacity);
        }
    }
}
