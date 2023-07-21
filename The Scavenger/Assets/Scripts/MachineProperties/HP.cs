using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class HP : MonoBehaviour
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
    }

}

