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
        private GridMap map;

        private void Awake()
        {
            chunk = GetComponentInParent<GridChunk>();
            map = chunk.GetComponentInParent<GridMap>();
        }

        public GridObject GetAdjacentObject(Vector2Int direction)   
        {
            return map.GetObjectAtRelativePos(gridPos, direction);
        }
    }
}
