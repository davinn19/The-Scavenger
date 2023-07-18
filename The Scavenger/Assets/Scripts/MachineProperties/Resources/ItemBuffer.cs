using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemBuffer : Buffer
    {
        [SerializeField] private int maxCapacity;
        [SerializeField] private int numSlots;

        [field: SerializeField] public ItemStack[] Slots { get; private set; }
        private bool[] slotsLocked;

        private void Awake()
        {
            ProcessStartingItems();
            slotsLocked = new bool[numSlots];
        }

        // Resizes slots array to numSlots and confirms all itemStacks set in editor match maxCapacity
        private void ProcessStartingItems()
        {
            ItemStack[] resizedSlots = new ItemStack[numSlots];

            for (int i = 0; i < Slots.Length; i++)
            {
                ItemStack itemStack = GetItemInSlot(i);
                itemStack.amount = Mathf.Min(itemStack.amount, maxCapacity);
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
            for (int i = 0; i < numSlots; i++)
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

            for (int i = 0; i < numSlots; i++)
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

        public List<ItemStack> GetItems()
        {
            List<ItemStack> itemStacks = new();
            foreach (ItemStack itemStack in Slots)
            {
                if (itemStack)
                {
                    itemStacks.Add(itemStack);
                }
            }
            return itemStacks;
        }

        public bool IsLocked(int slot)
        {
            return slotsLocked[slot];
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

            int amountInserted = Mathf.Min(amount, otherStack.amount, maxCapacity - insertedStack.amount);
            insertedStack.amount += amountInserted;

            return otherStack.amount - amountInserted;
        }

        // returns items not taken
        public int TakeFromBuffer(ItemBuffer otherBuffer, int amount)
        {
            int totalAmountToTake = amount;

            for (int otherSlot = 0; otherSlot < otherBuffer.numSlots; otherSlot++)
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

                for (int curSlot = 0; curSlot < numSlots; curSlot++)
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


        private int GetRemainingSlots()
        {
            // TODO fix
            return 0;
        }

    }
}
