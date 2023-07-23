using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Specialized conduit interface for energy buffers, since gridObjects cannot have multiple energy buffers.
    /// </summary>
    [RequireComponent(typeof(EnergyBuffer)), DisallowMultipleComponent]
    public class EnergyInterface : ConduitInterface<EnergyBuffer>
    {
        private EnergyBuffer energyBuffer;

        private void Awake()
        {
            energyBuffer = GetComponent<EnergyBuffer>();
        }

        /// <summary>
        /// Gets the only energy buffer.
        /// </summary>
        /// <returns>List containing the only energy buffer.</returns>
        public override List<EnergyBuffer> GetInputs() => GetEnergyBuffer();

        /// <summary>
        /// Gets the only energy buffer.
        /// </summary>
        /// <returns>List containing the only energy buffer.</returns>
        public override List<EnergyBuffer> GetOutputs() => GetEnergyBuffer();

        /// <summary>
        /// Gets the only energy buffer.
        /// </summary>
        /// <returns>List containing the only energy buffer.</returns>
        private List<EnergyBuffer> GetEnergyBuffer()
        {
            return new List<EnergyBuffer>() { energyBuffer };
        }

    }
}
