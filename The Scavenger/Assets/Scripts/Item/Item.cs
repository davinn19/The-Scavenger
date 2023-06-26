using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set;  }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}
