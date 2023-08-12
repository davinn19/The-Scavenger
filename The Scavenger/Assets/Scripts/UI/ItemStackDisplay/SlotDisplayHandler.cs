using UnityEngine;

namespace Scavenger.UI
{
    // TODO rename, add docs
    public class SlotDisplayHandler : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private PlayerInventory inventory;
        private InputHandler inputHandler;


        private void Awake()
        {
            inventory = gameManager.Inventory;

            inputHandler = gameManager.InputHandler;
            inputHandler.InputModeChanged += OnInputModeChanged;
        }

        public void OnTrashPressed()
        {
            inventory.UseHeldItem(int.MaxValue);
        }

        public void OnSlotDisplayPressed(SlotDisplay slotDisplay)
        {
            bool quickPressed = false; // TODO add shift input

            ItemStack heldItem = inventory.GetHeldItem();
            ItemStack displayStack = slotDisplay.GetDisplayedItemStack();

            // Immediately end interaction if slot is empty and no item is held
            if (!heldItem && !displayStack)  
            {
                return;
            }

            // Automatically set to interact mode
            inputHandler.InputMode = InputMode.Interact;

            // Try inserting held item first
            int itemsInserted = ItemTransfer.MoveStackToStack(heldItem, displayStack, heldItem.Amount, slotDisplay.Buffer.AcceptsItemStack, false);
            if (itemsInserted > 0)
            {
                return;
            }

            // If no items were inserted, try extracting
            int itemsExtracted = ItemTransfer.MoveStackToStack(displayStack, heldItem, displayStack.Amount, inventory.AcceptsItemStack, false);
            if (itemsExtracted > 0)
            {
                return;
            }

            // If no items were extracted, swap stacks
            ItemTransfer.Swap(heldItem, displayStack, inventory.AcceptsItemStack, slotDisplay.Buffer.AcceptsItemStack);
        }

        private void OnInputModeChanged(InputMode _)
        {
            inventory.MoveHeldItemToInventory();
        }
    }
}
