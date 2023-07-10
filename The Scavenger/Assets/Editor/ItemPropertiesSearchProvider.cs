using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scavenger
{
    public class ItemPropertiesSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private Item targetItem;

        public void Init(Item targetItem)
        {
            this.targetItem = targetItem;
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new();
            entries.Add(new SearchTreeGroupEntry(new GUIContent("Item Properties"), 0));

            foreach (Type definition in ItemPropertyDefinition.GetDefinitions())
            {
                if (!targetItem.HasProperty(definition))
                {
                    entries.Add(CreateEntry(definition));
                }
            }

            return entries;
        }

        private SearchTreeEntry CreateEntry(Type definition)
        {
            string name = definition.Name;

            SearchTreeEntry newEntry = new SearchTreeEntry(new GUIContent(name));

            newEntry.level = 1;
            newEntry.userData = definition;

            return newEntry;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Type definition = searchTreeEntry.userData as Type;
            targetItem.TryAddProperty(definition);
            return true;
        }
    }
}
