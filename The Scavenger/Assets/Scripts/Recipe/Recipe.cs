using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO add docs
namespace Scavenger.Recipes
{
    public interface Recipe
    {
        public bool IsInput(RecipeComponent component);
        public bool IsOutput(RecipeComponent component);
    }
}
