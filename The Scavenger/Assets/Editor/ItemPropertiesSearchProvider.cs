using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scavenger
{
    public class ItemPropertiesSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            return new List<SearchTreeEntry>() { new SearchTreeGroupEntry(new GUIContent("Item Properties"), 0) };
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            return true;
        }
    }
}
