using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO implement, add docs
namespace Scavenger
{
    public class HeldItemBuffer : ItemBuffer
    {
        private ItemStack[] item = new ItemStack[1];

        public ItemStack GetHeldItem() => item[0];

        /// <summary>
        /// Decrements held itemStack.
        /// </summary>
        /// <param name="amount">Amount of items to use.</param>
        /// <returns>Amount of items actually used.</returns>
        public int Use(int amount = 1)
        {
            Debug.Assert(!GetItemInSlot(0).IsEmpty());
            return ExtractSlot(0, amount);
        }

        public override bool AcceptsItemStack(int slot, ItemStack itemStack) => true;
        public override bool CanExtract(int slot) => false;
        public override bool CanInsert(int slot) => false;
        public override void Clear() => item = new ItemStack[1];
        public override ItemStack[] GetItems() => item;
        public override void SetItems(ItemStack[] items) => item = items;
    }
}
