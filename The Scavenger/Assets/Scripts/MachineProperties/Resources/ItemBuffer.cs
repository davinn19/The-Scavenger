using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemBuffer : Buffer
    {
        [field: SerializeField] public int MaxCapacity { get; private set; }
        [field: SerializeField] public int NumSlots { get; private set; }

        public Func<ItemStack, int, bool> AcceptsItemStack;

        [field: SerializeField] public ItemStack[] Slots { get; private set; }
        private bool[] slotsLocked;

        private void Awake()
        {
            ProcessStartingItems();
            slotsLocked = new bool[NumSlots];

            if (AcceptsItemStack == null)
            {
                AcceptsItemStack = (itemStack, _) => { return ItemStackFits(itemStack); };
            }
        }

        // Resizes slots array to numSlots and confirms all itemStacks set in editor match maxCapacity
        private void ProcessStartingItems()
        {
            ItemStack[] resizedSlots = new ItemStack[NumSlots];

            for (int i = 0; i < Slots.Length; i++)
            {
                ItemStack itemStack = GetItemInSlot(i);
                itemStack.amount = Mathf.Min(itemStack.amount, MaxCapacity);
                resizedSlots[i] = itemStack;
            }

            for (int i = Slots.Length; i < resizedSlots.Length; i++)
            {
                resizedSlots[i] = new ItemStack();
            }

            Slots = resizedSlots;
        }

        public int FindFirst(Predicate<ItemStack> condition)
        {
            for (int i = 0; i < NumSlots; i++)
            {
                ItemStack itemStack = GetItemInSlot(i);

                if (itemStack && condition(itemStack))
                {
                    return i;
                }
            }

            return -1;
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
        }

        public void ToggleLocked(int slot)
        {
            SetLocked(slot, !IsLocked(slot));
        }

        private bool ItemStackFits(ItemStack itemStack)
        {
            return itemStack.amount <= MaxCapacity;
        }


        public int Extract(int slot, int amount)
        {
            ItemStack extractedStack = GetItemInSlot(slot);

            if (!extractedStack)
            {
                return 0;
            }

            int amountExtracted = Mathf.Min(extractedStack.amount, amount);

            extractedStack.amount -= amountExtracted;
            if (extractedStack.IsEmpty() && !IsLocked(slot))
            {
                extractedStack.Clear();
            }

            return amountExtracted;
        }


        // returns remainder
        public int Insert(int slot, ItemStack otherStack, int amount)
        {
            if (!otherStack)
            {
                return 0;
            }

            ItemStack insertedStack = GetItemInSlot(slot);
            if (!insertedStack)
            {
                insertedStack.SetItem(otherStack.Item);
                insertedStack.data = otherStack.data;
            }

            if (!insertedStack.IsStackable(otherStack))
            {
                return 0;
            }

            int amountInserted = Mathf.Min(amount, otherStack.amount, MaxCapacity - insertedStack.amount);
            insertedStack.amount += amountInserted;

            return otherStack.amount - amountInserted;
        }

        // returns items not taken
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
                    int amountToTake = Mathf.Min(otherStack.amount, totalAmountToTake);
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
                    otherStack.amount -= remainder;
                }

            }

            return amount - totalAmountToTake;
        }

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

    }
}
