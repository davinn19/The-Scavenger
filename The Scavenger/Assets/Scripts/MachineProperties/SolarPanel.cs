using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Passively generates energy into a buffer
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

        private void GenerateEnergy()
        {
            int energyAdded = energyBuffer.InsertEnergy(energyPerTick);
            if (energyAdded > 0)
            {
                gridObject.OnSelfChanged();
            }

            gridObject.QueueTickUpdate(GenerateEnergy);
        }
    }
}
