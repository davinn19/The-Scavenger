using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scavenger
{
    public class GridObject : MonoBehaviour
    {
        public Vector2Int gridPos;  // TODO come up with better solution
        private GridMap map;
        private void Awake()
        {
            map = GetComponentInParent<GridMap>();
        }

        public GridObject GetAdjacentObject(Vector2Int direction)   
        {
            return map.GetObjectAtRelativePos(gridPos, direction);
        }

    }
}
