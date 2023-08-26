using Leguar.TotalJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.Recipes
{
    // TODO add docs
    public class RecyclerRecipes : RecipeList<RecyclerRecipe>
    {
        private static RecyclerRecipes instance = new RecyclerRecipes();
        public static RecyclerRecipes Instance => instance;

        private readonly List<RecyclerRecipe> recipes = new();


        public override List<RecyclerRecipe> GetRecipesWithInput(RecipeComponent input)
        {
            if (input is not RecipeComponent<ItemStack>)
            {
                return new();
            }
            return base.GetRecipesWithInput(input);
        }

        public override List<RecyclerRecipe> GetRecipesWithOutput(RecipeComponent output)
        {
            if (output is not RecipeComponent<ItemStack>)
            {
                return new();
            }
            return base.GetRecipesWithOutput(output);
        }

        public override List<RecyclerRecipe> GetRecipes() => recipes;

        public override List<Recipe> GetGenericRecipes()
        {
            return recipes.ConvertAll<Recipe>((recyclerRecipe) => recyclerRecipe);
        }

        public RecyclerRecipe GetRecipeWithInput(RecipeComponent input, RecycleTier recycleTier)
        {
            foreach (RecyclerRecipe recipe in GetRecipesWithInput(input))
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
            Debug.Assert(input.Amount == 1);

            // Check for recipe conflicts
            Debug.Assert(GetRecipeWithInput(input, recycleTier) == null);

            recipes.Add(new RecyclerRecipe(input, recycleTier, duration, outputs));
        }

        public override void LoadRecipes()
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
                    new ChanceItemStack("ScrapMetal", 0.25f),
                    new ChanceItemStack("UsedBattery", 0.25f),
                    new ChanceItemStack("MangledWires", 0.25f),
                    new ChanceItemStack("BrokenSolarPanel", 0.25f),
                    new ChanceItemStack("BurnedOutMotor", 0.25f),
                    new ChanceItemStack("TornPanel", 0.25f),
                    new ChanceItemStack("SpaceRock", 0.25f),
                }
                );

            AddRecipe(
                new ItemStack("ScrapMetal"),
                RecycleTier.Makeshift,
                7,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("AluminumScrap", 0.4f),
                    new ChanceItemStack("CopperScrap", 0.4f),
                    new ChanceItemStack("SteelScrap", 0.4f),
                    new ChanceItemStack("TitaniumScrap", 0.4f),
                }
                );

            AddRecipe(
                new ItemStack("UsedBattery"),
                RecycleTier.Makeshift,
                7,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("CopperScrap", 0.5f),
                    new ChanceItemStack("Lithium", 0.5f),
                }
                );

            AddRecipe(
                new ItemStack("BrokenSolarPanel"),
                RecycleTier.Makeshift,
                7,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("AluminumScrap", 0.5f),
                    new ChanceItemStack("ShatteredGlass", 0.5f),
                }
                );

            AddRecipe(
                new ItemStack("BurnedOutMotor"),
                RecycleTier.Makeshift,
                7,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("Graphite", 0.5f),
                    new ChanceItemStack("ArcMagnet", 0.5f),
                }
                );

            AddRecipe(
                new ItemStack("TornPanel"),
                RecycleTier.Makeshift,
                7,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("SteelScrap", 0.4f),
                    new ChanceItemStack("TitaniumScrap", 0.35f),
                }
                );

            AddRecipe(
                new ItemStack("SteelScrap"),
                RecycleTier.Makeshift,
                10,
                new ChanceItemStack[]
                {
                    new ChanceItemStack("FlatRollingPass", 0.1f),
                    new ChanceItemStack("SquareRollingPass", 0.1f),
                    new ChanceItemStack("RoundRollingPass", 0.1f),
                    new ChanceItemStack("BeamRollingPass", 0.1f),
                    new ChanceItemStack("PipeRollingPass", 0.1f),
                }
                );

            // TODO continue adding recipes
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
