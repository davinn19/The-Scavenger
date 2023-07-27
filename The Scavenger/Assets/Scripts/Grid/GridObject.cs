using System;
using UnityEngine;
using Scavenger.UI.UIContent;

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

        public event Action Placed;
        public event Action Removed;
        public event Action<Vector2Int> NeighborPlaced;
        public event Action NeighborChanged;
        public event Action SelfChanged;

        public Func<ItemBuffer, HeldItemHandler, Vector2Int, bool> TryInteract;
        public Func<Vector2Int, bool> TryEdit;
        public Func<ItemStack> GetRemoveDrops;

        [field: SerializeField] public GridObjectUIContent UIContent { get; private set; }


        private void Awake()
        {
            chunk = GetComponentInParent<GridChunk>();
            map = chunk.GetComponentInParent<GridMap>();

            HP = GetComponent<HP>();
            if (!HP)
            {
                HP = gameObject.AddComponent<HP>();
                HP.SetMaxHealth(DefaultHP);
            }
        }

        /// <summary>
        /// Gets the object adjacent in the specified direction.
        /// </summary>
        /// <param name="direction">The direction to check for adjacency.</param>
        /// <returns>The adjacent object.</returns>
        public GridObject GetAdjacentObject(Vector2Int direction)
        {
            return map.GetObjectAtPos(GridPos, direction);
        }

        /// <summary>
        /// Gets the object containing a specific component adjacent in the specified direction.
        /// </summary>
        /// <typeparam name="T">The component to check for.</typeparam>
        /// <param name="direction">The direction to check for adjacency.</param>
        /// <returns>The adjacent object containing the component.</returns>
        public T GetAdjacentObject<T>(Vector2Int direction) where T : Component
        {
            GridObject adjObject = GetAdjacentObject(direction);
            if (!adjObject)
            {
                return null;
            }

            return adjObject.GetComponent<T>();
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
        /// Schedules a tick update.
        /// </summary>
        /// <param name="callback">The function to run when updating this object.</param>
        public void QueueTickUpdate(Action callback)
        {
            map.updateCycle.QueueUpdate(callback);
        }

        /// <summary>
        /// Allows Placed event to be invoked externally
        /// </summary>
        public void OnPlace()
        {
            Placed?.Invoke();
        }

        /// <summary>
        /// Allows Removed event to be invoked externally
        /// </summary>
        public void OnRemove()
        {
            Removed?.Invoke();
        }

        /// <summary>
        /// Allows NeighborPlaced event to be invoked externally
        /// </summary>
        public void OnNeighborPlaced(Vector2Int side)
        {
            NeighborPlaced?.Invoke(side);
        }

        /// <summary>
        /// Allows NeighborChanged event to be invoked externally
        /// </summary>
        public void OnNeighborChanged()
        {
            NeighborChanged?.Invoke();
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
