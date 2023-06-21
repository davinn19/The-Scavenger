using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridMap : MonoBehaviour
    {
        public static Vector2Int[] adjacentDirections = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        [SerializeField] private int cellSize;
        [SerializeField] private int mapSize;

        private GridObject[,] grid;


        void Awake()
        {
            grid = new GridObject[mapSize, mapSize];
        }

        public void PlaceObject(GridObject gridObject, Vector2 pos)
        {
            Vector2 worldPos = new Vector2(Mathf.Floor(pos.x) + cellSize / 2f, Mathf.Floor(pos.y) + cellSize / 2f);
            Vector2Int gridPos = new Vector2Int(Mathf.FloorToInt(pos.x / cellSize), Mathf.FloorToInt(pos.y / cellSize));

            if (!IsInRange(gridPos))
            {
                Debug.Log("Out of map range.");
                return;
            }

            if (GetObjectAtPos(gridPos))
            {
                Debug.Log("Position is occupied");
                return;
            }

            GridObject newGridObj = Instantiate(gridObject, transform);
            newGridObj.gridPos = gridPos;
            grid[gridPos.x, gridPos.y] = newGridObj;

            newGridObj.transform.localPosition = worldPos;

        }

        public GridObject GetObjectAtPos(Vector2Int gridPos)
        { 
            if (IsInRange(gridPos))
            {
                return grid[gridPos.x, gridPos.y];
            }
            return null;
        }

        public GridObject GetObjectAtRelativePos(Vector2Int origin, Vector2Int relativePos)
        {
            Vector2Int pos = relativePos + origin;
            return GetObjectAtPos(pos);
        }

        public List<GridObject> GetAdjacentObjects(Vector2Int pos)
        {
            List<GridObject> adjacents = new List<GridObject>();

            foreach (Vector2Int direction in adjacentDirections)
            {
                GridObject adjacent = GetObjectAtRelativePos(pos, direction);
                if (adjacent)
                {
                    adjacents.Add(adjacent);
                }
            }

            return adjacents;
        }

        public bool IsInRange(Vector2Int pos)
        {
            int x = pos.x;
            int y = pos.y;
            return x >= 0 && x < mapSize && y >= 0 && y < mapSize;
        }

    }
}
