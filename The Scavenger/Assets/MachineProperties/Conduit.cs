using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class Conduit : MonoBehaviour
    {
        private HashSet<Vector2Int> connectedSides = new HashSet<Vector2Int>();

        private void Start()
        {
            InitConnectedSides();
        }

        public void InitConnectedSides()
        {
            GridObject gridObject = GetComponent<GridObject>();

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                GridObject adjObject = gridObject.GetAdjacentObject(side);

                Conduit adjConduit = null;

                if (!adjObject || !adjObject.TryGetComponent(out adjConduit))
                {
                    continue;
                }

                SetConnected(side, true);
                adjConduit.SetConnected(side * -1, true);

            }
            
        }

        public bool IsSideConnected(Vector2Int side)
        {
            return connectedSides.Contains(side);
        }

        public void SetConnected(Vector2Int side, bool connected)
        {
            if (connected)
            {
                connectedSides.Add(side);
            }
            else
            {
                connectedSides.Remove(side);
            }

        }

    }
}
