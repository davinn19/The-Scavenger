using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "CraftingRecipes", menuName = "Scavenger/Recipes/Crafting")]
    public class CraftingRecipes : ScriptableObject
    {
        public List<CraftingRecipe> recipes;
    }


    [System.Serializable]
    public class CraftingRecipe
    {
        public List<ItemStack> ingredients;
        public ItemStack result;
    }

}
