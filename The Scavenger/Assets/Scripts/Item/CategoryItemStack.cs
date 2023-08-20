using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class CategoryItemStack : CategoryRecipeComponent<ItemStack>
    {
        private string category;
        public string GetCategory() => category;

        private int amount;
        public int Amount => amount;

        public CategoryItemStack(string category, int amount)
        {
            Debug.Assert(amount > 0);
            this.category = category;

            this.amount = amount;
        }

        public bool CanSubstituteWith(RecipeComponent other)
        {
            ItemStack otherStack = other as ItemStack;
            if (otherStack != null)
            {
                return otherStack.Item.IsInCategory(category);
            }

            CategoryRecipeComponent<ItemStack> otherCategory = other as CategoryRecipeComponent<ItemStack>;
            if (otherCategory != null)
            {
                return otherCategory.GetCategory() == category;
            }

            return false;
        }

        public ItemStack GetRecipeComponent()
        {
            // TODO implement
            throw new System.NotImplementedException();
        }

    }
}
