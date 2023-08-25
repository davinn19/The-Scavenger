using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO add docs
namespace Scavenger.Recipes
{
    public class RecyclerRecipe : Recipe
    {
        public readonly RecipeComponent<ItemStack> Input;
        public readonly ChanceItemStack[] Outputs;
        public readonly int Duration;
        public readonly RecycleTier RecycleTier;

        public RecyclerRecipe(RecipeComponent<ItemStack> input, RecycleTier recycleTier, int duration, ChanceItemStack[] outputs)
        {
            Input = input;
            Outputs = outputs;
            Duration = duration;
            RecycleTier = recycleTier;
        }

        public bool IsInput(RecipeComponent component)
        {
            return Input.CanSubstituteWith(component);
        }

        public bool IsOutput(RecipeComponent component)
        {
            foreach (ChanceItemStack output in Outputs)
            {
                if (output.CanSubstituteWith(component))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
