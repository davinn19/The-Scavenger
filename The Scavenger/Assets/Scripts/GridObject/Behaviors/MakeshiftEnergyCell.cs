using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Energy cell that starts with some amount of energy.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer))]
    public class MakeshiftEnergyCell : EnergyCell
    {
        /// <summary>
        /// Checks if the energy cell has been placed down before.
        /// </summary>
        /// <param name="data"></param>
        public override void ReadPersistentData(PersistentData data)
        {
            base.ReadPersistentData(data);
            if (!data.ContainsKey("Energy"))
            {
                InitEnergy();
            }
        }

        /// <summary>
        /// Sets the energy cell's initial amount after being placed for the first time.
        /// </summary>
        private void InitEnergy()
        {
            energyBuffer.Energy = (int)(Random.Range(0.5f, 0.75f) * energyBuffer.Capacity);
        }
    }
}
