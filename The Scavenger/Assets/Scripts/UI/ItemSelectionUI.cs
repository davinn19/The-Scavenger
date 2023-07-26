using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    [RequireComponent(typeof(SlotDisplay))]
    public class ItemSelectionUI : MonoBehaviour
    {
        [SerializeField] private ItemSelection itemSelection;
        private GameUI gameUI;

        private RectTransform rectTransform;
        private SlotDisplay heldItemDisplay;

        private void Awake()
        {
            gameUI = GetComponentInParent<GameUI>();
            rectTransform = transform as RectTransform;
            heldItemDisplay = GetComponent<SlotDisplay>();

            heldItemDisplay.SetWatchedBuffer(itemSelection.HeldItemBuffer, 0);
        }

        private void Update()
        {
            rectTransform.position = gameUI.PointerPos;
        }
    }
}
