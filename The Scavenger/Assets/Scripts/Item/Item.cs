using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        public static readonly Type[] PropertyTypes = { typeof(PlacedObject), typeof(CableSpec) };
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [SerializeField, ItemProperties] private List<ItemProperty> properties;


        public string GetDisplayName()
        {
            return displayName;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        [ExecuteInEditMode]
        public void AddProperty(ItemProperty newProperty)
        {
            properties.Add(newProperty);
        }
    }

}
