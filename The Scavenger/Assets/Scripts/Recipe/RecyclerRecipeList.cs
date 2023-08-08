using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class RecyclerRecipeList : PrefabDatabase<RecyclerRecipe>
    {
        [SerializeField] private RecyclerRecipe[] recipes;

        // TODO add docs
        public RecyclerRecipe GetRecipe(Item input)
        {
            foreach (RecyclerRecipe recipe in recipes)
            {
                if (recipe.input == input)
                {
                    return recipe;
                }
            }

            return null;
        }

        // TODO add docs
        public bool IsRecyclable(ItemStack itemStack)
        {
            return GetRecipe(itemStack.Item) != null;
        }

        public override void UpdateDatabase()
        {
            List<RecyclerRecipe> newRecipes = new();

            List<RecyclerRecipe> foundRecipes = FindAssets();
            foreach (RecyclerRecipe recipe in foundRecipes)
            {
                string[] assetPath = AssetDatabase.GetAssetPath(recipe).Split('/');
                string tier = assetPath[assetPath.Length - 2];

                if (tier == name)
                {
                    newRecipes.Add(recipe);
                }
            }

            recipes = newRecipes.ToArray();
        }
    }
}
