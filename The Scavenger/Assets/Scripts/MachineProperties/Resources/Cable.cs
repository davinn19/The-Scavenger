using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(Conduit))]
    public abstract class Cable<T> : MonoBehaviour where T : Buffer
    {
        protected Conduit conduit;
        protected GridObject gridObject;
        [SerializeField] protected int transferRate;

        private void Awake()
        {
            conduit = GetComponent<Conduit>();
            gridObject = GetComponent<GridObject>();
        }

        protected List<T> GetConnectedOutputs()
        {
            List<T> outputs = new List<T>();

            Queue<Cable<T>> pendingSearches = new Queue<Cable<T>>();
            HashSet<Cable<T>> visited = new HashSet<Cable<T>>();

            pendingSearches.Enqueue(GetComponent<Cable<T>>());

            while (pendingSearches.Count > 0)
            {
                Cable<T> curCable = pendingSearches.Dequeue();

                if (visited.Contains(curCable))
                {
                    continue;
                }

                visited.Add(curCable);

                foreach (Vector2Int side in GridMap.adjacentDirections)
                {
                    if (!curCable.conduit.IsSideConnected(side) || curCable.conduit.IsSideExtracting(side))
                    {
                        continue;
                    }

                    // TODO get grid objects adjacent to current cable, not the cable calling the function
                    Cable<T> adjCable = gridObject.GetAdjacentObject<Cable<T>>(side);
                    if (adjCable && adjCable.Equals(curCable))
                    {
                        pendingSearches.Enqueue(adjCable);
                        continue;
                    }

                    ConduitInterface<T> adjInterface = gridObject.GetAdjacentObject<ConduitInterface<T>>(side);

                    if (adjInterface)
                    {
                        outputs.AddRange(adjInterface.GetOutputs()); ;
                    }
                }
            }

            return outputs;
        }

        public override bool Equals(object other)
        {
            if (!(other as Cable<T>))
            {
                return false;
            }
            
            return (other as Cable<T>).transferRate == transferRate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(transferRate, typeof(T));
        }
    }
}
