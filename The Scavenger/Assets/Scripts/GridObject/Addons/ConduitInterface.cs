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
        protected T Buffer { get; private set; }

        private void Awake()
        {
            Buffer = GetComponent<T>();
        }
    }

}
