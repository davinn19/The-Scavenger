using UnityEngine;

namespace Scavenger.UI
{
    public class SlotDisplayHandler : MonoBehaviour
    {
        [SerializeField] private ItemSelection itemSelection;

        public void OnTrashPressed()
        {
            itemSelection.Use(int.MaxValue);
        }

        public void OnSlotDisplayPressed(SlotDisplay slotDisplay)
        {
            // Auto take if no item is held
            if (itemSelection.IsEmpty())
            {
                itemSelection.TakeItemsFrom(slotDisplay.Buffer, slotDisplay.Slot);
                return;
            }

            ItemStack displayStack = slotDisplay.GetItemInSlot();
            if (!displayStack)
            {
                itemSelection.MoveItemsTo(slotDisplay.Buffer, slotDisplay.Slot);
                return;
            }

            
            // Try inserting held item first
            int itemsInserted = itemSelection.MoveItemsTo(slotDisplay.Buffer, slotDisplay.Slot);
            if (itemsInserted > 0)
            {
                return;
            }

            // If no items were inserted, try extracting
            int itemsExtracted = itemSelection.TakeItemsFrom(slotDisplay.Buffer, slotDisplay.Slot);
            if (itemsExtracted > 0)
            {
                return;
            }

            // If no items were extracted, swap stacks
            itemSelection.Swap(slotDisplay.Buffer, slotDisplay.Slot);

        }
    }
}
