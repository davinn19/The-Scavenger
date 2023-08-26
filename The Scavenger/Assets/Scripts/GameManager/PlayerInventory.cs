using Leguar.TotalJSON;
using Scavenger.GridObjectBehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs, implement
    public class PlayerInventory : ItemBuffer
    {
        public event Action HeldItemChanged;
        [SerializeField] ItemStack[] inventory;
        private ItemStack heldItem = new ItemStack();

        private const int numInventorySlots = 28;
        public override int NumSlots => numInventorySlots + 1;

        protected override void Init()
        {
            Array.Resize(ref inventory, numInventorySlots); // Resize inventory to proper size
            for (int slot = 0; slot < numInventorySlots; slot++)
            {
                if (inventory[slot] == null)    // Fill in any missing spaces
                {
                    inventory[slot] = new ItemStack();
                }
                else
                {
                    ItemStack itemStack = inventory[slot];
                    itemStack.SetAmount(itemStack.Amount);  // Make sure existing itemStacks are under max size
                }
                    
            }
            heldItem.Changed += HeldItemChanged;

            base.Init();
        }

        public ItemStack GetHeldItem() => heldItem;
        public List<ItemStack> GetInventory() => new(inventory);
        public override ItemStack GetItemInSlot(int slot)
        {
            if (slot < numInventorySlots)
            {
                return inventory[slot];
            }
            return heldItem;
        }

        public void UseHeldItem(int amount = 1)
        {
            ExtractSlot(numInventorySlots, amount);
        }

        public void MoveHeldItemToInventory()
        {
            ItemTransfer.MoveStackToBuffer(heldItem, GetInventory(), heldItem.Amount, AcceptsItemStack, false);
        }


        // Since these functions are primarily used by conduits, their implementation is irrelevant.
        public override bool AcceptsItemStack(ItemStack slot, ItemStack itemStack) => true;

        public override List<ItemStack> GetAllSlots() => new List<ItemStack>(inventory) { heldItem };
        public override List<ItemStack> GetInputSlots() => GetAllSlots();
        public override List<ItemStack> GetOutputSlots() => GetAllSlots();

        public override void ReadPersistentData(JSON data)
        {
            throw new System.NotImplementedException();
        }

        public override JSON WritePersistentData()
        {
            throw new System.NotImplementedException();
        }
    }
}
