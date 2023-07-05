using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemStack : MonoBehaviour
    {
        // TODO make private/protected
        public Item item;       
        public int amount;
        public Dictionary<string, string> data;

        public bool IsStackable(ItemStack other)
        {
            if (item != other.item)
            {
                return false;
            }

            return data.Equals(other.data);
        }

        // Takes amount from other stack and adds to itself
        public void TakeFromStack(ItemStack other, int requestedAmount, int limit)
        {
            int amountToTake = Mathf.Min(new int[] { requestedAmount, limit, other.amount});

            other.amount -= amountToTake;
            amount += amountToTake;

            if (other.amount <= 0)
            {
                Destroy(other.gameObject);
            }
        }

        public void Remove(int requestedAmount)
        {
            int amountToTake = Mathf.Min(new int[] { requestedAmount, amount });
            amount -= amountToTake;
        }

    }
}
