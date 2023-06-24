using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;       // TODO combine with placedObject's sprite, differentiate between placable and nonplacable
        [SerializeField] private GridObject placedObject;
    }
}
