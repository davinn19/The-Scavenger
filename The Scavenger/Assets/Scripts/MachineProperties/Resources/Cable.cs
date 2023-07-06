using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Base class for all resource transport, attach to a Conduit.
    /// </summary>
    /// <typeparam name="T">The Buffer type (energy, items) that Cable interacts with.</typeparam>

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

        
        /// <summary>
        /// Searches through connected cables for all connected output buffers.
        /// </summary>
        /// <returns>A list of connected output buffers, ordered by proximity to the cable.</returns>
        protected List<T> GetConnectedOutputs()
        {
            List<T> outputs = new List<T>();

            // Performs BFS so results are ordered by how close they are to the cable
            Queue<Cable<T>> pendingSearches = new Queue<Cable<T>>();
            HashSet<Cable<T>> visited = new HashSet<Cable<T>>();

            pendingSearches.Enqueue(GetComponent<Cable<T>>());

            while (pendingSearches.Count > 0)
            {
                Cable<T> curCable = pendingSearches.Dequeue();

                // Skips if curCable has already been visited
                if (visited.Contains(curCable))
                {
                    continue;
                }

                visited.Add(curCable);

                foreach (Vector2Int side in GridMap.adjacentDirections)
                {
                    // Checks for a side's connectivity and if it is in extract mode
                    if (!curCable.conduit.IsSideConnected(side) || curCable.conduit.IsSideExtracting(side))
                    {
                        continue;
                    }

                    // If side is connected to a compatible Cable, add to search
                    Cable<T> adjCable = curCable.gridObject.GetAdjacentObject<Cable<T>>(side);
                    if (adjCable && adjCable.Equals(curCable))
                    {
                        pendingSearches.Enqueue(adjCable);
                        continue;
                    }

                    // If side is connected to a compatible ConduitInterface, add to results
                    ConduitInterface<T> adjInterface = curCable.gridObject.GetAdjacentObject<ConduitInterface<T>>(side);
                    if (adjInterface)
                    {
                        outputs.AddRange(adjInterface.GetOutputs());
                    }
                }
            }

            return outputs;
        }


        /// <summary>
        /// Checks if two cables can connect.
        /// </summary>
        /// <param name="other">The object being compared to.</param>
        /// <returns>True if the cables can connect.</returns>
        public override bool Equals(object other)
        {
            // Cables must have equal transferRate and transfer the same resource type
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
