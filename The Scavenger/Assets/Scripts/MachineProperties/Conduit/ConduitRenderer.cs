using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scavenger
{
    // TODO check if this still works properly, add documentation
    [RequireComponent(typeof(SpriteResolver))]
    public class ConduitRenderer : MonoBehaviour
    {
        [SerializeField] private Conduit conduit;

        [SerializeField]
        [Tooltip("Connections are ordered according to the order of GridMap.adjacentDirections")]
        private SpriteResolver[] connectionSprites;

        private void Awake()
        {
            GridObject gridObject = GetComponent<GridObject>();

            gridObject.OnPlaced.Add(UpdateAllSides);
            gridObject.OnNeighborPlaced.Add((side) => { UpdateSide(side); return false; });
            gridObject.OnNeighborChanged.Add(() => { UpdateAllSides(); return false;  });
        }

        private void UpdateSide(Vector2Int side)
        {
            SpriteResolver connectionSprite = GetConnectionSprite(side);
            string label = GetLabelForSide(side);

            connectionSprite.SetCategoryAndLabel("Connections", label);
        }

        private void UpdateAllSides()
        {
            for (int sideIndex = 0; sideIndex < 4; sideIndex++)
            {
                SpriteResolver connectionSprite = connectionSprites[sideIndex];
                Vector2Int side = GridMap.adjacentDirections[sideIndex];
                string label = GetLabelForSide(side);
                string oldLabel = connectionSprite.GetLabel();

                connectionSprite.SetCategoryAndLabel("Connections", label);
            }
        }

        // Gets sprite resolver for specific side
        private SpriteResolver GetConnectionSprite(Vector2Int side)
        {
            for (int i = 0; i < GridMap.adjacentDirections.Length; i++)
            {
                if (GridMap.adjacentDirections[i] == side)
                {
                    return connectionSprites[i];
                }
            }
            return null;
        }

        // Gets the proper sprite resolver label for a specific side
        private string GetLabelForSide(Vector2Int side)
        {
            if (conduit.IsSideDisabled(side))
            {
                return "Disabled";
            }

            if (conduit.IsSideExtracting(side))
            {
                return "Extract";
            }

            if (!conduit.IsSideConnected(side))
            {
                return "None";
            }

            return "Normal";
        }
    }
}
