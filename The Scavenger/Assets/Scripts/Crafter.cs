using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(GridObject))]
    public class Crafter : MonoBehaviour
    {
        [SerializeField] private CraftingRecipes recipes;

        // TODO add docs

        public List<CraftingRecipe> GetRecipes(string search, FilterMode filterMode)
        {
            List<CraftingRecipe> results = new();
            
            foreach (CraftingRecipe recipe in recipes.recipes)
            {
                if (!recipe.result.Item.DisplayName.Contains(search))
                {
                    continue;
                }

                // TODO implement
            }

            return results;
        }

        public enum FilterMode
        { 
            All,
            HaveIngredient,
            Craftable
        }


    }
}
