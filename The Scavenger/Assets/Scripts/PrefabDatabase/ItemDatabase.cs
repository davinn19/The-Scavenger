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

        public override Item Get(string id)
        {
            if (id == "" || id == null)
            {
                return null;
            }

            foreach (Item item in items)
            {
                if (item.name == id)
                {
                    return item;
                }
            }
            throw new ArgumentException(string.Format("No item with id {0} was found.", id));
        }

        // TODO add docs
        private void Reset()
        {
            items = default;

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        // TODO add docs
        [ContextMenu("Update Database")]
        protected override void UpdateDatabase()
        {
            items = FindAssets().ToArray();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
