using System;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Tracks the player's currently held item.
    /// </summary>
    [RequireComponent(typeof(ItemBuffer))]
    public class HeldItemHandler : MonoBehaviour
    {
        /// <remarks>DO NOT EDIT DIRECTLY. Use other methods to change the held item.</remarks>
        public ItemBuffer HeldItemBuffer { get; private set; }
        public event Action HeldItemChanged;

        private ItemDropper itemDropper;
        private InputHandler inputHandler;

        private void Awake()
        {
            HeldItemBuffer = GetComponent<ItemBuffer>();
            HeldItemBuffer.SlotChanged += (_) => { HeldItemChanged?.Invoke(); }; 

            GameManager gameManager = GetComponentInParent<GameManager>();
            itemDropper = gameManager.ItemDropper;
            inputHandler = gameManager.InputHandler;
        }

        /// <summary>
        /// Gets the currently held itemStack.
        /// </summary>
        /// <returns>The currently held itemStack.</returns>
        public ItemStack GetHeldItem()
        {
            return HeldItemBuffer.GetItemInSlot(0);
        }

        /// <summary>
        /// Checks if an item is being held or not.
        /// </summary>
        /// <returns>True if no item is currently held.</returns>
        public bool IsEmpty()
        {
            ItemStack heldItem = GetHeldItem();
            return !heldItem || heldItem.IsEmpty();
        }

        

        /// <summary>
        /// Moves items from held item buffer to another buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to insert items into.</param>
        /// <param name="slot">Slot to insert into.</param>
        /// <returns>Amount of items moved.</returns>
        public int MoveItemsTo(ItemBuffer otherBuffer, int slot)
        {
            return ItemBuffer.TransferItemInSlot(HeldItemBuffer, 0, otherBuffer, slot);
        }

        /// <summary>
        /// Moves items from another buffer to held item buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to extract items from.</param>
        /// <param name="slot">Slot to extract from.</param>
        /// <returns>Amount of items moved.</returns>
        public int TakeItemsFrom(ItemBuffer otherBuffer, int slot)
        {
            return ItemBuffer.TransferItemInSlot(otherBuffer, slot, HeldItemBuffer, 0);
        }

        /// <summary>
        /// Swaps itemStacks between held item buffer and a slot in another buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to swap with.</param>
        /// <param name="slot">Slot to swap with.</param>
        public void Swap(ItemBuffer otherBuffer, int slot)
        {
            ItemBuffer.Swap(HeldItemBuffer, 0, otherBuffer, slot);
        }

        /// <summary>
        /// Dumps held item contents to another buffer and converts the remainder to a floating item.
        /// </summary>
        /// <param name="otherBuffer">The buffer to insert into.</param>
        public void Dump(ItemBuffer otherBuffer)
        {
            // Ignore if no item is held
            if (IsEmpty())
            {
                return;
            }    

            // Dump to other buffer first
            int remainder = otherBuffer.Insert(HeldItemBuffer.GetItemInSlot(0));

            // Converts remainder to floating item
            if (remainder > 0)
            {
                ItemStack floatingItemStack = new ItemStack(HeldItemBuffer.GetItemInSlot(0), remainder);

                itemDropper.CreateFloatingItem(floatingItemStack, inputHandler.HoveredWorldPos);
            }

            // Use up entire amount
            Use(int.MaxValue);
        }
    }
}
