using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridChunk : MonoBehaviour
    {
        public static int chunkSize = 16;

        public Vector2Int chunkIndex;

        private GridObject[,] objects = new GridObject[chunkSize, chunkSize];

        private GridMap map;

        private void Start()
        {
            map = GetComponentInParent<GridMap>();
        }

        
        public void AddToChunk(GridObject gridObject, Vector2Int gridPos)
        {
            Vector2Int chunkPos = GetChunkPos(gridPos);

            GridObject newGridObject = Instantiate(gridObject, transform);

            newGridObject.gridPos = gridPos;
            objects[chunkPos.x, chunkPos.y] = newGridObject;

            newGridObject.transform.localPosition = new Vector3(chunkPos.x + 0.5f, chunkPos.y + 0.5f, 0);
        }


        public GridObject GetObject(Vector2Int gridPos)
        {
            Vector2Int chunkPos = GetChunkPos(gridPos);
            return objects[chunkPos.x, chunkPos.y];
        }

        // Converts position relative to grid to position relative to chunk
        private Vector2Int GetChunkPos(Vector2Int gridPos)
        {
            return gridPos - chunkIndex * chunkSize;
        }

        // Gets index of chunk that would occupy the grid position
        public static Vector2Int GetChunkIndex(Vector2Int gridPos)
        {
            float x = 1f * gridPos.x / chunkSize;
            float y = 1f * gridPos.y / chunkSize;
            return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        }
    }
}
