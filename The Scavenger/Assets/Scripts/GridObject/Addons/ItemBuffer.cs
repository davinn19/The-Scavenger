using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Buffer that stores items.
    /// </summary>
    public class ItemBuffer : Buffer
    {
        [field: SerializeField] public int MaxCapacity { get; private set; }
        [field: SerializeField] public int NumSlots { get; private set; }

        public Func<ItemStack, int, bool> AcceptsItemStack = (_,_) => { return true; };

        [field: SerializeField] public ItemStack[] Slots { get; private set; }
        private bool[] slotsLocked;

        public event Action<int> SlotChanged;

        private void Awake()
        {
            ProcessStartingItems();
            slotsLocked = new bool[NumSlots];
        }

        // TODO implement, add docs
        protected override void ReadData(PersistentData data)
        {
            throw new NotImplementedException();
        }

        // TODO implement, add docs
        protected override void WriteData(PersistentData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Resizes slots array to numSlots and confirms all itemStacks set in editor match maxCapacity.
        /// </summary>
        private void ProcessStartingItems()
        {
            ItemStack[] resizedSlots = new ItemStack[NumSlots];

            for (int i = 0; i < Slots.Length; i++)
            {
                ItemStack itemStack = GetItemInSlot(i);
                if (itemStack.Amount <= 0)
                {
                    itemStack.Amount = 1;
                }
                itemStack.Amount = Mathf.Min(itemStack.Amount, MaxCapacity);
                resizedSlots[i] = itemStack;
            }

            for (int i = Slots.Length; i < resizedSlots.Length; i++)
            {
                resizedSlots[i] = new ItemStack();
            }

            Slots = resizedSlots;

            for (int i = 0; i < Slots.Length; i++)
            {
                SlotChanged?.Invoke(i);
            }
        }

        /// <summary>
        /// Gets itemStack in the specified slot.
        /// </summary>
        /// <param name="slot">Slot to retrieve itemStack.</param>
        /// <returns>The itemStack in the specified slot.</returns>
        public ItemStack GetItemInSlot(int slot)
        {
            return Slots[slot];
        }

        /// <summary>
        /// Sets the slot to contain the itemStack.
        /// </summary>
        /// <param name="itemStack">The itemStack to assign to the slot. WILL BE MODIFIED.</param>
        /// <param name="slot">The slot to assign the itemStack to.</param>
        private void SetItemInSlot(ItemStack itemStack, int slot)
        {
            Slots[slot] = itemStack;
            SlotChanged?.Invoke(slot);
        }

        /// <summary>
        /// Checks if a slot is locked.
        /// </summary>
        /// <param name="slot">The slot to check.</param>
        /// <returns>True if the slot is locked.</returns>
        public bool IsLocked(int slot)
        {
            return slotsLocked[slot];
        }

        /// <summary>
        /// Sets a slot as locked or unlocked.
        /// </summary>
        /// <param name="slot">The slot to set.</param>
        /// <param name="locked">If true, sets to locked.</param>
        private void SetLocked(int slot, bool locked)
        {
            slotsLocked[slot] = locked;
            if (!locked)
            {
                ItemStack itemStack = GetItemInSlot(slot);
                if (itemStack.IsEmpty())
                {
                    itemStack.Clear();
                }
            }
            SlotChanged?.Invoke(slot);
        }

        /// <summary>
        /// Switches a slot's locked status.
        /// </summary>
        /// <param name="slot">The slot to toggle.</param>
        public void ToggleLocked(int slot)
        {
            SetLocked(slot, !IsLocked(slot));
        }

        /// <summary>
        /// Extracts from the itemStack in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to extract from.</param>
        /// <param name="amount">The amount to attempt extracting.</param>
        /// <param name="simulate">If true, will not change the itemStack's amount.</param>
        /// <returns>The amount extracted.</returns>
        public int Extract(int slot, int amount, bool simulate = false)
        {
            ItemStack extractedStack = GetItemInSlot(slot);

            // Ignore if extracted stack is empty
            if (!extractedStack)
            {
                return 0;
            }

            int amountExtracted = Mathf.Min(extractedStack.Amount, amount);

            // Don't actually make changes if simulate is true
            if (!simulate)
            {
                extractedStack.Amount -= amountExtracted;
                if (extractedStack.IsEmpty() && !IsLocked(slot))
                {
                    extractedStack.Clear();
                }

                SlotChanged?.Invoke(slot);
            }

            return amountExtracted;
        }

        /// <summary>
        /// Inserts another stack into a specific slot.
        /// </summary>
        /// <param name="slot">The slot to insert into.</param>
        /// <param name="otherStack">The stack to insert into the buffer. Will not be modified.</param>
        /// <param name="amount">The amount to attempt inserting. If not specified, will attempt inserting as much as possible.</param>
        /// <param name="simulate">If true, will only calculate the result and not modify the buffer.</param>
        /// <returns>OtherStack's remaining amount after the insertion.</returns>
        public int Insert(int slot, ItemStack otherStack, int amount = int.MaxValue, bool simulate = false)
        {
            // Ignore if other stack is empty
            if (!otherStack || otherStack.IsEmpty())
            {
                return otherStack.Amount;
            }

            // Ignore if otherStack cannot be accepted into the specific slot
            if (!AcceptsItemStack(otherStack, slot))
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

            int amountInserted = Mathf.Min(amount, otherStack.Amount, MaxCapacity - insertedStack.Amount);

            // Do not change values if in simulate mode
            if (!simulate)
            {
                // In case insertedStack is empty (values are uninitialized), set its item and data equal to otherStack
                if (!insertedStack)
                {
                    insertedStack.SetItem(otherStack.Item);
                    insertedStack.Data = new PersistentData(otherStack.Data);
                }

                insertedStack.Amount += amountInserted;
                SlotChanged?.Invoke(slot);
            }
            
            return otherStack.Amount - amountInserted;
        }

        /// <summary>
        /// Inserts another itemStack into the first available slots in the buffer.
        /// </summary>
        /// <param name="itemStack">The itemStack to insert into the buffer. Will not be modified.</param>
        /// <param name="amount">Limit to the amount of items to insert</param>
        /// <param name="simulate">If true, will only calculate the results, not modifying the buffer.</param>
        /// <returns>The amount remaining in itemStack after insertion.</returns>
        public int Insert(ItemStack itemStack, int amount = int.MaxValue, bool simulate = false)
        {
            // Create copy stack to prevent modifying parameter
            itemStack = new ItemStack(itemStack);

            int insertsRemaining = amount;

            // Fill up existing stacks first
            for (int slot = 0; slot < NumSlots; slot++)
            {
                // Ignore empty slots
                if (!GetItemInSlot(slot))
                {
                    continue;
                }

                if (DoInsert(slot))
                {
                    return itemStack.Amount;
                }
            }

            // Fill up other slots next
            for (int slot = 0; slot < NumSlots; slot++)
            {
                if (DoInsert(slot))
                {
                    return itemStack.Amount;
                }
            }

            return itemStack.Amount;


            // Performs an insert to a slot and updates tracking variables
            bool DoInsert(int slot)
            {
                int remainder = Insert(slot, itemStack, insertsRemaining, simulate);

                insertsRemaining -= itemStack.Amount - remainder;
                itemStack.Amount = remainder;

                return (insertsRemaining == 0 || itemStack.Amount == 0);    // Returns true if loop should be stopped
            }
        }

        // TODO implement, add docs
        // Will factor in data
        public int Extract(ItemStack itemStack, int amount = int.MaxValue, bool simulate = false)
        {
            // Ignore if itemStack is empty
            if (!itemStack || itemStack.IsEmpty())
            {
                return 0;
            }

            int extractsRemaining = amount;

            for (int slot = 0; slot < NumSlots; slot++)
            {
                ItemStack slotStack = GetItemInSlot(slot);
                if (!slotStack || !itemStack.IsStackable(slotStack))
                {
                    continue;
                }

                int amountExtracted = Extract(slot, extractsRemaining, simulate);
                extractsRemaining -= amountExtracted;

                if (extractsRemaining == 0)
                {
                    return amount;
                }
            }

            return amount - extractsRemaining;
        }

        // TODO implement, add docs, combine with include data version of this function
        // Will ignore data
        public int Extract(Item item, int amount = int.MaxValue, bool simulate = false)
        {
            int extractsRemaining = amount;

            for (int slot = 0; slot < NumSlots; slot++)
            {
                ItemStack slotStack = GetItemInSlot(slot);
                if (!slotStack || slotStack.Item != item)
                {
                    continue;
                }

                int amountExtracted = Extract(slot, extractsRemaining, simulate);
                extractsRemaining -= amountExtracted;

                if (extractsRemaining == 0)
                {
                    return amount;
                }
            }

            return amount - extractsRemaining;
        }

        /// <summary>
        /// Swaps itemStacks in the slots of the buffers.
        /// </summary>
        /// <param name="buffer1">Buffer containing one of the itemStacks.</param>
        /// <param name="slot1">Slot to get the first itemStack from.</param>
        /// <param name="buffer2">Buffer containing the other itemStack.</param>
        /// <param name="slot2">Slot to get the second itemStack from.</param>
        public static void Swap(ItemBuffer buffer1, int slot1, ItemBuffer buffer2, int slot2)
        {
            if (buffer1.IsLocked(slot1) || buffer2.IsLocked(slot2))
            {
                return;
            }

            ItemStack itemStack1 = buffer1.GetItemInSlot(slot1);
            ItemStack itemStack2 = buffer2.GetItemInSlot(slot2);

            // TODO check for max capacity
            if (itemStack1 == itemStack2 || !buffer1.AcceptsItemStack(itemStack2, slot1) || !buffer2.AcceptsItemStack(itemStack1, slot2))
            {
                return;
            }

            buffer1.SetItemInSlot(itemStack2, slot1);
            buffer2.SetItemInSlot(itemStack1, slot2);
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
        public static int MoveItems(ItemBuffer sendingBuffer, int sendingSlot, ItemBuffer receivingBuffer, int receivingSlot, int amount = int.MaxValue)
        {
            ItemStack movedStack = sendingBuffer.GetItemInSlot(sendingSlot);
            int remainder = receivingBuffer.Insert(receivingSlot, movedStack, amount);

            int itemsMoved = movedStack.Amount - remainder;
            sendingBuffer.Extract(sendingSlot, itemsMoved);

            return itemsMoved;
        }


    }
}
