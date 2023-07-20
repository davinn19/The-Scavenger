using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Stores information about a specific chunk in the world.
    /// </summary>
    public class GridChunk : MonoBehaviour
    {
        public const int ChunkSize = 9;
        public Vector2Int ChunkIndex { get; set; }
        private readonly GridObject[,] objects = new GridObject[ChunkSize, ChunkSize];
        
        /// <summary>
        /// Sets a new gridObject at the specified position.
        /// </summary>
        /// <param name="gridObject">Prefab of new gridObject.</param>
        /// <param name="gridPos">Absolute position to set the new gridObject.</param>
        public void SetObjectAtPos(GridObject gridObject, Vector2Int gridPos)
        {
            ClearPos(gridPos);

            Vector2Int chunkPos = GetChunkPos(gridPos);
            GridObject newGridObject = Instantiate(gridObject, transform);
            newGridObject.name = gridObject.name;

            newGridObject.gridPos = gridPos;
            objects[chunkPos.x, chunkPos.y] = newGridObject;

            newGridObject.transform.position = GridMap.GetCenterOfTile(gridPos);
        }

        /// <summary>
        /// Removes the grid object at a position, if it exists.
        /// </summary>
        /// <param name="gridPos">Absolute position to clear.</param>
        private void ClearPos(Vector2Int gridPos)
        {
            GridObject existingObject = GetObject(gridPos);
            if (existingObject)
            {
                Destroy(existingObject.gameObject);
            }
            Vector2Int chunkPos = GetChunkPos(gridPos);
            objects[chunkPos.x, chunkPos.y] = null;
        }

        /// <summary>
        /// Gets the gridObject at the specified position.
        /// </summary>
        /// <param name="gridPos">Absolute position to get the gridObject.</param>
        /// <returns>GridObject at the position.</returns>
        public GridObject GetObject(Vector2Int gridPos)
        {
            Vector2Int chunkPos = GetChunkPos(gridPos);
            return objects[chunkPos.x, chunkPos.y];
        }

        /// <summary>
        /// Converts absolute position to chunk relative position.
        /// </summary>
        /// <param name="gridPos">Absolute position.</param>
        /// <returns>Equivalent chunk relative position.</returns>
        private Vector2Int GetChunkPos(Vector2Int gridPos)
        {
            return gridPos - ChunkIndex * ChunkSize;
        }

        /// <summary>
        /// Gets index of chunk that would occupy the grid position.
        /// </summary>
        /// <param name="gridPos">Absolute position.</param>
        /// <returns>Index of overlapping chunk.</returns>
        public static Vector2Int GetChunkIndex(Vector2Int gridPos)
        {
            float x = 1f * gridPos.x / ChunkSize;
            float y = 1f * gridPos.y / ChunkSize;
            return new Vector2Int(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
        }
    }
}
