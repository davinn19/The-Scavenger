using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class RecyclerRecipes : RecipeList<RecyclerRecipe>
    {
        private static RecyclerRecipes instance = new RecyclerRecipes();
        public static RecyclerRecipes Instance => instance;

        private readonly List<RecyclerRecipe> recipes = new();


        public List<RecyclerRecipe> GetRecipesWithInput(RecipeComponent input)
        {
            List<RecyclerRecipe> recipesWithInput = new();

            if (input is not RecipeComponent<ItemStack>)
            {
                return recipesWithInput;
            }

            foreach (RecyclerRecipe recipe in recipes)
            {
                if (recipe.IsInput(input))
                {
                    recipesWithInput.Add(recipe);
                }
            }

            return recipesWithInput;
        }

        public List<RecyclerRecipe> GetRecipesWithOutput(RecipeComponent output)
        {
            List<RecyclerRecipe> recipesWithOutput = new();
            if (output is not RecipeComponent<ItemStack>)
            {
                return recipesWithOutput;
            }

            foreach (RecyclerRecipe recipe in recipes)
            {
                if (recipe.IsOutput(output))
                {
                    recipesWithOutput.Add(recipe); 
                }
            }

            return recipesWithOutput;
        }

        public List<Recipe> GetRecipes()
        {
            return recipes.ConvertAll<Recipe>((recyclerRecipe) => recyclerRecipe);
        }

        public RecyclerRecipe GetRecipeWithInput(RecipeComponent input, RecycleTier recycleTier)
        {
            List<RecyclerRecipe> recipesWithInput = GetRecipesWithInput(input);
            foreach (RecyclerRecipe recipe in recipesWithInput)
            {
                if (recipe.RecycleTier == recycleTier)
                {
                    return recipe;
                }
            }
            return null;
        }

        private void AddRecipe(RecipeComponent<ItemStack> input, RecycleTier recycleTier, int duration, ChanceItemStack[] outputs)
        {
            foreach (RecyclerRecipe existingRecipe in recipes)
            {
                if (existingRecipe.RecycleTier == recycleTier && existingRecipe.Input.CanSubstituteWith(input))
                {
                    throw new ArgumentException(string.Format("There is already a recipe with the input {0} and recycle tier {1}", input.ToString(), recycleTier.ToString()));
                }
            }

            recipes.Add(new RecyclerRecipe(input, recycleTier, duration, outputs));
        }

        public void LoadRecipes()
        {
            AddRecipe(
                new ItemStack("SpaceDebris"),
                RecycleTier.Hand,
                10,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("ScrapMetal", 0.25f),
                    new ChanceItemStack("UsedBattery", 0.25f),
                    new ChanceItemStack("MangledWires", 0.25f),
                }
                );

            AddRecipe(
                new ItemStack("SpaceDebris"),
                RecycleTier.Makeshift,
                10,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("UsedBattery", 0.25f),
                    new ChanceItemStack("MangledWires", 0.25f),
                    new ChanceItemStack("BrokenSolarPanel", 0.25f),
                    new ChanceItemStack("BurnedOutMotor", 0.25f),
                    new ChanceItemStack("PuncturedFuelTank", 0.25f),
                    new ChanceItemStack("SpaceRock", 0.25f),
                }
                );

            //AddRecipe(
            //    new ItemStack(ItemDatabase.GetItem("UsedBattery"), 1),
            //    RecycleTier.Makeshift,
            //    7,
            //    new ChanceItemStack[]
            //    {
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("Lithium"),
            //            1,
            //            0.5f
            //            ),
            //    }
            //    );

            //AddRecipe(
            //    new ItemStack(ItemDatabase.GetItem("BrokenSolarPanel"), 1),
            //    RecycleTier.Makeshift,
            //    7,
            //    new ChanceItemStack[]
            //    {
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("AluminumScrap"),
            //            1,
            //            0.3f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("GlassShards"),
            //            1,
            //            0.25f
            //            ),
            //    }
            //    );

            //AddRecipe(
            //    new ItemStack(ItemDatabase.GetItem("BrokenMotor"), 1),
            //    RecycleTier.Makeshift,
            //    7,
            //    new ChanceItemStack[]
            //    {
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("CopperScrap"),
            //            1,
            //            0.3f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("Graphite"),
            //            1,
            //            0.25f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("ArcMagnet"),
            //            1,
            //            0.25f
            //            ),
            //    }
            //    );

            //AddRecipe(
            //    new ItemStack(ItemDatabase.GetItem("BrokenFuelTank"), 1),
            //    RecycleTier.Makeshift,
            //    7,
            //    new ChanceItemStack[]
            //    {
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("SteelScrap"),
            //            1,
            //            0.25f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("TitaniumScrap"),
            //            1,
            //            0.2f
            //            ),
            //    }
            //    );

            //AddRecipe(
            //    new ItemStack(ItemDatabase.GetItem("SteelScrap"), 1),
            //    RecycleTier.Makeshift,
            //    10,
            //    new ChanceItemStack[]
            //    {
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("FlatRollerPass"),
            //            1,
            //            0.1f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("SquareRollerPass"),
            //            1,
            //            0.1f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("RoundRollerPass"),
            //            1,
            //            0.1f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("TSlotRollerPass"),
            //            1,
            //            0.1f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("PipeRollerPass"),
            //            1,
            //            0.1f
            //            ),
            //        new ChanceItemStack(
            //            ItemDatabase.GetItem("RollerCutter"),
            //            1,
            //            0.1f
            //            ),
            //    }
            //    );
        }

    }

    public enum RecycleTier
    { 
        Hand,
        Makeshift,
        Basic,
        Advanced
    }

}
