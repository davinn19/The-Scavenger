using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class CableSpec : ItemProperty
    {
        public override string Name => "Cable Spec";

        [field: SerializeField] public int TransferRate { get; private set; }

    }
}
