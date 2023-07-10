using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class PlacedObject : ItemPropertyDefinition
    {
        public override string Name => "Placed Object";

        public GridObject placedObject = null;
    }
}
