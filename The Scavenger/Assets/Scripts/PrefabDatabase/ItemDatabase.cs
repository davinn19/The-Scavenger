using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scavenger/Database/Item")]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private Item[] items = new Item[1];
        [SerializeField] private List<Item> outOfRangeItems = new();

        public Item GetItem(int id)
        {
            if (id < items.Length)
            {
                return items[id];
            }

            return outOfRangeItems.Find((item) => { return item.ID == id; });
        }

        // TODO add docs
        private void Reset()
        {
            items = new Item[1];
            outOfRangeItems.Clear();

            foreach (Item foundItem in FindItemAssets())
            {
                foundItem.ID = 0;
                EditorUtility.SetDirty(foundItem);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        // TODO add docs
        [ContextMenu("Update Database")]
        private void UpdateDatabase()
        {
            outOfRangeItems.Clear();
            Queue<Item> noIDItems = new();

            List<Item> foundItems = FindItemAssets();
            items = new Item[foundItems.Count + 1];

            foreach (Item foundItem in foundItems)
            {
                if (foundItem.ID == 0)
                {
                    noIDItems.Enqueue(foundItem);
                }
                else if (foundItem.ID >= items.Length)
                {
                    outOfRangeItems.Add(foundItem);
                }
                else
                {
                    items[foundItem.ID] = foundItem;
                }
            }

            for (int i = 1; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    continue;
                }

                Item item;
                if (noIDItems.TryDequeue(out item))
                {
                    item.ID = i;
                    items[i] = item;
                    EditorUtility.SetDirty(item);
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }



        // TODO add docs
        private List<Item> FindItemAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:item");
            List<Item> foundItems = new();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
                foundItems.Add(item);
            }

            return foundItems;
        }


    }
}
