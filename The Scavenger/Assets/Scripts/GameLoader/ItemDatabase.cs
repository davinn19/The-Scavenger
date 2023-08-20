using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public static class ItemDatabase
    {
        private static bool loaded = false;
        public static bool Loaded => loaded;

        private static Dictionary<string, Item> items = new();
        private static Dictionary<string, List<Item>> categories = new();

        public static Item GetItem(string id)
        {
            if (id == null || id == "")
            {
                return null;
            }
            return items[id];
        }

        public static List<Item> GetItemsInCategory(string category) => categories[category];

        public static void Load()
        {
            foreach (Item item in Resources.LoadAll<Item>("Items"))
            {
                items[item.name] = item;

                foreach (string category in item.GetCategories())
                {
                    if (!categories.ContainsKey(category))
                    {
                        categories[category] = new();
                    }
                    categories[category].Add(item);
                }
            }

            loaded = true;
        }

    }
}
