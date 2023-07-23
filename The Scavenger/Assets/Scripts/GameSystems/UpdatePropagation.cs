using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class UpdatePropagation : MonoBehaviour
    {
        public List<Action> TickUpdate = new();

        private Queue<Vector2Int> queuedUpdates = new();


        private GridMap map;

        private void Awake()
        {
            map = GetComponent<GridMap>();
        }

        private void Update()
        {
            HandleNeighborChangedUpdates();
        }

        public void QueueNeighborUpdates(Vector2Int startPos)
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                queuedUpdates.Enqueue(side + startPos);
            }
        }

        // Handles updates when an object is placed
        public void HandlePlaceUpdate(Vector2Int placedPos)
        {
            map.GetObjectAtPos(placedPos).OnPlace();
            HandleNeighborPlacedUpdates(placedPos);
        }

        // Handles updates when an object is removed
        public void HandleRemoveUpdate(Vector2Int removedPos)
        {
            map.GetObjectAtPos(removedPos).OnRemove();
            HandleNeighborPlacedUpdates(removedPos);
        }

        // Calls neighbor placed methods in adjacent objects when an object is placed/removed
        private void HandleNeighborPlacedUpdates(Vector2Int placedPos)
        {
            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                Vector2Int neighborPos = placedPos + side;
                GridObject adjObject = map.GetObjectAtPos(neighborPos);

                if (adjObject)
                {
                    Vector2Int oppositeSide = side * -1;
                    adjObject.OnNeighborPlaced(oppositeSide);
                }
            }
        }

        // Updates neighbors when an object changes state
        private void HandleNeighborChangedUpdates()
        {
            while (queuedUpdates.Count > 0)
            {
                Vector2Int pos = queuedUpdates.Dequeue();
                GridObject gridObject = map.GetObjectAtPos(pos);

                if (gridObject)
                {
                    gridObject.OnNeighborChanged();
                }
            }
        }
    }
}
