using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Buffer that stores items.
    /// </summary>
    public abstract class ItemBuffer : Buffer   // TODO add docs
    {
        public abstract int NumSlots { get; }

        public event Action<ItemStack> SlotChanged;

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            foreach (ItemStack slot in GetAllSlots())
            {
                slot.Changed += () => { SlotChanged?.Invoke(slot); };
            }
        }

        /// <summary>
        /// Returns references to the item buffer's itemStacks.
        /// </summary>
        /// <param name="category">Set this to only return specific slots based on a conduit interaction.</param>
        /// <returns>References to the available itemStacks.</returns>
        public abstract List<ItemStack> GetAllSlots();
        public abstract List<ItemStack> GetInputSlots();
        public abstract List<ItemStack> GetOutputSlots();

        /// <summary>
        /// Gets itemStack in the specified slot.
        /// </summary>
        /// <param name="slot">Slot to retrieve itemStack.</param>
        /// <returns>The itemStack in the specified slot.</returns>
        public abstract ItemStack GetItemInSlot(int slot);

        /// <summary>
        /// Sets the slot to contain the itemStack.
        /// </summary>
        /// <param name="itemStack">The itemStack to assign to the slot. WILL NOT BE MODIFIED.</param>
        /// <param name="slot">The slot to assign the itemStack to.</param>
        public virtual void SetItemInSlot(int slot, ItemStack itemStack)
        {
            ItemStack oldItemStack = GetItemInSlot(slot);
            oldItemStack.Copy(itemStack);
        }

        // TODO add docs
        public void Clear()
        {
            foreach (ItemStack itemStack in GetAllSlots())
            {
                itemStack.Clear();
            }
        }

        /// <summary>
        /// Checks if the item buffer allows an itemStack to exist in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to check.</param>
        /// <param name="itemStack">The itemStack to check.</param>
        /// <returns>True if the itemStack is allowed there.</returns>
        public abstract bool AcceptsItemStack(ItemStack slot, ItemStack itemStack);
        public bool AcceptsItemStack(int slot, ItemStack itemStack) => AcceptsItemStack(GetItemInSlot(slot), itemStack);
        

        /// <summary>
        /// Checks if the buffer contains no items.
        /// </summary>
        /// <returns>True if the buffer contains no items.</returns>
        public bool IsEmpty()
        {
            foreach (ItemStack itemStack in GetAllSlots())
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
            return GetItemInSlot(slot).IsFull();
        }

        // Checks if the itemStack is being stored internally by the buffer
        // NOT FOR COMPARING VALUES
        public bool ItemStackIsInBuffer(ItemStack itemStack)
        {
            return GetAllSlots().Contains(itemStack);
        }

        public int GetItemStackSlot(ItemStack itemStack)
        {
            return GetAllSlots().IndexOf(itemStack);
        }


        /// <summary>
        /// Extracts from the itemStack in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to extract from.</param>
        /// <param name="amount">The amount to attempt extracting.</param>
        /// <returns>The amount extracted.</returns>
        public int ExtractSlot(int slot, int amount)
        {
            ItemStack extractedStack = GetItemInSlot(slot);
            return ItemTransfer.ExtractItem(extractedStack, (_) => true, amount, false);
        }
    }
}
