using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Passively generates energy into a buffer.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer), typeof(GridObject))]
    public class SolarPanel : MonoBehaviour
    {
        [SerializeField] private int energyPerTick;

        private EnergyBuffer energyBuffer;
        private GridObject gridObject;

        private void Awake()
        {
            energyBuffer = GetComponent<EnergyBuffer>();
            gridObject = GetComponent<GridObject>();
        }

        private void Start()
        {
            gridObject.QueueTickUpdate(GenerateEnergy);
        }

        /// <summary>
        /// Generates some amount of energy per tick.
        /// </summary>
        private void GenerateEnergy()
        {
            int energyAdded = energyBuffer.Insert(energyPerTick, false);
            if (energyAdded > 0)
            {
                gridObject.OnSelfChanged();
            }

            gridObject.QueueTickUpdate(GenerateEnergy);
        }
    }
}
