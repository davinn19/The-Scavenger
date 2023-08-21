using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [RequireComponent(typeof(EnergyBuffer), typeof(ArcFurnaceItemBuffer))]
    public class ArcFurnace : GridObjectBehavior
    {
        [SerializeField] private int temperature;
        [SerializeField] private int maxTemperature;
        [SerializeField] private int energyPerTick;
        [SerializeField] private int temperaturePerEnergy;

        private EnergyBuffer energyBuffer;
        private ArcFurnaceItemBuffer itemBuffer;

        protected override void Init()
        {
            base.Init();
            energyBuffer = GetComponent<EnergyBuffer>();
            itemBuffer = GetComponent<ArcFurnaceItemBuffer>();
        }

        protected override void TickUpdate()
        {
            energyBuffer.ResetEnergyTransferCount();

            // Increase temperature to max level
            int availableEnergy = Mathf.Min(energyBuffer.Energy, energyPerTick);
            int temperatureGained = Mathf.Min(maxTemperature - temperature, temperaturePerEnergy * availableEnergy);
            temperature += temperatureGained;
            energyBuffer.Spend(Mathf.CeilToInt(1f * temperatureGained / temperaturePerEnergy));

            // Skips if input is missing
            ItemStack input = itemBuffer.GetInput();
            if (!input)
            {
                return;
            }

            // Skips if input has no recipe
            FurnaceRecipe recipe = FurnaceRecipes.Instance.GetRecipeWithInput(input);
            if (recipe == null)
            {
                return;
            }

            // Skips if temperature is not high enough
            if (temperature < recipe.requiredTemperature)
            {
                return;
            }

            // Skips if output is full and cannot fit products
            ItemStack output = recipe.output.Clone();
            ItemStack byproduct = recipe.byproduct.Clone();
            bool outputMoved = output.Amount == ItemTransfer.MoveStackToStack(output, itemBuffer.GetOutput(), output.Amount, itemBuffer.AcceptsItemStack, true);
            bool byproductMoved = byproduct.Amount == ItemTransfer.MoveStackToStack(byproduct, itemBuffer.GetByproduct(), byproduct.Amount, itemBuffer.AcceptsItemStack, true);
            if (!outputMoved || !byproductMoved)
            {
                return;
            }

            // Move it for real
            ItemTransfer.MoveStackToStack(output, itemBuffer.GetOutput(), output.Amount, itemBuffer.AcceptsItemStack, true);
            if (Random.value < recipe.byproduct.GetChance())
            {
                ItemTransfer.MoveStackToStack(byproduct, itemBuffer.GetByproduct(), byproduct.Amount, itemBuffer.AcceptsItemStack, true);
            }
            
            itemBuffer.ExtractSlot(0, 1);
            temperature -= recipe.requiredTemperature * 2 / 3;
        }
    }
}
