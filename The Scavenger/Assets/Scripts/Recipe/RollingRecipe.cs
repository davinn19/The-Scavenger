using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO add docs
    public class RollingRecipe : Recipe
    {
        public readonly RecipeComponent<ItemStack> input;
        public readonly ItemStack output;
        public readonly ItemStack rollingPass;

        public RollingRecipe(RecipeComponent<ItemStack> input, ItemStack output, ItemStack rollingPass)
        {
            this.input = input;
            this.output = output;
            this.rollingPass = rollingPass;
        }

        public bool IsInput(RecipeComponent component) => input.CanSubstituteWith(component) || rollingPass.CanSubstituteWith(component);

        public bool IsOutput(RecipeComponent component) => output.CanSubstituteWith(component);

    }
}
