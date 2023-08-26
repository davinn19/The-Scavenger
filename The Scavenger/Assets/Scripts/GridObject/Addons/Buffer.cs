using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Base class for resource storage.
    /// </summary>
    public abstract class Buffer : MonoBehaviour, IHasPersistentData
    {
        public abstract void ReadPersistentData(JSON data);
        public abstract JSON WritePersistentData();
    }
}
