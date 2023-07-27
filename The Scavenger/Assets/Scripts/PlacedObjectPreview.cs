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
        [SerializeField] private InputHandler inputHandler;
        [SerializeField] private HeldItemHandler heldItemHandler;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            inputHandler.PointerMoved += UpdateAppearance;
            heldItemHandler.HeldItemChanged += UpdateAppearance;

            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Updates the preview's sprite to match the item's placed object and moves it to the correct grid space.
        /// </summary>
        private void UpdateAppearance()
        {
            ItemStack heldItem = heldItemHandler.GetHeldItem();

            if (inputHandler.OverGUI || !heldItem || !heldItem.Item.HasProperty<PlacedObject>())
            {
                enabled = false;
                return;
            }

            enabled = true;
            spriteRenderer.sprite = heldItem.Item.Icon;
            transform.position = GridMap.GetCenterOfTile(inputHandler.HoveredGridPos);
        }
    }
}
