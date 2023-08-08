using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Represents an object that exists on a gridMap.
    /// </summary>
    [RequireComponent(typeof(HP)), DisallowMultipleComponent]
    public class GridObject : MonoBehaviour
    {
        public const int DefaultHP = 100;

        [field: SerializeField] public string DisplayName { get; private set; }
        public Vector2Int GridPos { get; set; }
        private GridMap map;
        private GridChunk chunk;

        public HP HP { get; private set; }

        public event Action SelfChanged;

        private void Awake()
        {
            chunk = GetComponentInParent<GridChunk>();
            map = chunk.GetComponentInParent<GridMap>();
            HP = GetComponent<HP>();

            InitModel();
        }

        // TODO add docs
        private void InitModel()
        {
            if (TryGetComponent(out GridObjectBehavior behavior))
            {
                Model model = GetModel();
                model.Load(behavior);
            }
        }

        // TODO add docs
        public Model GetModel()
        {
            return GetComponentInChildren<Model>();
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

        // TODO add docs
        public List<ItemStack> GetDrops()
        {
            if (TryGetComponent(out LootTable lootTable))
            {
                return lootTable.GetDrops();
            }
            return new();
        }

        /// <summary>
        /// Allows SelfChanged event to be invoked externally
        /// </summary>
        public void OnSelfChanged()
        {
            SelfChanged?.Invoke();
            map.updatePropagation.QueueNeighborUpdates(GridPos);
        }

        // TODO add docs
        public void SubscribeTickUpdate(Action callback)
        {
            map.updateCycle.TickUpdate += callback;
        }

        public void UnsubscribeTickUpdate(Action callback)
        {
            map.updateCycle.TickUpdate -= callback;
        }


        // TODO add docs
        [MenuItem("GameObject/Scavenger/Grid Object")]
        private static void CreateGridObject()
        {
            GameObject newGameObject = new GameObject();
            newGameObject.AddComponent<GridObject>();
            newGameObject.name = "GridObject";
        }
    }
}
