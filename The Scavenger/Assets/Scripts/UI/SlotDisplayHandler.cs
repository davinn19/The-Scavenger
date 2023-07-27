using UnityEngine;

namespace Scavenger.UI
{
    // TODO rename, add docs
    public class SlotDisplayHandler : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        private HeldItemHandler heldItemHandler;

        private void Awake()
        {
            heldItemHandler = gameManager.HeldItemHandler;
        }

        public void OnTrashPressed()
        {
            heldItemHandler.Use(int.MaxValue);
        }

        public void OnSlotDisplayPressed(SlotDisplay slotDisplay)
        {
            // Auto take if no item is held
            if (heldItemHandler.IsEmpty())
            {
                heldItemHandler.TakeItemsFrom(slotDisplay.Buffer, slotDisplay.Slot);
                return;
            }

            ItemStack displayStack = slotDisplay.GetItemInSlot();
            if (!displayStack)
            {
                heldItemHandler.MoveItemsTo(slotDisplay.Buffer, slotDisplay.Slot);
                return;
            }

            
            // Try inserting held item first
            int itemsInserted = heldItemHandler.MoveItemsTo(slotDisplay.Buffer, slotDisplay.Slot);
            if (itemsInserted > 0)
            {
                return;
            }

            // If no items were inserted, try extracting
            int itemsExtracted = heldItemHandler.TakeItemsFrom(slotDisplay.Buffer, slotDisplay.Slot);
            if (itemsExtracted > 0)
            {
                return;
            }

            // If no items were extracted, swap stacks
            heldItemHandler.Swap(slotDisplay.Buffer, slotDisplay.Slot);

        }
    }
}
