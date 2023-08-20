using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class Categories : ItemProperty
    {
        public override string Name => "Categories";

        [SerializeField] private string[] categories;

        public bool IsInCategory(string category)
        {
            return Array.Exists(categories, (itemCategory) => category == itemCategory);
        }

        public string[] GetCategories()
        {
            return categories;
        }
    }
}
