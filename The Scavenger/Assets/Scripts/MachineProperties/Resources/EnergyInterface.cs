using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class EnergyInterface : ConduitInterface<EnergyBuffer>
    {
        [SerializeField] private EnergyBuffer energyBuffer;

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
