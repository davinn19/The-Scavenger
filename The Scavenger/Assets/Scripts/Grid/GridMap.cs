using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridMap : MonoBehaviour
    {
        public static Vector2Int[] adjacentDirections = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

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

        public bool TryInteract(Item item, Vector2Int gridPos)
        {
            GridObject existingObject = GetObjectAtPos(gridPos);
            if (existingObject)
            {
                bool interactSuccessful = existingObject.Interact(item);
                return interactSuccessful;
            }

            return false;
        }

        public bool TryPlaceItem(PlaceableItem item, Vector2Int gridPos)
        {
            GridObject existingObject = GetObjectAtPos(gridPos);
            if (existingObject)
            {
                return false;
            }

            SetObjectAtPos(item.PlacedObject, gridPos);

            PropagatePlacementUpdates(gridPos);

            return true;
        }

        private void PropagatePlacementUpdates(Vector2Int startPos)
        {
            Queue<(Vector2Int, Vector2Int)> queuedUpdates = new Queue<(Vector2Int, Vector2Int)>();
            HashSet<Vector2Int> finishedUpdates = new HashSet<Vector2Int>();

            // Do placement update
            GridObject startObject = GetObjectAtPos(startPos);

            startObject.OnPlace();
            finishedUpdates.Add(startPos);

            // Do neighbor placement update
            foreach (Vector2Int neighborSide in adjacentDirections)
            {
                Vector2Int neighborPos = startPos + neighborSide;
                GridObject adjObject = startObject.GetAdjacentObject(neighborSide);

                if (!adjObject)
                {
                    continue;
                }

                bool neighborUpdated = adjObject.OnNeighborPlaced(neighborSide * -1);
                finishedUpdates.Add(neighborPos);

                if (neighborUpdated)
                {
                    foreach (Vector2Int side in adjacentDirections)
                    {
                        queuedUpdates.Enqueue((neighborPos + side, side * -1));
                    }
                }
            }

            PropagateNeighborUpdates(queuedUpdates, finishedUpdates);
        }

        // queuedUpdates: (position to be updated, side invoking the update)
        private void PropagateNeighborUpdates(Queue<(Vector2Int, Vector2Int)> queuedUpdates, HashSet<Vector2Int> finishedUpdates)
        {
            while (queuedUpdates.Count > 0)
            {
                (Vector2Int, Vector2Int) nextUpdate = queuedUpdates.Dequeue();
                Vector2Int curPos = nextUpdate.Item1;
                Vector2Int updatedSide = nextUpdate.Item2;

                if (finishedUpdates.Contains(curPos))
                {
                    continue;
                }

                finishedUpdates.Add(curPos);
                GridObject gridObject = GetObjectAtPos(curPos);

                if (!gridObject)
                {
                    continue;
                }

                if (gridObject.OnNeighborUpdated(updatedSide))
                {
                    foreach (Vector2Int side in adjacentDirections)
                    {
                        queuedUpdates.Enqueue((curPos, side));
                    }
                }
            }
            

        }

    }
}
