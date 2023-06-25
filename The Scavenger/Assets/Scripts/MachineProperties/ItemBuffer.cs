using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemBuffer : MonoBehaviour
    {
        // TODO implement
        [SerializeField]
        [Min(1)]
        private int slotCapacity;

        [SerializeField]
        [Min(1)]
        private int numSlots;


        public ItemStack FindFirst(Func<ItemStack, bool> condition)
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

        private ItemStack[] GetItems()
        {
            return GetComponentsInChildren<ItemStack>();
        }

    }
}
