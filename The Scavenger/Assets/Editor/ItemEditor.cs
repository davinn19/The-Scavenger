using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System;

namespace Scavenger
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        private Rect buttonRect;
        private ItemPropertiesSearchProvider searchProvider;

        private SerializedProperty properties;

        public void OnEnable()
        {
            Item item = target as Item;
            searchProvider = CreateInstance<ItemPropertiesSearchProvider>();
            searchProvider.Init(item);

            properties = serializedObject.FindProperty("properties");

            serializedObject.Update();
            if (!(target as Item).HasProperty<Basic>())
            {
                Add(typeof(Basic));
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.Label("" + properties.arraySize);

            EditorGUILayout.PropertyField(properties);

            foreach (SerializedProperty itemProperty in properties)
            {
                ItemProperty hey = itemProperty.boxedValue as ItemProperty;
                EditorGUILayout.BeginFoldoutHeaderGroup(true, (hey.definition != null).ToString());
                EditorGUILayout.EndFoldoutHeaderGroup();
                
            }

            AddPropertyAdderButton();

            serializedObject.ApplyModifiedProperties();
        }

        public void AddPropertyAdderButton()
        {
            // Variables for sizing button
            float padding = 1;
            float searchWindowWidth = 240;
            float buttonHeight = EditorGUIUtility.singleLineHeight * 1.5f;

            GUILayout.BeginHorizontal();
            GUILayout.Label("", new GUILayoutOption[] { GUILayout.ExpandWidth(true)});  // Spacer to center button

            // Create button
            bool buttonPressed = GUILayout.Button("Add Property", new GUILayoutOption[] { GUILayout.Width(searchWindowWidth + padding), GUILayout.Height(buttonHeight) });

            // Saves button dimensions
            if (Event.current.type == EventType.Repaint)
            {
                buttonRect = GUILayoutUtility.GetLastRect();
            }

            GUILayout.Label("", new GUILayoutOption[] { GUILayout.ExpandWidth(true) }); // Spacer to center button
            GUILayout.EndHorizontal();

            // Add search window when button is pressed
            if (buttonPressed)
            {
                float searchX = buttonRect.width / 2 + buttonRect.xMin;
                float searchY = buttonRect.yMax + buttonHeight / 2 + padding;

                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(new Vector2(searchX, searchY))), searchProvider);
            }
        }

        public void Add(Type definitionType)
        {
            

            ItemProperty itemProperty = CreateInstance<ItemProperty>();
            ItemPropertyDefinition definition = itemProperty.InitDefinition(definitionType);
            definition.name = definitionType.Name;

            
            AssetDatabase.AddObjectToAsset(definition, target);
            //AssetDatabase.AddObjectToAsset(itemProperty, target);

            properties.arraySize++;
            properties.GetArrayElementAtIndex(properties.arraySize - 1).objectReferenceValue = itemProperty;
        }
    }
}
