using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scavenger.GridObjectBehaviors;

namespace Scavenger
{
    // TODO add docs, change namespace?
    public abstract class Model : MonoBehaviour
    {
        public abstract void Load(GridObjectBehavior behavior);
        public abstract void UpdateAppearance();
    }
}
