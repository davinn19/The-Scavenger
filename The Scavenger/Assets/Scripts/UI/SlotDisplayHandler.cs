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

        [SerializeField] private SlotDisplay pressedSlot = null;
        [SerializeField] private bool dragging = false;


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

            dragging = false;

            SlotDisplay destinationDisplay = GetDisplayUnderPointer();
            if (!destinationDisplay || destinationDisplay == slotDisplay)
            {
                return;
            }

            ItemBuffer buffer1 = pressedSlot.buffer;
            int slot1 = pressedSlot.slot;

            ItemBuffer buffer2 = destinationDisplay.buffer;
            int slot2 = destinationDisplay.slot;

            ItemBuffer.Swap(buffer1, slot1, buffer2, slot2);
            pressedSlot = null;
        }

        public SlotDisplay GetDisplayUnderPointer()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = pointerHover.ReadValue<Vector2>() };


            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                SlotDisplay slotDisplay;

                if (result.gameObject.TryGetComponent(out slotDisplay))
                {
                    return slotDisplay;
                }
            }

            return null;
        }
    }
}
