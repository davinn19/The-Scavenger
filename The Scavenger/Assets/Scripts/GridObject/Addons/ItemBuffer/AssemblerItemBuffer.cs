using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, implement
    public class AssemblerItemBuffer : ItemBuffer
    {
        private ItemStack baseItem = new ItemStack();
        private ItemStack[] addons = new ItemStack[]
        {
            new ItemStack(),
            new ItemStack(),
            new ItemStack(),
            new ItemStack(),
        };

        private ItemStack output = new ItemStack();

        private ItemStack[] filter = new ItemStack[5];

        public override int NumSlots => 6;

        protected override void Init()
        {
            foreach (ItemStack addon in addons)
            {
                addon.SetMaxAmount(1);
            }

            for (int i = 0; i < 5; i++)
            {
                filter[i] = new ItemStack();
            }
            base.Init();
        }

        // Inserted item must match filter
        public override bool AcceptsItemStack(ItemStack slot, ItemStack itemStack)
        {
            if (slot == output)
            {
                return true;
            }
            return filter[GetItemStackSlot(slot)].IsStackable(itemStack);
        }

        public override List<ItemStack> GetAllSlots()
        {
            List<ItemStack> slots = GetInputSlots();
            slots.Add(output);
            return slots;
        }

        // Base item + addons
        public override List<ItemStack> GetInputSlots()
        {
            List<ItemStack> slots = new() { baseItem };
            slots.AddRange(addons);
            return slots;
        }

        // Just the output
        public override List<ItemStack> GetOutputSlots()
        {
            return new() { output };
        }

        public ItemStack GetBase() => baseItem;
        public ItemStack[] GetAddons() => addons;
        public ItemStack GetOutput() => output;

        public void SpendInputs()
        {
            for (int i = 0; i < 5; i++)
            {
                ExtractSlot(i, 1);
            }
        }


        public override ItemStack GetItemInSlot(int slot)
        {
            if (slot == 0)
            {
                return baseItem;
            }

            if (slot == NumSlots - 1)
            {
                return output;
            }

            return addons[slot - 1];
        }

        

        public override void ReadPersistentData(JSON data) { }

        public override JSON WritePersistentData() => new();
    }
}
