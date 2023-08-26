using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO add docs
    public class RollingRecipes : RecipeList<RollingRecipe>
    {
        private static RollingRecipes instance = new RollingRecipes();
        public static RollingRecipes Instance => instance;
        private readonly List<RollingRecipe> recipes = new();


        public override List<RollingRecipe> GetRecipesWithOutput(RecipeComponent output)
        {
            if (output is not RecipeComponent<ItemStack>)
            {
                return new();
            }
            return base.GetRecipesWithOutput(output);
        }

        public override List<RollingRecipe> GetRecipesWithInput(RecipeComponent input)
        {
            if (input is not RecipeComponent<ItemStack>)
            {
                return new();
            }
            return base.GetRecipesWithInput(input);
        }

        public RollingRecipe GetRecipeWithInput(RecipeComponent<ItemStack> input, ItemStack rollingPass)
        {
            foreach (RollingRecipe recipe in GetRecipesWithInput(input))
            {
                if (recipe.rollingPass.CanSubstituteWith(rollingPass))
                {
                    return recipe;
                }
            }
            return null;
        }

        public override List<RollingRecipe> GetRecipes() => recipes;
        public override List<Recipe> GetGenericRecipes() => recipes.ConvertAll<Recipe>((recipe) => recipe);


        private void AddRecipe(RecipeComponent<ItemStack> input, ItemStack output, ItemStack rollingPass)
        {
            Debug.Assert(input.Amount == 1);
            Debug.Assert(output.Amount == 1);
            Debug.Assert(rollingPass.Amount == 1);
            Debug.Assert(rollingPass.Item.IsInCategory("RollingPass"));

            // One input cannot have multiple recipes
            Debug.Assert(GetRecipeWithInput(input, rollingPass) == null);

            recipes.Add(new RollingRecipe(input, output, rollingPass));
        }

        public override void LoadRecipes()
        {
            string[] metals = new string[]
            {
                "Aluminum",
                "Copper",
                "Steel",
                "Titanium",
            };

            foreach (string metal in metals)
            {
                AddRecipe(
                    new ItemStack(metal + "Bloom"),
                    new ItemStack(metal + "Slab"),
                    new ItemStack("FlatRollingPass")
                    );

                AddRecipe(
                    new ItemStack(metal + "Slab"),
                    new ItemStack(metal + "Plate"),
                    new ItemStack("FlatRollingPass")
                    );

                AddRecipe(
                    new ItemStack(metal + "Bloom"),
                    new ItemStack(metal + "Billet"),
                    new ItemStack("SquareRollingPass")
                    );

                AddRecipe(
                    new ItemStack(metal + "Billet"),
                    new ItemStack(metal + "Rod"),
                    new ItemStack("RoundRollingPass")
                    );

                AddRecipe(
                    new ItemStack(metal + "Billet"),
                    new ItemStack(metal + "Beam"),
                    new ItemStack("BeamRollingPass")
                    );

                AddRecipe(
                    new ItemStack(metal + "Rod"),
                    new ItemStack(metal + "Wire"),
                    new ItemStack("RoundRollingPass")
                    );
            }

            AddRecipe(
                new ItemStack("SiliconShard"),
                new ItemStack("SiliconBlock"),
                new ItemStack("FlatRollingPass")
                );

            AddRecipe(
               new ItemStack("SiliconBlock"),
               new ItemStack("PolySiliconWafer"),
               new ItemStack("FlatRollingPass")
               );
        }

        
    }
}
