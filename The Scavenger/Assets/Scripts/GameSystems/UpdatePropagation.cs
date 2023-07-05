using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class UpdatePropagation : MonoBehaviour
    {
        public List<Action> OnPlace = new();
        public List<Action> OnRemove = new();
        public List<Func<Vector2Int, bool>> OnNeighborPlaced = new();
        public List<Func<bool>> OnNeighborUpdated = new();
        public List<Action> TickUpdate = new();

        private GridMap map;

        private void Awake()
        {
            map = GetComponent<GridMap>();
        }

        // Handles updates when an object is placed
        public void HandlePlaceUpdate(Vector2Int placedPos)
        {
            foreach (Action action in map.GetObjectAtPos(placedPos).OnPlaced)
            {
                action();
            }

            HandleNeighborPlacedUpdates(placedPos);
        }

        // Handles updates when an object is removed
        public void HandleRemoveUpdate(Vector2Int removedPos)
        {
            foreach (Action action in map.GetObjectAtPos(removedPos).OnRemoved)
            {
                action();
            }

            HandleNeighborPlacedUpdates(removedPos);
        }

        // Calls neighbor placed methods in adjacent objects when an object is placed/removed
        private void HandleNeighborPlacedUpdates(Vector2Int placedPos)
        {
            Queue<Vector2Int> queuedUpdates = new Queue<Vector2Int>();

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                Vector2Int neighborPos = placedPos + side;
                GridObject adjObject = map.GetObjectAtPos(neighborPos);

                if (!adjObject)
                {
                    continue;
                }

                Vector2Int oppositeSide = side * -1;

                bool updated = false;

                foreach (Func<Vector2Int, bool> func in adjObject.OnNeighborPlaced)
                {
                    if (func(oppositeSide))
                    {
                        updated = true;
                    }
                }

                if (updated)
                {
                    foreach (Vector2Int neighborSide in GridMap.adjacentDirections)
                    {
                        queuedUpdates.Enqueue(neighborPos + neighborSide);
                    }
                }
            }

            HandleNeighborChangedUpdates(queuedUpdates);
        }


        public void HandleNeighborChangedUpdates(Vector2Int posChanged)
        {
            Queue<Vector2Int> neighborPos = new Queue<Vector2Int>();

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                neighborPos.Enqueue(posChanged + side);
            }

            HandleNeighborChangedUpdates(neighborPos);
        }

        // Updates neighbors when an object changes state
        private void HandleNeighborChangedUpdates(Queue<Vector2Int> queuedUpdates)
        {
            while (queuedUpdates.Count > 0)
            {
                Vector2Int pos = queuedUpdates.Dequeue();
                GridObject gridObject = map.GetObjectAtPos(pos);

                if (!gridObject)
                {
                    continue;
                }

                bool updated = false;
                foreach (Func<bool> func in gridObject.OnNeighborChanged)
                {
                    if (func())
                    {
                        updated = true;
                    }
                }

                if (updated)
                {
                    foreach (Vector2Int side in GridMap.adjacentDirections)
                    {
                        queuedUpdates.Enqueue(pos + side);
                    }
                }
            }
        }
    }
}
