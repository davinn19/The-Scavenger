using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemStack : MonoBehaviour
    {
        [field: SerializeField] public Item Item { get; private set; }  
        public int amount;
        public Dictionary<string, string> data;


        public void SetItem(Item newItem)
        {
            if (Item == null)
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

        /// <summary>
        /// Takes amount of items from another stack and adds to itself.
        /// </summary>
        /// <param name="other">The itemStack to take items from.</param>
        /// <param name="requestedAmount">The amount of items to take.</param>
        /// <param name="stackCapacity">The maximum amount of items this stack can contain.</param>
        /// <returns>The amount of items taken.</returns>
        public int TakeFromStack(ItemStack other, int requestedAmount = int.MaxValue, int stackCapacity = int.MaxValue)// TODO test
        {
            // TODO add test to ensure itemStacks are stackable
            int amountToTake = Mathf.Min(new int[] { requestedAmount, stackCapacity - amount, other.amount});

            other.amount -= amountToTake;
            amount += amountToTake;
            // TODO deal with empty stacks
            return amountToTake;
        }

        public void Remove(int requestedAmount = 1)
        {
            int amountToTake = Mathf.Min(new int[] { requestedAmount, amount });
            amount -= amountToTake;
        }

        public bool IsEmpty()
        {
            return amount <= 0;
        }

    }
}
