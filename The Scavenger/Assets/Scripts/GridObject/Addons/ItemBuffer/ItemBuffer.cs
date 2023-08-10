using Leguar.TotalJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Scavenger.ItemTransfer;

namespace Scavenger
{
    /// <summary>
    /// Buffer that stores items.
    /// </summary>
    public abstract class ItemBuffer : Buffer   // TODO add docs
    {
        public virtual int MaxStackSize => 100;
        public int NumSlots => GetItems().Count;

        public event Action<int> SlotChanged;

        private void Awake() => Init();
        protected virtual void Init() { }


        public abstract List<ItemStack> GetItems(ItemInteractionType interactionType = ItemInteractionType.All);

        /// <summary>
        /// Gets itemStack in the specified slot.
        /// </summary>
        /// <param name="slot">Slot to retrieve itemStack.</param>
        /// <returns>The itemStack in the specified slot.</returns>
        public ItemStack GetItemInSlot(int slot)
        {
            return GetItems()[slot];
        }

        /// <summary>
        /// Sets the slot to contain the itemStack.
        /// </summary>
        /// <param name="itemStack">The itemStack to assign to the slot. WILL NOT BE MODIFIED.</param>
        /// <param name="slot">The slot to assign the itemStack to.</param>
        public void SetItemInSlot(int slot, ItemStack itemStack)
        {
            ItemStack oldItemStack = GetItemInSlot(slot);
            oldItemStack.Copy(itemStack);
            if (oldItemStack.Amount > MaxStackSize)
            {
                oldItemStack.Amount = MaxStackSize;
            }
        }

        // TODO add docs
        public void Clear()
        {
            foreach (ItemStack itemStack in GetItems())
            {
                itemStack.Clear();
            }
        }

        public abstract List<int> GetInteractibleSlots(ItemInteractionType interactionType);

        /// <summary>
        /// Checks if the item buffer allows an itemStack to exist in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to check.</param>
        /// <param name="itemStack">The itemStack to check.</param>
        /// <returns>True if the itemStack is allowed there.</returns>
        public abstract bool AcceptsItemStack(int slot, ItemStack itemStack);


        /// <summary>
        /// Extracts from the itemStack in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to extract from.</param>
        /// <param name="amount">The amount to attempt extracting.</param>
        /// <returns>The amount extracted.</returns>
        public virtual int ExtractSlot(int slot, int amount)
        {
            ItemStack extractedStack = GetItemInSlot(slot);
            return ExtractItem(extractedStack, (_) => true, amount, false);
        }

        /// <summary>
        /// Inserts another stack into a specific slot.
        /// </summary>
        /// <param name="slot">The slot to insert into.</param>
        /// <param name="otherStack">The stack to insert into the buffer. WILL NOT BE MODIFIED.</param>
        /// <param name="amount">The amount to attempt inserting. If not specified, will attempt inserting as much as possible.</param>
        /// <returns>OtherStack's remaining amount after the insertion.</returns>
        public virtual int InsertSlot(int slot, ItemStack otherStack, int amount)
        {
            // Ignore if other stack is empty
            if (!otherStack || otherStack.IsEmpty())
            {
                return otherStack.Amount;
            }

            // Ignore if otherStack cannot be accepted into the specific slot
            if (!AcceptsItemStack(slot, otherStack))
            {
                return otherStack.Amount;
            }

            // Get stack you will insert into
            ItemStack insertedStack = GetItemInSlot(slot);

            // Ignore if stacks are unstackable
            if (!insertedStack.IsStackable(otherStack))
            {
                return otherStack.Amount;
            }

            int amountInserted = Mathf.Min(amount, otherStack.Amount, MaxStackSize - insertedStack.Amount);

            // In case insertedStack is empty (values are uninitialized), set its item and data equal to otherStack
            if (!insertedStack)
            {
                insertedStack = otherStack.Clone(amount : 0);
            }

            insertedStack.Amount += amountInserted;
            return otherStack.Amount - amountInserted;
        }

        /// <summary>
        /// Checks if the buffer contains no items.
        /// </summary>
        /// <returns>True if the buffer contains no items.</returns>
        public bool IsEmpty()
        {
            foreach (ItemStack itemStack in GetItems())
            {
                if (itemStack || !itemStack.IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }

        // TODO add docs
        public bool IsSlotFull(int slot)
        {
            return GetItemInSlot(slot).Amount == MaxStackSize;
        }

        /// <summary>
        /// Moves items in one slot of a buffer to another slot of another buffer.
        /// </summary>
        /// <param name="sendingBuffer">Buffer giving the items.</param>
        /// <param name="sendingSlot">Slot to extract from.</param>
        /// <param name="receivingBuffer">Buffer receiving the items.</param>
        /// <param name="receivingSlot">Slot to insert items to.</param>
        /// <param name="amount">Limit for amount of items to move.</param>
        /// <returns>Number of items moved.</returns>
        public static int TransferItemInSlot(ItemBuffer sendingBuffer, int sendingSlot, ItemBuffer receivingBuffer, int receivingSlot, int amount = int.MaxValue)
        {
            ItemStack movedStack = sendingBuffer.GetItemInSlot(sendingSlot);
            int remainder = receivingBuffer.InsertSlot(receivingSlot, movedStack, amount);

            int itemsMoved = movedStack.Amount - remainder;
            sendingBuffer.ExtractSlot(sendingSlot, itemsMoved);

            return itemsMoved;
        }
    }
}
