using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class CableSpec : ItemPropertyDefinition
    {
        public override string Name => "Cable Spec";

        public int transferRate = 0;
    }
}
