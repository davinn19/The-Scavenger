using Leguar.TotalJSON;
using System.Collections.Generic;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, implement
    public class RecyclerItemBuffer : ItemBuffer
    {
        private ItemStack input = new ItemStack();
        private ItemStack[] output;
        private const int outputSlots = 15;
        public override int NumSlots => outputSlots + 1;

        protected override void Init()
        {
            output = new ItemStack[outputSlots];
            for (int slot = 0; slot < outputSlots; slot++)
            {
                output[slot] = new ItemStack();
            }

            base.Init();
        }

        public override ItemStack GetItemInSlot(int slot)
        {
            if (slot == 0)
            {
                return input;
            }
            return output[slot - 1];
        }

        public override List<ItemStack> GetAllSlots()
        {
            List<ItemStack> items = new List<ItemStack> { input };
            items.AddRange(output);
            return items;
        }
        public override List<ItemStack> GetInputSlots() => new List<ItemStack>() { input };
        public override List<ItemStack> GetOutputSlots() => new List<ItemStack>(output);  // TODO see if you can get away with not creating a copy

        /// <remarks>
        /// Recycler buffers allow any item to be stored, but it's impossible to insert to an output, and the recycler only runs when the input is valid.
        /// </remarks>
        /// <returns>Always returns true.</returns>
        public override bool AcceptsItemStack(ItemStack slot, ItemStack itemStack) => true;

        public ItemStack GetInput()
        {
            return input;
        }

        /// <summary>
        /// Checks if the recycler's output slots are all full.
        /// </summary>
        /// <returns></returns>
        public bool IsOutputFull()
        {
            for (int slot = 1; slot <= outputSlots; slot++)
            {
                if (!IsSlotFull(slot))
                {
                    return false;
                }
            }
            return true;
        }

        // TODO include inventory in drops/loot table
        public override void ReadPersistentData(JSON data) { }

        public override JSON WritePersistentData() => new();
    }
}
