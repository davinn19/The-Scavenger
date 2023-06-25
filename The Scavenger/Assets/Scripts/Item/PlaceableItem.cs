using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "PlaceableObject", menuName = "Scavenger/Placeable Item")]
    public class PlaceableItem : Item
    {
        [field: SerializeField] public GridObject PlacedObject { get; private set; }
    }
}
