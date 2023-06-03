using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class GridMap : MonoBehaviour
    {
        [SerializeField] private int cellSize;
        private GridObject[,] grid = new GridObject[10,10];


        public void PlaceObject(GridObject gridObject, Vector2 pos)
        {

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
    }
}
