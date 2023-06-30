using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(Conduit))]
    public abstract class Cable : MonoBehaviour
    {
        protected Conduit conduit;
        protected GridObject gridObject;

        private void Awake()
        {
            conduit = GetComponent<Conduit>();
            gridObject = GetComponent<GridObject>();
        }

        protected List<BufferType> GetConnectedOutputs<CableType, BufferType>() where CableType : Cable where BufferType : Buffer
        {
            List<BufferType> outputs = new List<BufferType>();

            Queue<CableType> pendingSearches = new Queue<CableType>();
            HashSet<CableType> visited = new HashSet<CableType>();

            pendingSearches.Enqueue(GetComponent<CableType>());

            while (pendingSearches.Count > 0)
            {
                CableType curCable = pendingSearches.Dequeue();

                if (visited.Contains(curCable))
                {
                    continue;
                }

                visited.Add(curCable);

                foreach (Vector2Int side in GridMap.adjacentDirections)
                {
                    if (!curCable.conduit.IsSideConnected(side))
                    {
                        continue;
                    }

                    CableType adjCable = gridObject.GetAdjacentObject<CableType>(side);
                    if (adjCable && adjCable.Equals(curCable))
                    {
                        pendingSearches.Enqueue(adjCable);
                        continue;
                    }

                    BufferType adjBuffer = gridObject.GetAdjacentObject<BufferType>(side); 
                    if (adjBuffer && !conduit.IsSideExtracting(side))
                    {
                        outputs.Add(adjBuffer);
                    }
                }
            }

            return outputs;
        }
    }
}
