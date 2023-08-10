using Leguar.TotalJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Buffer that stores items.
    /// </summary>
    public abstract class ItemBuffer : Buffer
    {
        public enum SlotType
        {
            Input,
            Output
        }

        public virtual int MaxStackSize => 100;

        public event Action<int> SlotChanged;

        /// <summary>
        /// Gets internal itemStack array.
        /// </summary>
        /// <remarks>Returns a REFERENCE to the array. Do not return a copy when implementing.</remarks>
        /// <returns>The internal itemStack array.</returns>
        public abstract ItemStack[] GetItems();
        public abstract ItemStack[] GetItemsPartial(SlotType slotType);

        public abstract void SetItems(ItemStack[] items);
        public abstract void Clear();
        public virtual int GetNumSlots() => GetItems().Length;
        

        /// <summary>
        /// Checks if the item buffer allows an itemStack to exist in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to check.</param>
        /// <param name="itemStack">The itemStack to check.</param>
        /// <returns>True if the itemStack is allowed there.</returns>
        public abstract bool AcceptsItemStack(int slot, ItemStack itemStack);
        public abstract bool CanInsert(int slot);
        public abstract bool CanExtract(int slot);

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
        /// <param name="itemStack">The itemStack to assign to the slot. WILL BE MODIFIED.</param>
        /// <param name="slot">The slot to assign the itemStack to.</param>
        protected void SetItemInSlot(int slot, ItemStack itemStack)
        {
            GetItems()[slot] = itemStack;
        }

        /// <summary>
        /// Extracts from the itemStack in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to extract from.</param>
        /// <param name="amount">The amount to attempt extracting.</param>
        /// <returns>The amount extracted.</returns>
        public virtual int ExtractSlot(int slot, int amount)
        {
            ItemStack extractedStack = GetItemInSlot(slot);

            // Ignore if extracted stack is empty
            if (!extractedStack)
            {
                return 0;
            }

            int amountExtracted = Mathf.Min(extractedStack.Amount, amount);

            extractedStack.Amount -= amountExtracted;
            if (extractedStack.IsEmpty())
            {
                extractedStack = default;
            }

            SetItemInSlot(slot, extractedStack);
            SlotChanged?.Invoke(slot);

            return amountExtracted;
        }

        /// <summary>
        /// Inserts another stack into a specific slot.
        /// </summary>
        /// <param name="slot">The slot to insert into.</param>
        /// <param name="otherStack">The stack to insert into the buffer. Will not be modified.</param>
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
                insertedStack = new ItemStack(otherStack, 0);
            }

            insertedStack.Amount += amountInserted;
            SetItemInSlot(slot, insertedStack);

            return otherStack.Amount - amountInserted;
        }

        public bool IsEmpty()
        {
            if (GetItems().Length == 0)
            {
                return true;
            }

            return Array.TrueForAll(GetItems(), itemStack => !itemStack || itemStack.IsEmpty());
        }

        // TODO add docs
        public bool IsSlotFull(int slot)
        {
            return GetItemInSlot(slot).Amount == MaxStackSize;

        }

        protected void InvokeSlotChanged(int slot)
        {
            SlotChanged?.Invoke(slot);
        }


        public override void ReadPersistentData(JSON data)
        {
            if (data.ContainsKey("Items"))
            {
                SetItems(data.GetJArray("Items").Deserialize<ItemStack[]>());
            }
        }

        public override JSON WritePersistentData()
        {
            JSON data = new JSON();

            if (!IsEmpty())
            {
                data.Add("Items", JArray.Serialize(GetItems()));
            }

            return data;
        }


        //private void Awake()
        //{
        //    ProcessStartingItems();
        //    slotsLocked = new bool[NumSlots];
        //}

        ///// <summary>
        ///// Resizes slots array to numSlots and confirms all itemStacks set in editor match maxCapacity.
        ///// </summary>
        //private void ProcessStartingItems()
        //{
        //    ItemStack[] resizedSlots = new ItemStack[NumSlots];

        //    for (int i = 0; i < Slots.Length; i++)
        //    {
        //        ItemStack itemStack = GetItemInSlot(i);
        //        if (itemStack.Amount <= 0)
        //        {
        //            itemStack.Amount = 1;
        //        }
        //        itemStack.Amount = Mathf.Min(itemStack.Amount, MaxCapacity);
        //        resizedSlots[i] = itemStack;
        //    }

        //    Slots = resizedSlots;

        //    for (int i = 0; i < Slots.Length; i++)
        //    {
        //        SlotChanged?.Invoke(i);
        //    }
        //}

        //// TODO implement, add docs
        //// Will factor in data
        //public int Extract(ItemStack itemStack, int amount = int.MaxValue, bool simulate = false)
        //{
        //    // Ignore if itemStack is empty
        //    if (!itemStack || itemStack.IsEmpty())
        //    {
        //        return 0;
        //    }

        //    int extractsRemaining = amount;

        //    for (int slot = 0; slot < NumSlots; slot++)
        //    {
        //        ItemStack slotStack = GetItemInSlot(slot);
        //        if (!slotStack || !itemStack.IsStackable(slotStack))
        //        {
        //            continue;
        //        }

        //        int amountExtracted = Extract(slot, extractsRemaining, simulate);
        //        extractsRemaining -= amountExtracted;

        //        if (extractsRemaining == 0)
        //        {
        //            return amount;
        //        }
        //    }

        //    return amount - extractsRemaining;
        //}

        //// TODO implement, add docs, combine with include data version of this function
        //// Will ignore data
        //public int Extract(Item item, int amount = int.MaxValue, bool simulate = false)
        //{
        //    int extractsRemaining = amount;

        //    for (int slot = 0; slot < NumSlots; slot++)
        //    {
        //        ItemStack slotStack = GetItemInSlot(slot);
        //        if (!slotStack || slotStack.Item != item)
        //        {
        //            continue;
        //        }

        //        int amountExtracted = Extract(slot, extractsRemaining, simulate);
        //        extractsRemaining -= amountExtracted;

        //        if (extractsRemaining == 0)
        //        {
        //            return amount;
        //        }
        //    }

        //    return amount - extractsRemaining;
        //}

        /// <summary>
        /// Inserts another itemStack into the first available slots in the buffer.
        /// </summary>
        /// <param name="buffer">The item buffer to perform insertion on.</param>
        /// <param name="itemStack">The itemStack to insert into the buffer. Will not be modified.</param>
        /// <param name="amount">Limit to the amount of items to insert</param>
        /// <returns>The amount remaining in itemStack after insertion.</returns>
        public static int InsertAvailableSlots(ItemBuffer buffer, ItemStack itemStack, int amount = int.MaxValue)
        {
            int insertsRemaining = amount;

            for (int slot = 0; slot < buffer.GetNumSlots(); slot++)
            {
                if (!buffer.CanInsert(slot))
                {
                    continue;
                }

                // Ignore empty slots first
                if (!buffer.GetItemInSlot(slot))
                {
                    continue;
                }

                if (DoInsert(slot))
                {
                    return itemStack.Amount;
                }
            }

            for (int slot = 0; slot < buffer.GetNumSlots(); slot++)
            {
                if (!buffer.CanInsert(slot))
                {
                    continue;
                }

                // Ignore slots with items next
                if (buffer.GetItemInSlot(slot))
                {
                    continue;
                }

                if (DoInsert(slot))
                {
                    return itemStack.Amount;
                }
            }

            return itemStack.Amount;

            // Performs an insert to a slot and updates tracking variables
            bool DoInsert(int slot)
            {
                int remainder = buffer.InsertSlot(slot, itemStack, insertsRemaining);

                insertsRemaining -= itemStack.Amount - remainder;
                itemStack.Amount = remainder;

                return (insertsRemaining == 0 || itemStack.Amount == 0);    // Returns true if loop should be stopped
            }
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
            // Ignore if swapping the same itemStack
            if (buffer1 == buffer2 && slot1 == slot2)
            {
                return;
            }

            ItemStack itemStack1 = buffer1.GetItemInSlot(slot1);
            ItemStack itemStack2 = buffer2.GetItemInSlot(slot2);

            // Make sure both extracted item stacks can fully fit in the other buffer
            if (itemStack1.Amount > buffer2.MaxStackSize || itemStack2.Amount > buffer1.MaxStackSize)
            {
                return;
            }

            // Make sure the buffer accept their new itemstack
            if (!buffer1.AcceptsItemStack(slot1, itemStack2) || !buffer2.AcceptsItemStack(slot2, itemStack1))
            {
                return;
            }

            // Make sure both slots are compeletely empty after extracting (accounts for locked slots)
            buffer1.ExtractSlot(slot1, int.MaxValue);
            buffer2.ExtractSlot(slot2, int.MaxValue);

            if (!buffer1.GetItemInSlot(slot1) || !buffer2.GetItemInSlot(slot2))
            {
                // Restore extraction if failed
                buffer1.SetItemInSlot(slot1, itemStack1);
                buffer2.SetItemInSlot(slot2, itemStack2);
                return;
            }

            // Otherwise, do swap
            buffer1.SetItemInSlot(slot1, itemStack2);
            buffer2.SetItemInSlot(slot2, itemStack1);
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

        // TODO implement
        /// <summary>
        /// Moves as many items as possible from one buffer to another.
        /// </summary>
        /// <param name="sendingBuffer"></param>
        /// <param name="receivingBuffer"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static int TransferAvailableItems(ItemBuffer sendingBuffer, ItemBuffer receivingBuffer, int amount = int.MaxValue)
        {
            int totalAmountMoved = 0;

            for (int sourceSlot = 0; sourceSlot < sendingBuffer.GetNumSlots(); sourceSlot++)
            {
                ItemStack sendingStack = sendingBuffer.GetItemInSlot(sourceSlot);
                if (!sendingStack)
                {
                    continue;
                }

                int remainder = receivingBuffer.Insert(sendingStack, amount - totalAmountMoved);
                int amountInserted = sendingStack.Amount - remainder;

                Buffer.Extract(sourceSlot, amountInserted);
                totalAmountMoved += amountInserted;

                if (totalAmountMoved == amount)
                {
                    break;
                }
            }

            return totalAmountMoved;
        }
    }
}
