using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scavenger/Database/Item")]
    public class ItemDatabase : PrefabDatabase<Item>
    {
        [SerializeField] private Item[] items;

        public Item Get(string id)
        {
            if (id == "" || id == null)
            {
                return null;
            }

            Item cacheResult = GetFromCache(id);
            if (cacheResult)
            {
                return cacheResult;
            }

            foreach (Item item in items)
            {
                if (item.name == id)
                {
                    AddToCache(item.name, item);
                    return item;
                }
            }
            throw new ArgumentException(string.Format("No item with id {0} was found.", id));
        }

        // TODO add docs
        [ContextMenu("Update Database")]
        public override void UpdateDatabase()
        {
            items = FindAssets().ToArray();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
