using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Helper class for transporting items between buffers
    /// </summary>
    public static class ItemTransfer    // TODO implement, add/fix docs
    {
        public static int ExtractItem(ItemStack itemStack, Predicate<ItemStack> predicate, int amount, bool simulate)
        {
            if (!itemStack || !predicate(itemStack) || !itemStack.IsEmpty())
            {
                return 0;
            }

            int amountToExtract = Mathf.Min(itemStack.Amount, amount);

            if (!simulate)
            {
                itemStack.Amount -= amountToExtract;
                if (itemStack.IsEmpty())
                {
                    itemStack.Clear();
                }
            }

            return amountToExtract;
        }

        // TODO add docs
        public static int ExtractFromBuffer(List<ItemStack> buffer, Predicate<ItemStack> predicate, int amount , bool simulate) 
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

        /// <summary>
        /// Inserts another itemStack into the first available slots in the buffer.
        /// </summary>
        /// <param name="buffer">The item buffer to insert into.</param>
        /// <param name="itemStack">The itemStack to insert into the buffer. WILL NOT BE MODIFIED.</param>
        /// <param name="amount">Limit to the amount of items to insert</param>
        /// <returns>The amount remaining in itemStack after insertion.</returns>

        //public static int InsertItem(ItemStack insertingStack, List<ItemStack> buffer, int amount = int.MaxValue) // TODO handle whitelist
        //{
        //    itemStack = itemStack.Clone();   // Create a copy itemStack to prevent modifying
        //    int insertsRemaining = amount;

        //    for (int slot = 0; slot < buffer.NumSlots; slot++)
        //    {
        //        // Ignore empty slots first
        //        if (!buffer.GetItemInSlot(slot))
        //        {
        //            continue;
        //        }

        //        if (DoInsert(slot))
        //        {
        //            return itemStack.Amount;
        //        }
        //    }

        //    for (int slot = 0; slot < buffer.NumSlots; slot++)
        //    {
        //        // Ignore slots with items next
        //        if (buffer.GetItemInSlot(slot))
        //        {
        //            continue;
        //        }

        //        if (DoInsert(slot))
        //        {
        //            return itemStack.Amount;
        //        }
        //    }

        //    return itemStack.Amount;

        //    // Performs an insert to a slot and updates tracking variables
        //    bool DoInsert(int slot)
        //    {
        //        int remainder = buffer.InsertSlot(slot, itemStack, insertsRemaining);

        //        insertsRemaining -= itemStack.Amount - remainder;
        //        itemStack.Amount = remainder;

        //        return (insertsRemaining == 0 || itemStack.Amount == 0);    // Returns true if loop should be stopped
        //    }
        //}

        /// <summary>
        /// Swaps itemStacks in the slots of the buffers.
        /// </summary>
        /// <param name="buffer1">Buffer containing one of the itemStacks.</param>
        /// <param name="slot1">Slot to get the first itemStack from.</param>
        /// <param name="buffer2">Buffer containing the other itemStack.</param>
        /// <param name="slot2">Slot to get the second itemStack from.</param>
        public static void Swap(ItemBuffer buffer1, int slot1, ItemBuffer buffer2, int slot2)
        {
            // Ignore if swapping the same itemStack
            if (buffer1 == buffer2 && slot1 == slot2)
            {
                return;
            }

            ItemStack itemStack1 = buffer1.GetItemInSlot(slot1).Clone();
            ItemStack itemStack2 = buffer2.GetItemInSlot(slot2).Clone();

            // Make sure both extracted item stacks can fully fit in the other buffer
            if (itemStack1.Amount > buffer2.MaxStackSize || itemStack2.Amount > buffer1.MaxStackSize)
            {
                return;
            }

            // Make sure the buffer accept their new itemstack
            if (!buffer1.AcceptsItemStack(slot1, itemStack2) || !buffer2.AcceptsItemStack(slot2, itemStack1))
            {
                return;
            }

            // Make sure both slots are compeletely empty after extracting (accounts for locked slots)
            buffer1.ExtractSlot(slot1, int.MaxValue);
            buffer2.ExtractSlot(slot2, int.MaxValue);

            if (!buffer1.GetItemInSlot(slot1) || !buffer2.GetItemInSlot(slot2))
            {
                // Restore extraction if failed
                buffer1.SetItemInSlot(slot1, itemStack1);
                buffer2.SetItemInSlot(slot2, itemStack2);
                return;
            }

            // Otherwise, do swap
            buffer1.SetItemInSlot(slot1, itemStack2);
            buffer2.SetItemInSlot(slot2, itemStack1);
        }

        // TODO implement, add docs, move to other file
        /// <summary>
        /// Moves as many items as possible from one buffer to another without overflowing.
        /// </summary>
        /// <param name="sendingBuffer"></param>
        /// <param name="receivingBuffer"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        //public static int Dump(ItemBuffer sendingBuffer, ItemBuffer receivingBuffer, int amount = int.MaxValue)
        //{
        //    int totalAmountMoved = 0;

        //    for (int sourceSlot = 0; sourceSlot < sendingBuffer.NumSlots; sourceSlot++)
        //    {
        //        ItemStack sendingStack = sendingBuffer.GetItemInSlot(sourceSlot);
        //        if (!sendingStack)
        //        {
        //            continue;
        //        }

        //        int remainder = receivingBuffer.InsertAvailableSlots(sendingStack, amount - totalAmountMoved);
        //        int amountInserted = sendingStack.Amount - remainder;

        //        Buffer.Extract(sourceSlot, amountInserted);
        //        totalAmountMoved += amountInserted;

        //        if (totalAmountMoved == amount)
        //        {
        //            break;
        //        }
        //    }

        //    return totalAmountMoved;

        //}
        
    }

    public enum ItemInteractionType
    {
        All,
        Insert,
        Extract
    }
}
