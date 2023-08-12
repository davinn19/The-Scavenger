using Leguar.TotalJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO redo, add docs
    [RequireComponent(typeof(RecyclerItemBuffer), typeof(EnergyBuffer))]
    public class Recycler : GridObjectBehavior
    {
        [field: SerializeField] public RecyclerRecipeList RecipeList { get; private set; }

        private RecyclerItemBuffer itemBuffer;
        private EnergyBuffer energyBuffer;

        [SerializeField, HideInInspector]
        private float recycleProgress = 0;

        [SerializeField, Min(0)]
        private int ticksPerRecycle;

        [SerializeField, Min(0)]
        private int energyPerTick;

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            itemBuffer = GetComponent<RecyclerItemBuffer>();
            energyBuffer = GetComponent<EnergyBuffer>();
        }

        // TODO implement
        protected override void TickUpdate()
        {
            energyBuffer.ResetEnergyTransferCount();

            ItemStack input = itemBuffer.GetInput();

            // Resets if input is missing/invalid
            if (!input || !RecipeList.IsRecyclable(input.Item))
            {
                recycleProgress = 0;
                return;
            }

            // Pauses if output is full (keep progress)
            if (itemBuffer.IsOutputFull())
            {
                return;
            }

            // Do recycle progress
            recycleProgress++;
            if (recycleProgress < ticksPerRecycle)
            {
                return;
            }

            RecyclerRecipe recipe = RecipeList.GetRecipeWithInput(input.Item);
            List<ItemStack> recipeDrops = RecipeList.GenerateDrops(recipe);

            List<ItemStack> outputSlots = itemBuffer.GetOutputSlots();

            ItemTransfer.MoveBufferToBuffer(recipeDrops, outputSlots, int.MaxValue, itemBuffer.AcceptsItemStack);

            recycleProgress = 0;
            itemBuffer.ExtractSlot(0, 1);
        }
    }
}
