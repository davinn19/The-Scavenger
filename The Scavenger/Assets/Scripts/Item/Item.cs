using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private string displayName;
        [SerializeField] private Sprite icon;
        [ItemProperties] public List<ItemProperty> properties = new();


        public string GetDisplayName()
        {
            return displayName;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public ItemProperty GetProperty(Type definition)
        {
            foreach (ItemProperty property in properties)
            {
                if (property.IsDefinedBy(definition))
                {
                    return property;
                }
            }

            return null;
        }

        public T GetProperty<T>() where T : ItemPropertyDefinition => GetProperty(typeof(T)) as T;


        public bool TryGetProperty(Type definition, out ItemProperty property)
        {
            property = GetProperty(definition);
            return property != null;
        }

        public bool TryGetProperty<T>(out T property) where T : ItemPropertyDefinition
        {
            property = GetProperty<T>();
            return property != null;
        }


        public bool HasProperty(Type propertyType) => GetProperty(propertyType) != null;

        public bool HasProperty<T>() where T : ItemPropertyDefinition => GetProperty<T>() != null;


        public T TryAddProperty<T>() where T : ItemPropertyDefinition, new()
        {
            if (!HasProperty<T>())
            {
                return null;
            }

            ItemProperty newProperty = new ItemProperty();
            T definition = newProperty.SetDefinition<T>();

            properties.Add(newProperty);

            return definition;
        }

        public ItemProperty TryAddProperty(Type definition)
        {
            if (HasProperty(definition))
            {
                return null;
            }

            ItemProperty newProperty = new ItemProperty();
            newProperty.SetDefinition(definition);

            properties.Add(newProperty);

            return newProperty;
        }

        // TODO decide which functions should be private
    }

}
