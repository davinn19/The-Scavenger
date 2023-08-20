using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class FurnaceRecipe : Recipe
    {
        public readonly RecipeComponent<ItemStack> input;
        public readonly ItemStack output;
        public readonly int requiredTemperature;

        public FurnaceRecipe(RecipeComponent<ItemStack> input, ItemStack output, int requiredTemperature)
        {
            this.input = input;
            this.output = output;
            this.requiredTemperature = requiredTemperature;
        }

        public FurnaceRecipe(Item input, Item output, int requiredTemperature)
        {
            this.input = new ItemStack(input, 1);
            this.output = new ItemStack(output, 1);
            this.requiredTemperature = requiredTemperature;
        }

        public bool IsInput(RecipeComponent component)
        {
            return input.CanSubstituteWith(component);
        }

        public bool IsOutput(RecipeComponent component)
        {
            return output.CanSubstituteWith(component);
        }

    }
}
