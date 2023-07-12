using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class PlacedObject : ItemProperty
    {
        public override string Name => "Placed Object";

        [field: SerializeField] public GridObject Object { get; private set; }

    }
}
