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

        public int RecycleProgress { get; private set; }

        [field: SerializeField, Min(0)]
        public int TicksPerRecycle { get; private set; }

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
                RecycleProgress = 0;
                return;
            }

            // Pauses if output is full (keep progress) or there is not enough energy
            if (energyBuffer.Energy < energyPerTick || itemBuffer.IsOutputFull())
            {
                return;
            }

            // Do recycle progress
            RecycleProgress++;
            energyBuffer.Spend(energyPerTick);
            if (RecycleProgress < TicksPerRecycle)
            {
                return;
            }

            RecyclerRecipe recipe = RecipeList.GetRecipeWithInput(input.Item);
            List<ItemStack> recipeDrops = RecipeList.GenerateDrops(recipe);

            List<ItemStack> outputSlots = itemBuffer.GetOutputSlots();

            ItemTransfer.MoveBufferToBuffer(recipeDrops, outputSlots, int.MaxValue, itemBuffer.AcceptsItemStack);

            RecycleProgress = 0;
            itemBuffer.ExtractSlot(0, 1);
        }

        // TODO add docs
        public override void ReadPersistentData(JSON data)
        {
            base.ReadPersistentData(data);
            if (data.ContainsKey("EnergyBuffer"))
            {
                energyBuffer.ReadPersistentData(data.GetJSON("EnergyBuffer"));
            }
            if (data.ContainsKey("ItemBuffer"))
            {
                itemBuffer.ReadPersistentData(data.GetJSON("ItemBuffer"));
            }
        }

        // TODO add docs
        public override JSON WritePersistentData()
        {
            JSON data = base.WritePersistentData();
            JSONHelper.TryAdd(data, "EnergyBuffer", energyBuffer.WritePersistentData());
            JSONHelper.TryAdd(data, "ItemBuffer", itemBuffer.WritePersistentData());
            return data;
        }
    }
}
