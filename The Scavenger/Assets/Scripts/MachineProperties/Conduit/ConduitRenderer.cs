using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scavenger
{
    // TODO check if this still works properly, add documentation
    public class ConduitRenderer : MonoBehaviour
    {
        [SerializeField] private Conduit conduit;

        [SerializeField]
        [Tooltip("Connections are ordered according to the order of GridMap.adjacentDirections")]
        private SpriteResolver[] connectionSprites;

        private void Awake()
        {
            GridObject gridObject = GetComponent<GridObject>();

            gridObject.SubscribeSelfChanged(UpdateAllSides);

            //gridObject.OnPlaced.Add(UpdateAllSides);
            //gridObject.OnNeighborPlaced.Add((side) => { UpdateSide(side); return false; });
            //gridObject.OnNeighborChanged.Add(() => { UpdateAllSides(); return false;  });
        }

        private void UpdateAllSides()
        {
            for (int sideIndex = 0; sideIndex < 4; sideIndex++)
            {
                SpriteResolver connectionSprite = connectionSprites[sideIndex];
                Vector2Int side = GridMap.adjacentDirections[sideIndex];
                string label = GetLabelForSide(side);

                connectionSprite.SetCategoryAndLabel("Connections", label);
            }
        }

        // Gets the proper sprite resolver label for a specific side
        private string GetLabelForSide(Vector2Int side)
        {
            if (conduit.IsSideExtracting(side))
            {
                return "Extract";
            }

            if (conduit.IsSideConnected(side))
            {
                return "Connected";
            }
            else
            {
                return "Unconnected";
            }
        }
    }
}
