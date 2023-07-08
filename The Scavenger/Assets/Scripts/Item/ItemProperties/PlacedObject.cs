using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [Serializable, CreateAssetMenu(menuName = "Scavenger/PlacedObject")]
    public class PlacedObject : ItemProperty
    {
        public GridObject placedObject;
    }
}
