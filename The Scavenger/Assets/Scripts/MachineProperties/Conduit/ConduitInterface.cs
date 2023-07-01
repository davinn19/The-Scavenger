using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public abstract class ConduitInterface : MonoBehaviour
    { 
    
    }

    public abstract class ConduitInterface<T> : ConduitInterface where T : Buffer
    {
        public abstract List<T> GetInputs();

        public abstract List<T> GetOutputs();
    }

}
