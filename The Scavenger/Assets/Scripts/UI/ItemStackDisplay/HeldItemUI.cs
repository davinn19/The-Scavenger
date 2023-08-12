using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    /// <summary>
    /// Controls UI for the current held item.
    /// </summary>
    [RequireComponent(typeof(SlotDisplay))]
    public class HeldItemUI : MonoBehaviour
    {
        [SerializeField] private PlayerInventory inventory;
        private GameUI gameUI;

        private RectTransform rectTransform;
        private SlotDisplay heldItemDisplay;

        private void Start()
        {
            gameUI = GetComponentInParent<GameUI>();
            rectTransform = transform as RectTransform;
            heldItemDisplay = GetComponent<SlotDisplay>();

            heldItemDisplay.SetWatchedBuffer(inventory, inventory.GetHeldItem());
        }

        private void Update()
        {
            rectTransform.position = gameUI.PointerPos + new Vector2(10, -10);
        }
    }
}
