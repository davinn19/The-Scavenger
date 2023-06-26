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
            gridObject.OnNeighborUpdated = OnNeighborUpdated;
            gridObject.OnNeighborPlaced = OnNeighborPlaced;
        }

        // If the adjacent object is removed or accepts conduits, reset the disabled side
        private bool OnNeighborPlaced(Vector2Int sideUpdated)
        {
            Debug.Log(sideUpdated);
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


        private bool OnNeighborUpdated(Vector2Int sideUpdated)
        {
            GridObject gridObject = GetComponent<GridObject>();
            GridObject adjObject = gridObject.GetAdjacentObject(sideUpdated);

            bool oldDisabled = IsSideDisabled(sideUpdated);
            bool disabled = oldDisabled;

            Conduit otherConduit;
            if (adjObject.TryGetComponent(out otherConduit))
            {
                Vector2Int oppositeSide = sideUpdated * -1;
                disabled = otherConduit.IsSideDisabled(oppositeSide);
            }

            SetDisabledSide(sideUpdated, disabled);


            PrintDisabledSides();
            return disabled != oldDisabled;
        }

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

        public bool IsSideDisabled(Vector2Int side)
        {
            return disabledSides.Contains(side);
        }

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
