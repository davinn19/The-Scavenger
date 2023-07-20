using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scavenger.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUI : MonoBehaviour
    {
        public const int InventoryWidth = 7;
        public const int InventoryHeight = 4;

        [SerializeField] private ItemSelection itemSelection;
        [SerializeField] private ItemBuffer inventory;
        private SlotDisplay[] slotDisplays;

        [SerializeField] private Rect closedState;
        [SerializeField] private Rect openState;

        [SerializeField] private RectTransform selectedItemIndicator;

        private void Awake()
        {
            InitSlotDisplays();
        }

        private void InitSlotDisplays()
        {
            slotDisplays = GetComponentsInChildren<SlotDisplay>();

            for (int slot = 0; slot < slotDisplays.Length; slot++)
            {
                SlotDisplay slotDisplay = slotDisplays[slot];
                slotDisplay.Buffer = inventory;
                slotDisplay.Slot = slot;
            }
        }


        private void Update()
        {
            // TODO link to event
            selectedItemIndicator.anchoredPosition = GetSlotDisplayPos(itemSelection.SelectedSlot);
        }

        private Vector2 GetSlotDisplayPos(int slotIndex)
        {
            int x = slotIndex % InventoryWidth;
            int y = - slotIndex / InventoryWidth;
            return new Vector2(x, y) * 55;
        }
    }
}
