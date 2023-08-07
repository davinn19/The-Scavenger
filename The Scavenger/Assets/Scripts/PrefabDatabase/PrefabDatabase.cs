using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public abstract class PrefabDatabase<T> : ScriptableObject where T : Object
    {
        private Dictionary<string, T> cache = new();

        public virtual T Get(string id)
        {
            if (cache.ContainsKey(id))
            {
                return cache[id];
            }
            return null;
        }

        protected void AddToCache(T asset)
        {
            cache.Add(asset.name, asset);
        }


        protected List<T> FindAssets()
        {
            string[] guids = AssetDatabase.FindAssets("", new string[] { "Assets/Prefabs" });
            List<T> foundAssets = new();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                {
                    GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                    if (!gameObject)
                    {
                        continue;
                    }

                    if (gameObject.TryGetComponent(out T asset))
                    {
                        foundAssets.Add(asset);
                    }

                }
                else
                {
                    T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                    if (asset)
                    {
                        foundAssets.Add(asset);
                    }
                }
            }
            return foundAssets;
        }

        protected abstract void UpdateDatabase();
    }
}
