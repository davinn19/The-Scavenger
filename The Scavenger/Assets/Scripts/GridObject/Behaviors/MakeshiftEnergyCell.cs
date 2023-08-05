using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Energy cell that starts with some amount of energy.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer))]
    public class MakeshiftEnergyCell : EnergyCell
    {
        // TODO add docs
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
            energyBuffer.Energy = (int)(Random.Range(0.3f, 0.6f) * energyBuffer.Capacity);
        }
    }
}
