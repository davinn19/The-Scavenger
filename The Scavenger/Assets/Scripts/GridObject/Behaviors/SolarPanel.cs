using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Passively generates energy into a buffer.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer))]
    public class SolarPanel : GridObjectBehavior
    {
        [SerializeField] private int energyPerTick;

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
            int energyAdded = energyBuffer.Insert(energyPerTick, false);
            if (energyAdded > 0)
            {
                gridObject.OnSelfChanged();
            }
        }
    }
}
