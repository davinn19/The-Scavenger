using Leguar.TotalJSON;
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

        public void Read(JSON data)
        {
            if (persistent)
            {
                ReadData(data);
            }
        }


        public void Write(JSON data)
        {
            if (persistent)
            {
                WriteData(data);
            }
        }

        protected abstract void ReadData(JSON data);
        protected abstract void WriteData(JSON data);
    }
}
