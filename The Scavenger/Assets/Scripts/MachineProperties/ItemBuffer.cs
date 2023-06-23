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

        private Dictionary<Item, int> items = new Dictionary<Item, int>();


        public Item FindFirst(Func<Item, bool> condition)
        {
            foreach (Item item in items.Keys)
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
