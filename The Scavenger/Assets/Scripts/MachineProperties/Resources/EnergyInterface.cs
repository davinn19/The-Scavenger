using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO continue documentation
    [RequireComponent(typeof(EnergyBuffer))]
    public class EnergyInterface : ConduitInterface<EnergyBuffer>
    {
        private EnergyBuffer energyBuffer;

        private void Awake()
        {
            energyBuffer = GetComponent<EnergyBuffer>();
        }

        public override List<EnergyBuffer> GetInputs()
        {
            return GetEnergyBuffer();
        }

        public override List<EnergyBuffer> GetOutputs()
        {
            return GetEnergyBuffer();
        }

        private List<EnergyBuffer> GetEnergyBuffer()
        {
            List<EnergyBuffer> result = new List<EnergyBuffer>();

            if (energyBuffer)
            {
                result.Add(energyBuffer);
            }

            return result;
        }

    }
}
