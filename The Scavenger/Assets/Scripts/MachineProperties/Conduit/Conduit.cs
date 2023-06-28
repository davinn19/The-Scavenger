using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class Conduit : MonoBehaviour
    {   
        private HashSet<Vector2Int> disabledSides = new HashSet<Vector2Int>();

        private void Awake()
        {
            GridObject gridObject = GetComponent<GridObject>();
            gridObject.OnNeighborPlaced.Add(OnNeighborPlaced);
            gridObject.OnNeighborChanged.Add(OnNeighborChanged);
        }

        // If the adjacent object is removed or accepts conduits, reset the disabled side
        private bool OnNeighborPlaced(Vector2Int sideUpdated)
        {
            GridObject gridObject = GetComponent<GridObject>();
            GridObject adjObject = gridObject.GetAdjacentObject(sideUpdated);

            bool oldDisabled = IsSideDisabled(sideUpdated);
            if (!adjObject || adjObject.GetComponent<ConduitInterface>() || adjObject.GetComponent<Conduit>())
            {
                SetDisabledSide(sideUpdated, false);
            }

            PrintDisabledSides();
            return oldDisabled != IsSideDisabled(sideUpdated);
        }


        // Makes sure its disabled sides align with adjacent conduits
        private bool OnNeighborChanged()
        {
            bool changed = false;
            GridObject gridObject = GetComponent<GridObject>();

            foreach (Vector2Int side in GridMap.adjacentDirections)
            {
                bool oldDisabled = IsSideDisabled(side);
                bool disabled = oldDisabled;

                GridObject adjObject = gridObject.GetAdjacentObject(side);

                if (!adjObject)
                {
                    disabled = false;
                }
                else
                {
                    Conduit otherConduit = adjObject.GetComponent<Conduit>();
                    ConduitInterface otherInterface = adjObject.GetComponent<ConduitInterface>();

                    if (otherConduit)
                    {
                        Vector2Int oppositeSide = side * -1;
                        disabled = otherConduit.IsSideDisabled(oppositeSide);
                    }
                    else if (otherInterface)
                    {

                    }
                }

                SetDisabledSide(side, disabled);

                if (disabled != oldDisabled)
                {
                    changed = true;
                }
            }

            return changed;
        }

        // Checks if a side is connected to something
        public bool IsSideConnected(Vector2Int side)
        {
            if (IsSideDisabled(side))
            {
                return false;
            }

            GridObject adjObject = GetComponent<GridObject>().GetAdjacentObject(side);

            if (!adjObject)
            {
                return false;
            }

            return adjObject.GetComponent<ConduitInterface>() || adjObject.GetComponent<Conduit>();
        }

        // Checks if a side is disabled from connecting
        public bool IsSideDisabled(Vector2Int side)
        {
            return disabledSides.Contains(side);
        }

        // Sets a side to be disabled/enabled from connecting
        public void SetDisabledSide(Vector2Int side, bool disabled)
        {
            if (disabled)
            {
                disabledSides.Add(side);
            }
            else
            {
                disabledSides.Remove(side);
            }
        }

        // Outputs the disabled sides
        private void PrintDisabledSides()
        {
            string output = name;
            foreach (Vector2Int side in disabledSides)
            {
                output += "\n" + side.ToString();
            }

            Debug.Log(output);
        }
    }
}
