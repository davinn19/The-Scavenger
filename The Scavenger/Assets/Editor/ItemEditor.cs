using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Scavenger
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        private Rect buttonRect;
        private ItemPropertiesSearchProvider searchProvider;


        private SerializedProperty displayName;
        private SerializedProperty icon;

        private SerializedProperty properties;

        // private Texture2D thumbnail = null;


        private void OnEnable()
        {
            Item item = target as Item;
            searchProvider = CreateInstance<ItemPropertiesSearchProvider>();
            searchProvider.Init(item, AddProperty);

            displayName = serializedObject.FindProperty("<DisplayName>k__BackingField");
            icon = serializedObject.FindProperty("<Icon>k__BackingField");
            properties = serializedObject.FindProperty("properties");

            //UpdateThumbnail();
        }


        //public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
        //{
        //    if (thumbnail == null)
        //    {
        //        return base.RenderStaticPreview(assetPath, subAssets, width, height);
        //    }
        //    return thumbnail;
        //}

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptProperty();
            DrawMainProperties();
            DrawAddedProperties();
            DrawPropertyAdderButton();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptProperty()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
        }

        private void DrawMainProperties()
        {
            EditorGUILayout.PropertyField(displayName);

            //EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(icon);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    UpdateThumbnail();
            //}
        }

        //private void UpdateThumbnail()
        //{
        //    if (icon.objectReferenceValue == null)
        //    {
        //        thumbnail = null;
        //        return;
        //    }

        //    Sprite sprite = icon.objectReferenceValue as Sprite;
        //    if (sprite == null)
        //    {
        //        thumbnail = null;
        //    }
        //    else
        //    {
        //        thumbnail = AssetPreview.GetAssetPreview(sprite);
        //    }
        //}

        private void DrawAddedProperties()
        {
            foreach (SerializedProperty element in properties)
            {
                SerializedObject itemProperty = new SerializedObject(element.objectReferenceValue);
                itemProperty.Update();

                EditorGUILayout.BeginFoldoutHeaderGroup(true, itemProperty.targetObject.name);

                SerializedProperty itemPropertyVariable = itemProperty.GetIterator();
                while (itemPropertyVariable.NextVisible(true))
                {
                    if (itemPropertyVariable.name == "m_Script")
                    {
                        continue;
                    }

                    EditorGUILayout.PropertyField(itemPropertyVariable);
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                itemProperty.ApplyModifiedProperties();
            }
        }


        private void DrawPropertyAdderButton()
        {
            // Variables for sizing button
            float padding = 1;
            float searchWindowWidth = 240;
            float buttonHeight = EditorGUIUtility.singleLineHeight * 1.5f;

            GUILayout.BeginHorizontal();
            GUILayout.Label("", new GUILayoutOption[] { GUILayout.ExpandWidth(true) });  // Spacer to center button

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


        private void AddProperty(Type propertyType)
        {
            serializedObject.Update();

            if (!propertyType.IsSubclassOf(typeof(ItemProperty)))
            {
                Debug.LogError(propertyType.Name + " is not an ItemProperty");
                return;
            }

            if (HasProperty(propertyType))
            {
                Debug.LogError("The item already contains property " + propertyType.Name);
                return;
            }

            ItemProperty newProperty = CreateInstance(propertyType) as ItemProperty;
            newProperty.name = newProperty.Name;
            newProperty.Item = target as Item;

            AssetDatabase.AddObjectToAsset(newProperty, target);


            properties.arraySize++;
            properties.GetArrayElementAtIndex(properties.arraySize - 1).objectReferenceValue = newProperty;

            serializedObject.ApplyModifiedProperties();
        }


        private bool HasProperty(Type propertyType)
        {
            foreach (UnityEngine.Object asset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(target)))
            {
                if (asset.GetType() == propertyType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
