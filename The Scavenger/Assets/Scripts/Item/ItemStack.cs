using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [System.Serializable]
    public class ItemStack
    {
        [field: SerializeField] public Item Item { get; private set; }
        [Min(0)] public int amount;
        public Dictionary<string, string> data;


        public ItemStack(Item item, int amount, Dictionary<string, string> data = null)
        {
            SetItem(item);
            this.amount = amount;

            if (data != null)
            {
                this.data = data;
            }
            else
            {
                this.data = new Dictionary<string, string>();
            }
        }

        public ItemStack() => Clear();

        public void Clear()
        {
            Item = null;
            amount = 0;
            data = new Dictionary<string, string>();
        }


        public void SetItem(Item newItem)
        {
            if (Item != null)
            {
                Debug.LogWarning("ItemStack is already assigned an item");
                return;
            }

            Item = newItem;
        }

        public bool IsStackable(ItemStack other)
        {
            if (Item != other.Item)
            {
                return false;
            }

            return data.Equals(other.data);
        }

        public bool IsEmpty()
        {
            return amount <= 0;
        }

        public static implicit operator bool(ItemStack itemStack)
        {
            return itemStack != null && itemStack.Item != null;
        }
    }
}
