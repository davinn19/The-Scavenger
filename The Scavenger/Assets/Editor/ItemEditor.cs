using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace Scavenger
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        private Rect buttonRect;
        private ItemPropertiesSearchProvider searchProvider;

        public void OnEnable()
        {
            searchProvider = CreateInstance<ItemPropertiesSearchProvider>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            AddPropertyAdder();
        }

        public void AddPropertyAdder()
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
    }
}
