using Scavenger.GridObjectBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Scavenger.Models
{
    // TODO add docs
    public class ConduitModel : Model
    {
        [SerializeField] private Sprite connectedSprite;
        [SerializeField] private Sprite extractingSprite;

        [SerializeField]
        [Tooltip("Connections are ordered according to the order of GridMap.adjacentDirections")]
        private SpriteRenderer[] connectionSprites;

        [SerializeField]
        private GameObject[] cableTypeSprites;

        private Conduit conduit;

        public override void Load(GridObjectBehavior behavior)
        {
            conduit = behavior.GetComponent<Conduit>();

            GridObject gridObject = behavior.GetComponent<GridObject>();
            gridObject.SelfChanged += UpdateAppearance;
        }

        public override void UpdateAppearance()
        {
            UpdateAllSides();
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates the connection sprites based on adjacent gridObjects and each side's transport mode.
        /// </summary>
        private void UpdateAllSides()
        {
            for (int sideIndex = 0; sideIndex < 4; sideIndex++)
            {
                SpriteRenderer connectionSprite = connectionSprites[sideIndex];
                Vector2Int side = GridMap.adjacentDirections[sideIndex];
                Sprite sprite = GetSpriteForSide(side);

                connectionSprite.sprite = sprite;
            }
        }

        /// <summary>
        /// Updates the symbol sprites based on which cables are in the conduit.
        /// </summary>
        private void UpdateCenter()
        {
            // TODO add fluid cable
            List<string> cables = new();
            if (conduit.GetComponent<EnergyCable>())
            {
                cables.Add("Energy");
            }

            if (conduit.GetComponent<ItemCable>())
            {
                cables.Add("Item");
            }

            if (conduit.GetComponent<DataCable>())
            {
                cables.Add("Data");
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
                        resolvers[j].SetCategoryAndLabel("Cable Symbols", cables[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the proper sprite for a specific side based on its connected status.
        /// </summary>
        /// <param name="side">The side to get the label for.</param>
        /// <returns>The sprite for a specific side.</returns>
        private Sprite GetSpriteForSide(Vector2Int side)
        {
            if (conduit.IsSideExtracting(side))
            {
                return extractingSprite;
            }

            if (conduit.IsSideConnected(side))
            {
                return connectedSprite;
            }
            else
            {
                return null;
            }
        }
    }
}
