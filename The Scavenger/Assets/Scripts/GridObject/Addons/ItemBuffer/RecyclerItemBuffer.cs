using Leguar.TotalJSON;
using Scavenger.GridObjectBehaviors;
using System.Collections.Generic;
using UnityEngine;
using static Scavenger.ItemTransfer;

namespace Scavenger
{
    // TODO add docs, implement
    public class RecyclerItemBuffer : ItemBuffer
    {
        private ItemStack input = new ItemStack();
        private ItemStack[] output;

        private const int outputSlots = 15;

        protected override void Init()
        {
            base.Init();
            output = new ItemStack[outputSlots];
            for (int slot = 0; slot < outputSlots; slot++)
            {
                output[slot] = new ItemStack();
            }
        }

        public override List<int> GetInteractibleSlots(ItemInteractionType interactionType)
        {
            if (interactionType == ItemInteractionType.Insert)
            {
                return new List<int> { 0 };
            }
            else
            {
                List<int> slots = new();
                for (int slot = 1; slot <= outputSlots; slot++)
                {
                    slots.Add(slot);
                }
                return slots;

            }
        }

        public override List<ItemStack> GetItems(ItemInteractionType interactionType = ItemInteractionType.All)
        {
            List<ItemStack> items = new List<ItemStack>() { input };
            if (interactionType == ItemInteractionType.Insert)
            {
                return items;
            }
            else if (interactionType == ItemInteractionType.Extract)
            {
                return new List<ItemStack>(output);
            }
            else
            {
                items.AddRange(output);
                return items;
            }
        }

        public override bool AcceptsItemStack(int slot, ItemStack itemStack)
        {
            if (slot != 0)
            {
                return true;
            }
            return GetComponent<Recycler>().RecipeList.IsRecyclable(itemStack.Item);
        }

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

        public override JSON WritePersistentData() => new JSON();
    }
}
