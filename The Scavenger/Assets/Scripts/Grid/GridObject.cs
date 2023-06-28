using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridObject : MonoBehaviour
    {
        public Vector2Int gridPos;
        private GridChunk chunk;
        [SerializeField] private GridMap map;

        // TODO move somewhere else? maybe?

        public List<Action> OnPlaced = new();
        public List<Action> OnRemoved = new();
        public List<Func<Vector2Int, bool>> OnNeighborPlaced = new();
        public List<Func<bool>> OnNeighborChanged = new();
        public List<Action> OnTick = new();


        private void Awake()
        {
            chunk = GetComponentInParent<GridChunk>();
            map = chunk.GetComponentInParent<GridMap>();
        }

        public GridObject GetAdjacentObject(Vector2Int direction)   
        {
            return map.GetObjectAtRelativePos(gridPos, direction);
        }


        public virtual bool Interact(Item otherObject)
        {
            return false;   // TODO figure out later
        }

    }
}
