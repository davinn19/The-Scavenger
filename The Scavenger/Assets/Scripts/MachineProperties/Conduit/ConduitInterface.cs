using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Nongeneric version of ConduitInterface.
    /// </summary>
    public abstract class ConduitInterface : MonoBehaviour { }


    /// <summary>
    /// Gets input/output buffers for conduits.
    /// </summary>
    /// <typeparam name="T">The type of Buffer this ConduitInterface works with.</typeparam>
    public abstract class ConduitInterface<T> : ConduitInterface where T : Buffer
    {
        /// <summary>
        /// Gets available input buffers.
        /// </summary>
        /// <returns>List of input buffers.</returns>
        public abstract List<T> GetInputs();

        /// <summary>
        /// Gets available output buffers.
        /// </summary>
        /// <returns>List of output buffers.</returns>
        public abstract List<T> GetOutputs();
    }

}
