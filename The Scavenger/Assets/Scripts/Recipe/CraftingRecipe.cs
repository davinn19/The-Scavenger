using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class CraftingRecipe : Recipe
    {
        public readonly RecipeComponent<ItemStack>[] Inputs;
        public readonly ItemStack Output;
        public readonly bool MakeshiftCraftable;

        public CraftingRecipe(RecipeComponent<ItemStack>[] inputs, ItemStack output, bool makeshiftCraftable)
        {
            Inputs = inputs;
            Output = output;
            MakeshiftCraftable = makeshiftCraftable;
        }

        public bool IsInput(RecipeComponent component)
        {
            foreach (RecipeComponent<ItemStack> input in Inputs)
            {
                if (input.CanSubstituteWith(component))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOutput(RecipeComponent component)
        {
            return Output.CanSubstituteWith(component);  // TODO lesser standard???
        }
    }
}
