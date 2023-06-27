using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemBuffer : Buffer
    {
        [SerializeField] private int maxCapacity;
        [SerializeField] private List<ItemStack> items;
        [SerializeField] private int numSlots;

        public ItemStack FindFirst(Predicate<ItemStack> condition)
        {
            foreach (ItemStack item in items)
            {
                if (condition(item))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
