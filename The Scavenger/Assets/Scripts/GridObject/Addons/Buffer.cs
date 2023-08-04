using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Base class for resource storage.
    /// </summary>
    public abstract class Buffer : MonoBehaviour, IHasPersistentData
    {
        [SerializeField] protected bool persistent = true;
        // TODO add docs
        public abstract void ReadPersistentData(PersistentData data);
        public abstract PersistentData WritePersistentData(PersistentData data);
    }
}
