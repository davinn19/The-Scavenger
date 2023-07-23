using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Scavenger.UI
{
    // TODO fix dragging bugs
    public class SlotDisplayHandler : MonoBehaviour
    {
        [SerializeField] private SlotDisplay pressedSlot;
        private bool dragging = false;

        [SerializeField] private ItemSelection itemSelection;
        [SerializeField] private GameObject trash;

        private GameUI gameUI;


        private void Awake()
        {
            gameUI = GetComponentInParent<GameUI>();
        }

        public void OnSlotDisplayDown(SlotDisplay slotDisplay)
        {
            if (!pressedSlot)
            {
                pressedSlot = slotDisplay;
                itemSelection.SelectedSlot = slotDisplay.transform.GetSiblingIndex();
            }
        }

        public void OnSlotDisplayExit(SlotDisplay slotDisplay)
        {
            if (pressedSlot && pressedSlot == slotDisplay)
            {
                dragging = true;
            }
           
        }
        public void OnSlotDisplayUp(SlotDisplay slotDisplay)
        {
            if (!pressedSlot || pressedSlot != slotDisplay)
            {
                return;
            }

            pressedSlot = null;
            dragging = false;

            if (IsOverTrash())
            {
                slotDisplay.GetItemInSlot().Clear();    // TODO move through buffer instead of accessing itemstack directly
                return;
            }

            SlotDisplay hoveredSlot = GetHoveredSlot();
            if (!hoveredSlot || hoveredSlot == slotDisplay)
            {
                return;
            }

            ItemBuffer buffer1 = slotDisplay.Buffer;
            int slot1 = slotDisplay.Slot;

            ItemBuffer buffer2 = hoveredSlot.Buffer;
            int slot2 = hoveredSlot.Slot;

            ItemBuffer.Swap(buffer1, slot1, buffer2, slot2);
           
        }

        private bool IsOverTrash()
        {
            return gameUI.HoveredElement == trash;
        }


        private SlotDisplay GetHoveredSlot()
        {
            if (!gameUI.HoveredElement)
            {
                return null;
            }
            return gameUI.HoveredElement.GetComponent<SlotDisplay>();
        }
    }
}
