using UnityEngine;

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

            ItemBuffer sendingBuffer = slotDisplay.Buffer;
            int sendingSlot = slotDisplay.Slot;
            ItemStack sendingStack = sendingBuffer.GetItemInSlot(sendingSlot);

            if (sendingStack.IsEmpty())
            {
                return;
            }

            ItemBuffer receivingBuffer = hoveredSlot.Buffer;
            int receivingSlot = hoveredSlot.Slot;
            ItemStack receivingStack = receivingBuffer.GetItemInSlot(receivingSlot);


            if (receivingStack.IsStackable(sendingStack))
            {
                ItemBuffer.MoveItems(sendingBuffer, sendingSlot, receivingBuffer, receivingSlot);
            }
            else
            {
                ItemBuffer.Swap(sendingBuffer, sendingSlot, receivingBuffer, receivingSlot);
            }
            

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
