using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scavenger
{
    public class ItemPropertiesSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private Item targetItem;
        Action<Type> callback;

        public void Init(Item targetItem, Action<Type> callback)
        {
            this.targetItem = targetItem;
            this.callback = callback;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Item Properties"), 0));

            foreach (Type propertyType in ItemProperty.GetProperties())
            {
                if (!targetItem.HasProperty(propertyType))
                {
                    entries.Add(CreateEntry(propertyType));
                }
            }

            return entries;
        }

        private SearchTreeEntry CreateEntry(Type propertyType)
        {
            string name = propertyType.Name;

            SearchTreeEntry newEntry = new SearchTreeEntry(new GUIContent(name));

            newEntry.level = 1;
            newEntry.userData = propertyType;

            return newEntry;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Type propertyType = searchTreeEntry.userData as Type;
            callback(propertyType);
            return true;
        }
    }
}
