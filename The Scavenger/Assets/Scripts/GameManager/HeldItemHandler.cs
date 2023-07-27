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


        private void Awake()
        {
            HeldItemBuffer = GetComponent<ItemBuffer>();
            HeldItemBuffer.SlotChanged += InvokeChangedEvent;
        }

        /// <summary>
        /// Invokes the changed event whenever the held item is changed.
        /// </summary>
        /// <remarks>The slot changed is ignored because there is only one slot.</remarks>
        private void InvokeChangedEvent(int _)
        {
            HeldItemChanged?.Invoke();
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
        /// Extracts from the held item buffer.
        /// </summary>
        /// <param name="amount">Amount of items to use.</param>
        /// <returns>Amount of items actually used.</returns>
        public int Use(int amount = 1)
        {
            Debug.Assert(!HeldItemBuffer.GetItemInSlot(0).IsEmpty());
            return HeldItemBuffer.Extract(0, amount);
        }

        /// <summary>
        /// Moves items from held item buffer to another buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to insert items into.</param>
        /// <param name="slot">Slot to insert into.</param>
        /// <returns>Amount of items moved.</returns>
        public int MoveItemsTo(ItemBuffer otherBuffer, int slot)
        {
            return ItemBuffer.MoveItems(HeldItemBuffer, 0, otherBuffer, slot);
        }

        /// <summary>
        /// Moves items from another buffer to held item buffer.
        /// </summary>
        /// <param name="otherBuffer">Buffer to extract items from.</param>
        /// <param name="slot">Slot to extract from.</param>
        /// <returns>Amount of items moved.</returns>
        public int TakeItemsFrom(ItemBuffer otherBuffer, int slot)
        {
            return ItemBuffer.MoveItems(otherBuffer, slot, HeldItemBuffer, 0);
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
    }
}
