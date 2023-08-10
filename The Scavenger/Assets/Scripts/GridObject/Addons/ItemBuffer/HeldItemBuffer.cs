using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO implement, add docs
namespace Scavenger
{
    public class HeldItemBuffer : ItemBuffer
    {
        private ItemStack heldItem = new ItemStack();

        private ItemDropper itemDropper;
        private InputHandler inputHandler;
        public event Action HeldItemChanged;

        public ItemStack GetHeldItem() => heldItem;

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
        public override List<int> GetInteractibleSlots(ItemInteractionType interactionType) => new List<int> { 1 };

        public override List<ItemStack> GetItems(ItemInteractionType interactionType = ItemInteractionType.All) => new List<ItemStack>() { heldItem };

        public void Swap(ItemBuffer other)// TODO implement
        {

        }

        public override void ReadPersistentData(JSON data)
        {
            if (data.ContainsKey("HeldItem"))
            {
                heldItem = data.GetJSON("HeldItem").Deserialize<ItemStack>();
            }
        }

        public override JSON WritePersistentData()
        {
            JSON data = new JSON();
            if (heldItem)
            {
                data.Add("HeldItem", heldItem);
            }
            return data;
        }
    }
}
