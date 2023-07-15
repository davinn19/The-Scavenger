using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridMap : MonoBehaviour
    {
        public static Vector2Int[] adjacentDirections = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        public UpdatePropagation updatePropagation;
        public UpdateCycle updateCycle;

        private Dictionary<Vector2Int, GridChunk> gridChunks = new Dictionary<Vector2Int, GridChunk>();

        // Gets the chunk at the chunk index, creates it if nonexistent
        public GridChunk GetChunk(Vector2Int chunkIndex)
        {
            if (!gridChunks.ContainsKey(chunkIndex))
            {
                return CreateChunk(chunkIndex);
            }

            return gridChunks[chunkIndex];
        }

        // Creates a chunk at the chunk index
        private GridChunk CreateChunk(Vector2Int chunkIndex)
        {
            GameObject chunkObject = new GameObject(chunkIndex.ToString());
            chunkObject.transform.parent = transform;
            chunkObject.transform.localPosition = new Vector3(chunkIndex.x, chunkIndex.y, 0) * GridChunk.chunkSize;

            GridChunk gridChunk = chunkObject.AddComponent<GridChunk>();
            gridChunk.chunkIndex = chunkIndex;
            

            gridChunks.Add(chunkIndex, gridChunk);

            return gridChunk;
        }

        // Gets the chunk that occupies the grid position
        public GridChunk GetChunkAtPos(Vector2Int gridPos)
        {
            Vector2Int chunkIndex = GridChunk.GetChunkIndex(gridPos);
            return GetChunk(chunkIndex);
        }

        // Sets a grid object at a position
        private void SetObjectAtPos(GridObject gridObject, Vector2Int gridPos)
        {
            GridChunk chunk = GetChunkAtPos(gridPos);
            chunk.SetObjectAtPos(gridObject, gridPos);
        }

        // Gets the object at the grid position
        public GridObject GetObjectAtPos(Vector2Int gridPos)
        {
            GridChunk chunk = GetChunkAtPos(gridPos);
            return chunk.GetObject(gridPos);
        }

        // Gets the object at the relative grid position
        public GridObject GetObjectAtRelativePos(Vector2Int origin, Vector2Int relativePos)
        {
            Vector2Int gridPos = relativePos + origin;
            return GetObjectAtPos(gridPos);
        }

        public void TryInteract(ItemStack itemStack, Vector2Int gridPos, Vector2Int sidePressed)
        {
            GridObject existingObject = GetObjectAtPos(gridPos);
            if (existingObject)
            {
                existingObject.Interact(itemStack, sidePressed);
            }
        }

        public bool TryPlaceItem(Item item, Vector2Int gridPos)
        {
            GridObject existingObject = GetObjectAtPos(gridPos);
            if (existingObject)
            {
                return false;
            }

            SetObjectAtPos(item.GetProperty<PlacedObject>().Object, gridPos);

            updatePropagation.HandlePlaceUpdate(gridPos);
            
            return true;
        }
    }
}
