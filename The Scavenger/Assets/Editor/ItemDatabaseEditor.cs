using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scavenger.CustomEditor
{
    // TODO remove
    public class ItemDatabaseEditor : Editor
    {
        private SerializedProperty keysProperty;
        private SerializedProperty itemsProperty;

        private void OnEnable()
        {
            itemsProperty = serializedObject.FindProperty("items");
            keysProperty = serializedObject.FindProperty("keys");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            bool pressed = GUILayout.Button("Update Item Database");
            if (pressed)
            {
                UpdateItems();
            }
        }

        private void UpdateItems()
        {
            serializedObject.Update();

            Item[] items = GetItems();
            itemsProperty.arraySize = items.Length;

            for (int i = 0; i < items.Length; i++)
            {
                keysProperty.GetArrayElementAtIndex(i).stringValue = items[i].name;
                itemsProperty.GetArrayElementAtIndex(i).objectReferenceValue = items[i];
            }

            serializedObject.ApplyModifiedProperties();
        }

        private Item[] GetItems()
        {
            List<Item> items = new();

            string[] guids = AssetDatabase.FindAssets("t:item");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}
