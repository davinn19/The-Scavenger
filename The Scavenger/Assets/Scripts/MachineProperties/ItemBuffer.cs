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


        public int GetInsertableAmount(Item item, int amount)
        {
            int existingAmount = 0;

            if (!items.ContainsKey(item))
            {
                if (items.Count >= numSlots)
                {
                    return 0;
                }

                existingAmount = 0;
            }
        }

    }
}
