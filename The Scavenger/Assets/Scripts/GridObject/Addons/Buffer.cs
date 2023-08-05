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

        public void ReadPersistentData(PersistentData data)
        {
            if (persistent)
            {
                ReadData(data);
            }
        }


        public PersistentData WritePersistentData(PersistentData data)
        {
            if (persistent)
            {
                WriteData(data);
            }
            return data;
        }

        protected abstract void ReadData(PersistentData data);
        protected abstract void WriteData(PersistentData data);
    }
}
