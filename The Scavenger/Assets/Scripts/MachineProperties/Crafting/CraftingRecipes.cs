using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "CraftingRecipes", menuName = "Scavenger/Recipes/Crafting")]
    public class CraftingRecipes : ScriptableObject
    {
        [SerializeField] private List<CraftingRecipe> recipes;

        public ReadOnlyCollection<CraftingRecipe> GetRecipes()
        {
            return new ReadOnlyCollection<CraftingRecipe>(recipes);
        }
    }


    [System.Serializable]
    public class CraftingRecipe
    {
        public List<ItemStack> Ingredients;
        public ItemStack Result;

        public static implicit operator bool(CraftingRecipe recipe)
        {
            return recipe != null;
        }
    }

}
