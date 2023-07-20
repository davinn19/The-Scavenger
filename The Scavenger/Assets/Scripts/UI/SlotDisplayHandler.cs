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
        private Controls controls;
        private InputAction pointerHover;

        [SerializeField] private SlotDisplay pressedSlot;
        private bool dragging = false;

        [SerializeField] private ItemSelection itemSelection;

        private void Awake()
        {
            controls = new Controls();
        }

        private void OnEnable()
        {
            pointerHover = controls.GridMap.PointerHover;
            pointerHover.Enable();
        }

        private void OnDisable()
        {
            pointerHover.Disable();
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

            SlotDisplay destinationDisplay = GetDisplayUnderPointer();
            if (!destinationDisplay || destinationDisplay == slotDisplay)
            {
                return;
            }

            ItemBuffer buffer1 = slotDisplay.Buffer;
            int slot1 = slotDisplay.Slot;

            ItemBuffer buffer2 = destinationDisplay.Buffer;
            int slot2 = destinationDisplay.Slot;

            ItemBuffer.Swap(buffer1, slot1, buffer2, slot2);
           
        }

        public SlotDisplay GetDisplayUnderPointer()
        {
            PointerEventData pointerData = new(EventSystem.current) { position = pointerHover.ReadValue<Vector2>() };

            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerData, results);
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.TryGetComponent(out SlotDisplay slotDisplay))
                {
                    return slotDisplay;
                }
            }

            return null;
        }
    }
}
