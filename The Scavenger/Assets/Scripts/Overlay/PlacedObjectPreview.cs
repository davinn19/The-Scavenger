using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Shows a preview of an item's usage on the grid.
    /// </summary>
    public class PlacedObjectPreview : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        private InputHandler inputHandler;
        private HeldItemBuffer heldItemBuffer;
        private GridMap map;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            inputHandler = gameManager.InputHandler;
            heldItemBuffer = gameManager.HeldItemBuffer;
            map = gameManager.Map;

            inputHandler.PointerMoved += UpdateAppearance;
            heldItemBuffer.HeldItemChanged += UpdateAppearance;

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Updates the preview's sprite to match the item's placed object and moves it to the correct grid space.
        /// </summary>
        private void UpdateAppearance()
        {
            Vector2Int hoveredPos = inputHandler.HoveredGridPos;
            ItemStack heldItem = heldItemBuffer.GetHeldItem();

            if (inputHandler.OverGUI || map.GetObjectAtPos(hoveredPos) || !heldItem || !heldItem.Item.TryGetProperty(out PlacedObject placedObject))
            {
                spriteRenderer.enabled = false;
                return;
            }

            spriteRenderer.enabled = true;
            spriteRenderer.sprite = placedObject.Object.GetModel().GetThumbnail();
            transform.position = GridMap.GetCenterOfTile(hoveredPos);
        }
    }
}
