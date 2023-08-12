using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Helper class for transporting items between buffers.
    /// </summary>
    public static class ItemTransfer    // TODO implement, add/fix docs
    {
        public static int ExtractItem(ItemStack itemStack, Predicate<ItemStack> predicate, int amount, bool simulate)
        {
            if (!itemStack || itemStack.IsEmpty() || !predicate(itemStack))
            {
                return 0;
            }

            int amountToExtract = Mathf.Min(itemStack.Amount, amount);

            if (!simulate)
            {
                itemStack.RemoveAmount(amountToExtract);
            }

            return amountToExtract;
        }

        // TODO add docs
        public static int ExtractFromBuffer(List<ItemStack> buffer, Predicate<ItemStack> predicate, int amount, bool simulate) 
        {
            int amountExtracted = 0;

            foreach (ItemStack itemStack in buffer)
            {
                amountExtracted += ExtractItem(itemStack, predicate, amount - amountExtracted, simulate);

                if (amountExtracted == amount)
                {
                    return amount;
                }
            }
            return amountExtracted;
        }

        // checks for ITEM equality, ignores persistent data
        public static bool BufferContainsItem(List<ItemStack> buffer, ItemStack comparison) => ExtractFromBuffer(buffer, (itemStack) => itemStack.Item == comparison.Item, 1, true) == 1;
        public static bool BufferContainsAnyItem(List<ItemStack> buffer, List<ItemStack> comparisons)
        {
            foreach (ItemStack comparison in comparisons)
            {
                if (BufferContainsItem(buffer, comparison))
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Swaps the itemStacks in different slots.
        /// </summary>
        /// <param name="itemStack1">One itemStack to swap.</param>
        /// <param name="itemStack2">The other itemStack to swap.</param>
        /// <param name="itemStack1Accepts">Checks if itemStack1 accepts itemStack2's insertion.</param>
        /// <param name="itemStack2Accepts">Checks if itemStack2 accepts itemStack1's insertion.</param>
        public static void Swap(ItemStack itemStack1, ItemStack itemStack2, Func<ItemStack, ItemStack, bool> itemStack1Accepts, Func<ItemStack, ItemStack, bool> itemStack2Accepts)
        {
            // Ignore if swapping the same itemStack
            if (itemStack1 == itemStack2)
            {
                return;
            }

            // Make sure both extracted item stacks can fully fit in the other buffer
            if (itemStack1.Amount > itemStack2.MaxAmount || itemStack2.Amount > itemStack1.MaxAmount)
            {
                return;
            }

            // Make sure the conditions are accepted
            if (!itemStack1Accepts(itemStack1, itemStack2) || !itemStack2Accepts(itemStack2, itemStack1))
            {
                return;
            }

            // Now do the swap
            ItemStack temp = itemStack1.Clone();
            itemStack1.Copy(itemStack2);
            itemStack2.Copy(temp);
        }

        // Modifies sending stack
        public static int MoveStackToStack(ItemStack sendingStack, ItemStack receivingStack, int amount, Func<ItemStack, ItemStack, bool> condition, bool simulate)
        {
            if (!sendingStack || sendingStack.IsEmpty())
            {
                return 0;
            }

            if (!receivingStack.IsStackable(sendingStack) || !condition(receivingStack, sendingStack))
            {
                return 0;
            }

            int amountMoved = Mathf.Min(amount, sendingStack.Amount, receivingStack.GetAmountBeforeFull());

            if (!simulate)
            {
                if (!receivingStack)
                {
                    receivingStack.Copy(sendingStack);
                }
                else
                {
                    receivingStack.AddAmount(amountMoved);
                }

                sendingStack.RemoveAmount(amountMoved);
            }

            return amountMoved;
        }
        
        // Modifies sending stack
        public static int MoveStackToBuffer(ItemStack sendingStack, List<ItemStack> receivingStacks, int amount, Func<ItemStack, ItemStack, bool> condition, bool simulate)
        {
            amount = Math.Min(amount, sendingStack.Amount);
            int amountMoved = 0;

            foreach (ItemStack receivingStack in receivingStacks)
            {
                if (amountMoved == amount)
                {
                    break;
                }

                if (!receivingStack.IsEmpty())
                {
                    amountMoved += MoveStackToStack(sendingStack, receivingStack, amount - amountMoved, condition, simulate);
                }
            }

            foreach (ItemStack receivingStack in receivingStacks)
            {
                if (amountMoved == amount)
                {
                    break;
                }

                if (receivingStack.IsEmpty())
                {
                    amountMoved += MoveStackToStack(sendingStack, receivingStack, amount - amountMoved, condition, simulate);
                }
            }

            return amountMoved;
        }

        // modifies sending stack
        public static int MoveBufferToBuffer(List<ItemStack> sendingStacks, List<ItemStack> receivingStacks, int amount, Func<ItemStack, ItemStack, bool> condition)
        {
            int totalAmountMoved = 0;

            foreach (ItemStack sendingStack in sendingStacks)
            {
                if (totalAmountMoved == amount)
                {
                    break;
                }

                int amountMoved = MoveStackToBuffer(sendingStack, receivingStacks, amount - totalAmountMoved, condition, false);
                totalAmountMoved += amountMoved;
            }

            return totalAmountMoved;
        }
    }
}
