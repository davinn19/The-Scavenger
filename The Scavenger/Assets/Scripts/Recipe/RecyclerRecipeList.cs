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
        public List<RecyclerRecipe> GetRecipesWithItem(Item item)
        {
            List<RecyclerRecipe> results = GetRecipesWithOutput(item);

            RecyclerRecipe inputRecipe = GetRecipeWithInput(item);
            if (inputRecipe)
            {
                results.Add(inputRecipe);
            }

            return results;
        }

        // TODO add docs
        public RecyclerRecipe GetRecipeWithInput(Item input)
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
        public List<RecyclerRecipe> GetRecipesWithOutput(Item output)
        {
            List<RecyclerRecipe> results = new List<RecyclerRecipe>();

            foreach (RecyclerRecipe recipe in recipes)
            {
                foreach (RecyclerDrop recyclerDrop in recipe.output)
                {
                    if (recyclerDrop.item == output)
                    {
                        results.Add(recipe);
                        break;
                    }
                }
            }

            return results;
        }

        // TODO add docs
        public bool IsRecyclable(Item input)
        {
            if (!input)
            {
                return false;
            }

            return GetRecipeWithInput(input) != null;
        }

        public List<ItemStack> GenerateDrops(RecyclerRecipe recipe)
        {
            List<ItemStack> drops = new();
            foreach (RecyclerDrop drop in recipe.output)
            {
                if (Random.value > drop.chance)
                {
                    continue;
                }

                int amount = Random.Range(1, drop.amount + 1);
                ItemStack dropStack = new ItemStack(drop.item, drop.amount);
                drops.Add(dropStack);
            }

            return drops;
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
