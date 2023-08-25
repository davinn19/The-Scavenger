using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO add docs
    public class FurnaceRecipe : Recipe
    {
        public readonly RecipeComponent<ItemStack> input;
        public readonly ItemStack output;
        public readonly ChanceItemStack byproduct;
        public readonly int requiredTemperature;

        public FurnaceRecipe(RecipeComponent<ItemStack> input, ItemStack output, ChanceItemStack byproduct, int requiredTemperature)
        {
            this.input = input;
            this.output = output;
            this.byproduct = byproduct;
            this.requiredTemperature = requiredTemperature;
        }

        public bool IsInput(RecipeComponent component)
        {
            return input.CanSubstituteWith(component);
        }

        public bool IsOutput(RecipeComponent component)
        {
            bool isOutput = output.CanSubstituteWith(component);

            if (byproduct != null)
            {
                isOutput = isOutput || byproduct.CanSubstituteWith(component);
            }
            return isOutput;
        }

    }
}
