using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO implement, add docs
    public class AssemblerRecipe : Recipe
    {
        public readonly RecipeComponent<ItemStack> baseItem;
        public readonly RecipeComponent<ItemStack>[] addons;
        public readonly ItemStack output;

        public AssemblerRecipe(RecipeComponent<ItemStack> baseItem, RecipeComponent<ItemStack>[] addons, ItemStack output)
        {
            this.baseItem = baseItem;
            this.addons = addons;
            this.output = output;
        }

        public bool IsInput(RecipeComponent component)
        {
            if (baseItem.CanSubstituteWith(component))
            {
                return true;
            }

            foreach (RecipeComponent<ItemStack> addon in addons)
            {
                if (addon.CanSubstituteWith(component))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsOutput(RecipeComponent component)
        {
            return output.CanSubstituteWith(component);
        }

    }
}
