using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Energy cell that starts with some amount of energy.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer))]
    public class MakeshiftEnergyCell : EnergyCell, IHasPersistentData
    {
        // TODO add docs
        public void Read(JSON data)
        {
            if (!data.ContainsKey("InitialEnergySet"))
            {
                InitEnergy();
            }
        }

        public void Write(JSON data)
        { 
            data.AddOrReplace("InitialEnergySet", true);
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
