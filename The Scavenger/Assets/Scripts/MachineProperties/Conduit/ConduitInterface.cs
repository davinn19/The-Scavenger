using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Nongeneric version of ConduitInterface
    /// </summary>
    public abstract class ConduitInterface : MonoBehaviour
    { 
    
    }


    /// <summary>
    /// Gets input/output Buffers for Conduits
    /// </summary>
    /// <typeparam name="T">The type of Buffer this ConduitInterface works with.</typeparam>
    public abstract class ConduitInterface<T> : ConduitInterface where T : Buffer
    {
        public abstract List<T> GetInputs();

        public abstract List<T> GetOutputs();
    }

}
