using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class ArcFurnaceItemBuffer : ItemBuffer
    {
        private ItemStack input = new ItemStack();
        private ItemStack output = new ItemStack();
        private ItemStack byproduct = new ItemStack();

        public override int NumSlots => 3;

        public override bool AcceptsItemStack(int slot, ItemStack itemStack) => true;

        public ItemStack GetInput() => input;
        public ItemStack GetOutput() => output;
        public ItemStack GetByproduct() => byproduct;

        public override List<ItemStack> GetAllSlots() => new() { input, output, byproduct };

        public override List<ItemStack> GetInputSlots() => new() { input };

        public override ItemStack GetItemInSlot(int slot)
        {
            switch (slot)
            { 
                case 0:
                    return input;
                case 1:
                    return output;
                case 2:
                    return byproduct;
                default:
                    throw new System.IndexOutOfRangeException();
            }
        }

        public override List<ItemStack> GetOutputSlots() => new() { output, byproduct };

        public override void ReadPersistentData(JSON data) { }

        public override JSON WritePersistentData() => new();
    }
}
