using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO rename file & namespace, add docs
    public interface IHasPersistentData
    {
        public abstract void ReadPersistentData(JSON data);
        public abstract JSON WritePersistentData();
    }
}
