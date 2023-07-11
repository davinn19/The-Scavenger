using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private List<ItemProperty> properties = new() { };

        public string GetDisplayName()
        {
            return GetProperty<Basic>().displayName;
        }

        public Sprite GetIcon()
        {
            return GetProperty<Basic>().icon;
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

        public T GetProperty<T>() where T : ItemPropertyDefinition
        {
            ItemProperty itemProperty = GetProperty(typeof(T));

            if (itemProperty == null)
            {
                return null;
            }

            return itemProperty.definition as T;
        }


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

        public bool HasProperty<T>() where T : ItemPropertyDefinition
        {
            T hey = GetProperty<T>();
            return hey != null;
        }

        public bool TryAddProperty(Type definition)
        {
            if (HasProperty(definition))
            {
                return false;
            }

            string path = "Assets/Prefabs/Item/Data/" + GetInstanceID() + "/" + definition.Name + ".asset";

            ItemProperty newProperty = CreateInstance<ItemProperty>();
            ItemPropertyDefinition definitionInstance = newProperty.InitDefinition(definition);

            AssetDatabase.CreateAsset(newProperty, path);
            AssetDatabase.AddObjectToAsset(definitionInstance, definitionInstance);
            properties.Add(newProperty);

            return true;
        }

        public bool TryAddProperty<T>() where T : ItemPropertyDefinition, new() => TryAddProperty(typeof(T));


        public void OnDestroy()
        {
            foreach (ItemProperty itemProperty in properties)
            {
                string path = "Assets/Prefabs/Item/Data/" + GetInstanceID() + "/" + itemProperty.definition.Name + ".asset";
                AssetDatabase.DeleteAsset(path);
            }
        }
        // TODO decide which functions should be private
    }

}
