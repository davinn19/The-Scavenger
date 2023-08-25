using Leguar.TotalJSON;
using Scavenger.Recipes;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO redo, add docs
    [RequireComponent(typeof(RecyclerItemBuffer), typeof(EnergyBuffer))]
    public class Recycler : GridObjectBehavior
    {
        [SerializeField] private RecycleTier recycleTier;

        private RecyclerItemBuffer itemBuffer;
        private EnergyBuffer energyBuffer;

        private RecyclerRecipe currentRecipe;
        private ItemStack input;

        public int RecycleProgress { get; private set; }

        [field: SerializeField, Min(0)]
        public int TicksPerRecycle { get; private set; }

        [SerializeField, Min(0)]
        private int energyPerTick;

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            energyBuffer = GetComponent<EnergyBuffer>();
            itemBuffer = GetComponent<RecyclerItemBuffer>();

            input = itemBuffer.GetInput();
            input.Changed += UpdateCurrentRecipe;
        }

        // TODO implement
        protected override void TickUpdate()
        {
            energyBuffer.ResetEnergyTransferCount();

            // Resets if input is missing/no recipe
            if (currentRecipe == null)
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
            
            List<ItemStack> outputs = GenerateOutputs(currentRecipe);
            List<ItemStack> outputSlots = itemBuffer.GetOutputSlots();
            ItemTransfer.MoveBufferToBuffer(outputs, outputSlots, int.MaxValue, itemBuffer.AcceptsItemStack);

            itemBuffer.ExtractSlot(0, 1);
            RecycleProgress = 0;
        }

        private void UpdateCurrentRecipe()
        {
            RecyclerRecipe newRecipe = null;

            if (input)
            {
                newRecipe = RecyclerRecipes.Instance.GetRecipeWithInput(input, recycleTier);
            }

            if (newRecipe != currentRecipe)
            {
                currentRecipe = newRecipe;
                RecycleProgress = 0;
            }
        }

        private List<ItemStack> GenerateOutputs(RecyclerRecipe recipe)
        {
            List<ItemStack> outputs = new();
            foreach (ChanceItemStack chanceItemStack in recipe.Outputs)
            {
                if (Random.Range(0, 1f) < chanceItemStack.GetChance())
                {
                    outputs.Add(chanceItemStack.GetRecipeComponent());
                }
            }
            return outputs;
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
