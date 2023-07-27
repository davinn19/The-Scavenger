using UnityEngine;

namespace Scavenger.UI
{
    // TODO rename, add docs
    public class SlotDisplayHandler : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private HeldItemHandler heldItemHandler;
        private ItemBuffer inventory;
        private InputHandler inputHandler;


        private void Awake()
        {
            heldItemHandler = gameManager.HeldItemHandler;
            inventory = gameManager.Inventory;

            inputHandler = gameManager.InputHandler;
            inputHandler.InputModeChanged += OnInputModeChanged;
        }

        public void OnTrashPressed()
        {
            heldItemHandler.Use(int.MaxValue);
        }

        public void OnSlotDisplayPressed(SlotDisplay slotDisplay)
        {
            bool noItemHeld = heldItemHandler.IsEmpty();
            ItemStack displayStack = slotDisplay.GetItemInSlot();

            // Immediately end interaction if slot is empty and no item is held
            if (noItemHeld && !displayStack)  
            {
                return;
            }

            // Automatically set to interact mode
            inputHandler.InputMode = InputMode.Interact;

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

        private void OnInputModeChanged(InputMode _)
        {
            heldItemHandler.Dump(inventory);
        }
    }
}
