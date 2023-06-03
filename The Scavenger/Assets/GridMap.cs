using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridMap : MonoBehaviour
    {
        private readonly Vector2[] adjacentDirections = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        [SerializeField] private int cellSize;
        [SerializeField] private int mapSize;

        private GridObject[,] grid;


        void Awake()
        {
            grid = new GridObject[mapSize, mapSize];
        }

        public void PlaceObject(GridObject gridObject, Vector2 pos)
        {
            if (!IsInRange(pos))
            {
                Debug.Log("Out of map range.");
                return;
            }

            if (GetObjectAtPos(pos))
            {
                Debug.Log("Position is occupied");
                return;
            }

            grid[(int)pos.x, (int)pos.y] = gridObject;
            gridObject.transform.localPosition = pos;

        }

        public GridObject GetObjectAtPos(Vector2 pos)
        { 
            return grid[(int)pos.x, (int)pos.y];
        }

        public GridObject GetObjectAtRelativePos(Vector2 origin, Vector2 relativePos)
        {
            Vector2 pos = relativePos + origin;
            return grid[(int)pos.x, (int)pos.y];
        }

        public List<GridObject> GetAdjacentObjects(Vector2 pos)
        {
            List<GridObject> adjacents = new List<GridObject>();

            foreach (Vector2 direction in adjacentDirections)
            {
                GridObject adjacent = GetObjectAtRelativePos(pos, direction);
                if (adjacent)
                {
                    adjacents.Add(adjacent);
                }
            }

            return adjacents;
        }

        public bool IsInRange(Vector2 pos)
        {
            int x = (int)pos.x;
            int y = (int)pos.y;
            return x >= 0 && x < mapSize && y >= 0 && y < mapSize;
        }
    }
}
