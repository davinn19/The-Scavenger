using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(HP)), DisallowMultipleComponent]
    public class GridObject : MonoBehaviour
    {
        public Vector2Int gridPos;
        private GridMap map;
        private GridChunk chunk;

        public event Action Placed;
        public event Action Removed;
        public event Action<Vector2Int> NeighborPlaced;
        public event Action NeighborChanged;
        public event Action SelfChanged;

        public Action<ItemStack, Vector2Int> Interact = (_, _) => { };

        public void QueueTickUpdate(Action callback)
        {
            map.updateCycle.QueueUpdate(callback);
        }

        private void Awake()
        {
            chunk = GetComponentInParent<GridChunk>();
            map = chunk.GetComponentInParent<GridMap>();
        }


        public GridObject GetAdjacentObject(Vector2Int direction)   
        {
            return map.GetObjectAtRelativePos(gridPos, direction);
        }


        public T GetAdjacentObject<T>(Vector2Int direction) where T : Component
        {
            GridObject adjObject = GetAdjacentObject(direction);
            if (!adjObject)
            {
                return null;
            }

            return adjObject.GetComponent<T>();
        }


        public T GetAdjacentObject<T>(Vector2Int direction, Predicate<T> condition) where T : Component
        {
            T adjObject = GetAdjacentObject<T>(direction);

            if (adjObject && condition(adjObject))
            {
                return adjObject;
            }

            return null;
        }

        public void OnPlace()
        {
            Placed?.Invoke();
        }

        public void OnRemove()
        {
            Removed?.Invoke();
        }

        public void OnNeighborPlaced(Vector2Int side)
        {
            NeighborPlaced?.Invoke(side);
        }

        public void OnNeighborChanged()
        {
            NeighborChanged?.Invoke();
        }

        public void OnSelfChanged()
        {
            SelfChanged?.Invoke();
            map.updatePropagation.QueueNeighborUpdates(gridPos);
        }
    }
}
