using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
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

        // Resizes slots array to numSlots and confirms all itemStacks set in editor match maxCapacity
        private void ProcessStartingItems()
        {
            ItemStack[] resizedSlots = new ItemStack[NumSlots];

            for (int i = 0; i < Slots.Length; i++)
            {
                ItemStack itemStack = GetItemInSlot(i);
                itemStack.Amount = Mathf.Min(itemStack.Amount, MaxCapacity);
                resizedSlots[i] = itemStack;
            }

            for (int i = Slots.Length; i < resizedSlots.Length; i++)
            {
                resizedSlots[i] = new ItemStack();
            }

            Slots = resizedSlots;
        }

        public List<int> FindAll(Predicate<ItemStack> condition)
        {
            List<int> foundSlots = new();

            for (int i = 0; i < NumSlots; i++)
            {
                ItemStack itemStack = GetItemInSlot(i);

                if (itemStack && condition(itemStack))
                {
                    foundSlots.Add(i);
                }
            }

            return foundSlots;
        }


        public ItemStack GetItemInSlot(int slot)
        {
            return Slots[slot];
        }

        private void SetItemInSlot(ItemStack itemStack, int slot)
        {
            Slots[slot] = itemStack;
            SlotChanged?.Invoke(slot);
        }

        public bool IsLocked(int slot)
        {
            return slotsLocked[slot];
        }

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

        public void ToggleLocked(int slot)
        {
            SetLocked(slot, !IsLocked(slot));
        }

        /// <summary>
        /// Extracts from the itemStack in a specific slot.
        /// </summary>
        /// <param name="slot">The slot to extract from.</param>
        /// <param name="amount">The amount to attempt extracting.</param>
        /// <returns>The amount extracted.</returns>
        public int Extract(int slot, int amount)
        {
            ItemStack extractedStack = GetItemInSlot(slot);

            // Ignore if extracted stack is empty
            if (!extractedStack)
            {
                return 0;
            }

            int amountExtracted = Mathf.Min(extractedStack.Amount, amount);

            extractedStack.Amount -= amountExtracted;
            if (extractedStack.IsEmpty() && !IsLocked(slot))
            {
                extractedStack.Clear();
            }

            SlotChanged?.Invoke(slot);
            
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
            if (!otherStack)
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
                    insertedStack.data = new Dictionary<string, string>(otherStack.data);
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
        /// <param name="simulate">If true, will only calculate the results, not modifying the buffer.</param>
        /// <returns>The amount remaining in itemStack after insertion.</returns>
        public int Insert(ItemStack itemStack, bool simulate)
        {
            // Create copy stack to prevent modifying parameter
            itemStack = new ItemStack(itemStack);

            for (int slot = 0; slot < NumSlots; slot++)
            {
                int remainder = Insert(slot, itemStack, itemStack.Amount, simulate);
                if (remainder == 0)
                {
                    return 0;
                }

                itemStack.Amount = remainder; 
            }

            return itemStack.Amount;
        }

        // returns items not taken, TODO use new insert function, remake
        public int TakeFromBuffer(ItemBuffer otherBuffer, int amount)
        {
            int totalAmountToTake = amount;

            for (int otherSlot = 0; otherSlot < otherBuffer.NumSlots; otherSlot++)
            {
                ItemStack otherStack = otherBuffer.GetItemInSlot(otherSlot);
                if (!otherStack)
                {
                    continue;
                }

                List<int> stackableSlots = FindAll(otherStack.IsStackable);

                // Insert into existing stacks first
                foreach (int stackableSlot in stackableSlots)
                {
                    int amountToTake = Mathf.Min(otherStack.Amount, totalAmountToTake);
                    int amountInserted = Insert(stackableSlot, otherStack, totalAmountToTake);
                    otherBuffer.Extract(otherSlot, amountInserted);


                    totalAmountToTake -= amountInserted;
                    if (totalAmountToTake <= 0)
                    {
                        return amount;
                    }
                }

                for (int curSlot = 0; curSlot < NumSlots; curSlot++)
                {
                    if (totalAmountToTake <= 0)
                    {
                        return amount;
                    }

                    if (otherStack.IsEmpty())
                    {
                        break;
                    }

                    int remainder = Insert(curSlot, otherStack, totalAmountToTake);
                    otherStack.Amount -= remainder;
                }
            }

            return amount - totalAmountToTake;
        }

        // TODO implement merging stackable stacks in a different function
        public static void Swap(ItemBuffer buffer1, int slot1, ItemBuffer buffer2, int slot2)
        {
            if (buffer1.IsLocked(slot1) || buffer2.IsLocked(slot2))
            {
                return;
            }

            ItemStack itemStack1 = buffer1.GetItemInSlot(slot1);
            ItemStack itemStack2 = buffer2.GetItemInSlot(slot2);

            if (itemStack1 == itemStack2 || !buffer1.AcceptsItemStack(itemStack2, slot1) || !buffer2.AcceptsItemStack(itemStack1, slot2))
            {
                return;
            }

            buffer1.SetItemInSlot(itemStack2, slot1);
            buffer2.SetItemInSlot(itemStack1, slot2);
        }

        public static void MoveItems(ItemBuffer sendingBuffer, int sendingSlot, ItemBuffer receivingBuffer, int receivingSlot, int amount = int.MaxValue)
        {
            ItemStack movedStack = sendingBuffer.GetItemInSlot(sendingSlot);
            int remainder = receivingBuffer.Insert(receivingSlot, movedStack, amount);
            sendingBuffer.Extract(sendingSlot, movedStack.Amount - remainder);
        }

    }
}
