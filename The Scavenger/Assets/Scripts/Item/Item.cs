using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField, Multiline] public string Description { get; private set; }

        [SerializeField] private List<ItemProperty> properties = new() { };


        public bool Interact(GameManager gameManager, int inventorySlot, Vector2Int pressedPos)
        {
            if (TryGetProperty(out Interactibe interactProperty))
            {
                interactProperty.Interact(gameManager, inventorySlot, pressedPos);
                return true;
            }
            return false;
        }


        public ItemProperty GetProperty(Type propertyType)
        {
            foreach (ItemProperty property in properties)
            {
                if (property.GetType() == propertyType)
                {
                    return property;
                }
            }

            return null;
        }

        public T GetProperty<T>() where T : ItemProperty
        {
            foreach (ItemProperty property in properties)
            {
                if (property is T)
                {
                    return property as T;
                }
            }

            return null;
        }

        public bool TryGetProperty<T>(out T property) where T : ItemProperty
        {
            property = GetProperty<T>();
            return property != null;
        }

        public bool HasProperty(Type propertyType) => GetProperty(propertyType) != null;

        public bool HasProperty<T>() where T : ItemProperty
        {
            T hey = GetProperty<T>();
            return hey != null;
        }

        public ItemProperty[] GetProperties()
        {
            return properties.ToArray();
        }

        // TODO decide which functions should be private
    }

}
