using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scavenger
{
    public class GridObject : MonoBehaviour
    {
        public Vector2Int gridPos;
        private GridChunk chunk;
        [SerializeField] private GridMap map;

        // TODO move somewhere else? maybe?
        public Action OnPlace = () => { };
        public Action OnRemove = () => { };
        public Func <Vector2Int, bool> OnNeighborPlaced = (_) => { return false; };
        public Func<Vector2Int, bool> OnNeighborUpdated = (_) => { return false; };
        public Action TickUpdate = () => { };


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
