using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [Serializable, CreateAssetMenu(menuName = "Scavenger/CableSpec")]
    public class CableSpec : ItemProperty
    {
        public int transferRate;
    }
}
