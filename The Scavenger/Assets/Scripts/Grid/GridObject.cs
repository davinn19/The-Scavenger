using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridObject : MonoBehaviour
    {
        public Vector2Int gridPos;
        private GridMap map;
        private GridChunk chunk;

        // TODO move somewhere else? maybe?

        public List<Action> OnPlaced = new();
        public List<Action> OnRemoved = new();
        public List<Action<Vector2Int>> OnNeighborPlaced = new();
        public List<Action> OnNeighborChanged = new();

        public List<Action> OnTick = new();

        private List<Action> selfChangedCallbacks = new();


        public Action<ItemStack, Vector2Int> Interact;


        public void SubscribeSelfChanged(Action callback)
        {
            selfChangedCallbacks.Add(callback);
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

        public void OnSelfChanged()
        {
            foreach (Action action in selfChangedCallbacks)
            {
                action();
            }

            map.updatePropagation.QueueNeighborUpdates(gridPos);
        }
    }
}
