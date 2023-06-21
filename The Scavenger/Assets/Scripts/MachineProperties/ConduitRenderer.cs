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
        private SpriteResolver spriteResolver;
        private Conduit conduit;
        private SpriteLibrary spriteLibrary;

        private void Start()
        {
            conduit = GetComponent<Conduit>();
            spriteResolver = GetComponent<SpriteResolver>();
            spriteLibrary = GetComponent<SpriteLibrary>();
        }

        private void Update()
        {
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            string connectedSides = "";

            for (int sideIndex = 0; sideIndex < 4; sideIndex++)
            {
                Vector2Int side = GridMap.adjacentDirections[sideIndex];
                if (conduit.IsSideConnected(side))
                {
                    connectedSides += sideIndex;
                }

            }

            if (connectedSides == "")
            {
                connectedSides = "None";
            }

            spriteResolver.SetCategoryAndLabel("Conduit", connectedSides);

        }
    }
}
