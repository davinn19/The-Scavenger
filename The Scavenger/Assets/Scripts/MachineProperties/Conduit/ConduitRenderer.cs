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

        [SerializeField]
        private GameObject[] cableTypeSprites;

        private void Awake()
        {
            GridObject gridObject = GetComponent<GridObject>();
            gridObject.SubscribeSelfChanged(UpdateAppearance);
        }


        private void UpdateAppearance()
        {
            UpdateAllSides();
            UpdateCenter();
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

        private void UpdateCenter()
        {
            // TODO implement
            List<string> cables = new();
            if (GetComponent<EnergyCable>())
            {
                cables.Add("Energy");
            }

            if (GetComponent<ItemCable>())
            {
                cables.Add("Item");
            }

            int numCables = cables.Count;
            for (int i = 0; i < 4; i++)
            {
                GameObject sprites = cableTypeSprites[i];
                if (i != numCables - 1)
                {
                    sprites.SetActive(false);
                }
                else
                {
                    sprites.SetActive(true);
                    SpriteResolver[] resolvers = sprites.GetComponentsInChildren<SpriteResolver>();

                    for (int j = 0; j < resolvers.Length; j++)
                    {
                        resolvers[j].SetCategoryAndLabel("Cable Symbols", cables[i]);
                    }
                }
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
