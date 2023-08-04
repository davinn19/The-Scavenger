using System;
using UnityEngine;
using Scavenger.UI.UIContent;
using System.Collections.Generic;

namespace Scavenger
{
    /// <summary>
    /// Represents an object that exists on a gridMap.
    /// </summary>
    [DisallowMultipleComponent]
    public class GridObject : MonoBehaviour
    {
        public const int DefaultHP = 100;
        public Vector2Int GridPos { get; set; }
        private GridMap map;
        private GridChunk chunk;

        public HP HP { get; private set; }
        
        public Func<ItemStack> GetRemoveDrops;  // TODO implement

        public event Action SelfChanged;

        private void Awake()
        {
            chunk = GetComponentInParent<GridChunk>();
            map = chunk.GetComponentInParent<GridMap>();
            HP = GetComponent<HP>();
        }

        /// <summary>
        /// Gets the object adjacent in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to check for adjacency.</param>
        /// <returns>The adjacent object.</returns>
        public GridObject GetAdjacentObject(Vector2Int direction)
        {
            return map.GetObjectAtPos(GridPos + direction);
        }

        /// <summary>
        /// Gets the object containing a specific component adjacent in the specified direction.
        /// </summary>
        /// <typeparam name="T">The component to check for.</typeparam>
        /// <param name="direction">The direction to check for adjacency.</param>
        /// <returns>The adjacent object containing the component.</returns>
        public T GetAdjacentObject<T>(Vector2Int direction) where T : Component
        {
            return map.GetObjectAtPos<T>(GridPos + direction);
        }

        /// <summary>
        /// Gets the object containing a specific component and satisfying the condition adjacent in the specified direction.
        /// </summary>
        /// <typeparam name="T">The component to check for.</typeparam>
        /// <param name="direction">The direction to check for adjacency.</param>
        /// <param name="condition">The condition the grid object must pass</param>
        /// <returns>The adjacent object containing the component and satisfying the condition.</returns>
        public T GetAdjacentObject<T>(Vector2Int direction, Predicate<T> condition) where T : Component
        {
            T adjObject = GetAdjacentObject<T>(direction);

            if (adjObject && condition(adjObject))
            {
                return adjObject;
            }

            return null;
        }

        /// <summary>
        /// Allows SelfChanged event to be invoked externally
        /// </summary>
        public void OnSelfChanged()
        {
            SelfChanged?.Invoke();
            map.updatePropagation.QueueNeighborUpdates(GridPos);
        }

    }
}
