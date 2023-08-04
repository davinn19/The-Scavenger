using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO rename file & namespace, add docs

    public interface IHasPersistentData
    {
        public abstract void ReadPersistentData(PersistentData data);
        public abstract PersistentData WritePersistentData(PersistentData data);
    }
}
