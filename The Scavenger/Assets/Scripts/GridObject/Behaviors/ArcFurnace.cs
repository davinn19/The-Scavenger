using Scavenger.Recipes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs
    [RequireComponent(typeof(EnergyBuffer), typeof(ArcFurnaceItemBuffer))]
    public class ArcFurnace : GridObjectBehavior
    {
        [SerializeField] private int temperature;
        [SerializeField] public int maxTemperature;
        [SerializeField] private int energyPerTick;
        [SerializeField] private int temperaturePerEnergy;

        private EnergyBuffer energyBuffer;
        private ArcFurnaceItemBuffer itemBuffer;

        private FurnaceRecipe currentRecipe;
        private ItemStack input;

        protected override void Init()
        {
            base.Init();
            energyBuffer = GetComponent<EnergyBuffer>();
            itemBuffer = GetComponent<ArcFurnaceItemBuffer>();

            input = itemBuffer.GetInput();
            input.Changed += UpdateCurrentRecipe;
            UpdateCurrentRecipe();
        }

        protected override void TickUpdate()
        {
            energyBuffer.ResetEnergyTransferCount();

            // Resets if input is missing/has no recipe
            if (currentRecipe == null)
            {
                temperature = 0;
                return;
            }

            // Pauses if output is full and cannot fit products
            ItemStack output = currentRecipe.output.Clone();
            ItemStack byproduct = currentRecipe.byproduct.Clone();
            bool outputMoved = output.Amount == ItemTransfer.MoveStackToStack(output, itemBuffer.GetOutput(), output.Amount, itemBuffer.AcceptsItemStack, true);
            bool byproductMoved = byproduct.Amount == ItemTransfer.MoveStackToStack(byproduct, itemBuffer.GetByproduct(), byproduct.Amount, itemBuffer.AcceptsItemStack, true);
            if (!outputMoved || !byproductMoved)
            {
                return;
            }

            // Pauses if not enough energy
            if (energyBuffer.Energy < energyPerTick)
            {
                return;
            }

            // Add temperature
            int availableEnergy = Mathf.Min(energyPerTick, energyBuffer.Energy);
            int remainingTemperature = maxTemperature - temperature;
            int energyUsed = Mathf.Min(availableEnergy, Mathf.CeilToInt(1f * remainingTemperature / temperaturePerEnergy));

            energyBuffer.Spend(energyUsed);
            temperature += energyUsed * temperaturePerEnergy;

            // Skip if temperature requirement is not met
            if (temperature < currentRecipe.requiredTemperature)
            {
                return;
            }

            // Add output/byproduct
            ItemTransfer.MoveStackToStack(output, itemBuffer.GetOutput(), output.Amount, itemBuffer.AcceptsItemStack, false);
            if (Random.value < currentRecipe.byproduct.GetChance())
            {
                ItemTransfer.MoveStackToStack(byproduct, itemBuffer.GetByproduct(), byproduct.Amount, itemBuffer.AcceptsItemStack, false);
            }

            // Spend input
            temperature -= currentRecipe.requiredTemperature * 2 / 3;
            itemBuffer.ExtractSlot(0, 1);
        }

        public int GetTemperature() => temperature;

        public FurnaceRecipe GetCurrentRecipe() => currentRecipe;

        private void UpdateCurrentRecipe()
        {
            FurnaceRecipe newRecipe = null;
            if (input)
            {
                newRecipe = FurnaceRecipes.Instance.GetRecipeWithInput(input);
            }

            if (newRecipe != currentRecipe)
            {
                currentRecipe = newRecipe;
                temperature = 0;
            }

        }
    }
}
