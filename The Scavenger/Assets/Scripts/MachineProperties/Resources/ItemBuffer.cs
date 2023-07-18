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

        private ItemStack[] GetItems()
        {
            return GetComponentsInChildren<ItemStack>();
        }

        public ItemStack FindFirst(Predicate<ItemStack> condition)
        {
            foreach (ItemStack item in GetItems())
            {
                if (condition(item))
                {
                    return item;
                }
            }
            return null;
        }

        public List<ItemStack> FindAll(Predicate<ItemStack> condition)
        {
            List<ItemStack> items = new();
            foreach (ItemStack item in GetItems())
            {
                if (condition(item))
                {
                    items.Add(item);
                }
            }
            return items;
        }

        
        public int TakeFromBuffer(ItemBuffer other, int amount)
        {
            int itemsToTake = amount;

            foreach (ItemStack itemStack in other.GetItems())
            {
                List<ItemStack> compatibleStacks = FindAll(itemStack.IsStackable);
                
                // Insert into existing stacks first
                foreach (ItemStack compatibleStack in compatibleStacks)
                {
                    itemsToTake -= compatibleStack.TakeFromStack(itemStack, itemsToTake, maxCapacity);
                    
                    if (itemsToTake <= 0)
                    {
                        return amount;
                    }
                }

                int remainingSlots = GetRemainingSlots();

                // Create a new stack if there is anything left over
                while (!itemStack.IsEmpty() && itemsToTake > 0 && remainingSlots > 0)
                {
                    ItemStack newStack = CreateItemStack(itemStack.Item);
                    remainingSlots--;

                    itemsToTake -= newStack.TakeFromStack(itemStack, stackCapacity: maxCapacity);
                }
            }
            // TODO handle empty stacks
            return amount - itemsToTake;
        }


        private int GetRemainingSlots()
        {
            return numSlots - GetItems().Length; // TODO decide between transform.childCount / GetItems().Length
        }


        private ItemStack CreateItemStack(Item item)
        {
            ItemStack newStack = new GameObject(item.name).AddComponent<ItemStack>();

            newStack.SetItem(item);
            newStack.transform.parent = transform;

            return newStack;
        }

    }
}
