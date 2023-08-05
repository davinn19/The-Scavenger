using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs, filter inputs and outputs using slots instead
    public class ItemInterface : ConduitInterface<ItemBuffer>
    {
        // returns amount of items moved
        public int MoveTo(int amount, ItemInterface destination)
        {
            int totalAmountMoved = 0;

            for (int sourceSlot = 0; sourceSlot < Buffer.NumSlots; sourceSlot++)
            {
                ItemStack sourceStack = Buffer.GetItemInSlot(sourceSlot);
                if (!sourceStack)
                {
                    continue;
                }

                int remainder = destination.Buffer.Insert(sourceStack, amount - totalAmountMoved);
                int amountInserted = sourceStack.Amount - remainder;

                Buffer.Extract(sourceSlot, amountInserted);
                totalAmountMoved += amountInserted;

                if (totalAmountMoved == amount)
                {
                    break;
                }
            }

            return totalAmountMoved;
        }
    }
}
