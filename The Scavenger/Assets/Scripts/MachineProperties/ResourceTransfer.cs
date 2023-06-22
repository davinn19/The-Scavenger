using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public abstract class ResourceTransfer : MonoBehaviour
    {
        [SerializeField] protected List<Vector2> connectedSides = new List<Vector2>();

        public abstract void InitConnectedSides();

        public abstract bool CanConnectTo(GridObject gridObject);

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
