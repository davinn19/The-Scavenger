using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scavenger
{
    [RequireComponent(typeof(SpriteResolver))]
    [RequireComponent(typeof(Conduit))]
    public class ConduitRenderer : MonoBehaviour
    {
        [SerializeField] private Conduit conduit;

        [SerializeField]
        [Tooltip("Connections are ordered according to the order of GridMap.adjacentDirections")]
        private SpriteResolver[] connectionSprites;

        private void Awake()
        {
            GridObject gridObject = GetComponent<GridObject>();

            gridObject.OnPlaced.Add(OnPlaced);
            gridObject.OnNeighborPlaced.Add(UpdateSide);
            gridObject.OnNeighborChanged.Add(UpdateAllSides);

            

        }

        private void OnPlaced()
        {
            UpdateAllSides();
        }

        private bool UpdateSide(Vector2Int side)
        {
            SpriteResolver connectionSprite = GetConnectionSprite(side);
            string label = GetLabelForSide(side);
            string oldLabel = connectionSprite.GetLabel();

            connectionSprite.SetCategoryAndLabel("Connections", label);

            return label != oldLabel;
        }

        private bool UpdateAllSides()
        {
            bool changed = false;

            for (int sideIndex = 0; sideIndex < 4; sideIndex++)
            {
                SpriteResolver connectionSprite = connectionSprites[sideIndex];
                Vector2Int side = GridMap.adjacentDirections[sideIndex];
                string label = GetLabelForSide(side);
                string oldLabel = connectionSprite.GetLabel();

                connectionSprite.SetCategoryAndLabel("Connections", label);
            }

            return changed;
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
            if (!conduit.IsSideConnected(side))
            {
                return "None";
            }
            ConduitInterface conduitInterface = GetComponent<GridObject>().GetAdjacentObject(side).GetComponent<ConduitInterface>();

            if (conduitInterface)
            {
                Vector2Int opppositeSide = side * -1;
                SideConfig sideConfig = conduitInterface.GetSideConfig(opppositeSide);

                if (sideConfig.transportMode == TransportMode.INSERT)
                {
                    return "Insert";
                }
                else if (sideConfig.transportMode == TransportMode.EXTRACT)
                {
                    return "Extract";
                }
            }

            return "Normal";
        }
    }
}
