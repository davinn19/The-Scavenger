using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Defines an itemStack's behavior with different properties.
    /// </summary>
    [CreateAssetMenu(fileName = "Item", menuName = "Scavenger/Item")]
    public class Item : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField, Multiline] public string Description { get; private set; }

        [SerializeField] private List<ItemProperty> properties = new() { };

        /// <summary>
        /// Attempts to lead an interation with the gridMap. Will fail if the item does not contain the Interactible property.
        /// </summary>
        /// <param name="gameManager">The current game manager.</param>
        /// <param name="heldItem">the current held item handler.</param>
        /// <param name="pressedPos">The grid position pressed.</param>
        /// <returns></returns>
        public bool Interact(GameManager gameManager, HeldItemHandler heldItem, Vector2Int pressedPos)
        {
            if (TryGetProperty(out Interactibe interactProperty))
            {
                interactProperty.Interact(gameManager, heldItem, pressedPos);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the item's property.
        /// </summary>
        /// <param name="propertyType">The property to get.</param>
        /// <returns>The property with the specified type.</returns>
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

        /// <summary>
        /// Gets the item's property.
        /// </summary>
        /// <typeparam name="T">The property to get.</typeparam>
        /// <returns>The property with the specified type.</returns>
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

        /// <summary>
        /// Attempts to get the item's property.
        /// </summary>
        /// <typeparam name="T">The property to get.</typeparam>
        /// <param name="property">Where to store the retrieved property.</param>
        /// <returns>True if the item contains the property.</returns>
        public bool TryGetProperty<T>(out T property) where T : ItemProperty
        {
            property = GetProperty<T>();
            return property != null;
        }

        /// <summary>
        /// Checks if the item has a property.
        /// </summary>
        /// <param name="propertyType">The property to check for.</param>
        /// <returns>True if the item has the property.</returns>
        public bool HasProperty(Type propertyType) => GetProperty(propertyType) != null;

        /// <summary>
        /// Checks if the item has a property.
        /// </summary>
        /// <typeparam name="T">The property to check for.</typeparam>
        /// <returns>True if the item has the property.</returns>
        public bool HasProperty<T>() where T : ItemProperty
        {
            T hey = GetProperty<T>();
            return hey != null;
        }
    }

}
