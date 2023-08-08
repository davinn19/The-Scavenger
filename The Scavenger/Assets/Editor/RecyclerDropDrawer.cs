using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Scavenger.CustomEditor
{
    // TODO add docs
    [CustomPropertyDrawer(typeof(RecyclerDrop))]
    public class RecyclerDropDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            int padding = 5;
            Rect itemRect = new Rect(position.x, position.y, 200, position.height);
            Rect amountRect = new Rect(itemRect.xMax + padding, position.y, 50, position.height);
            Rect chanceRect = new Rect(amountRect.xMax + padding, position.y, position.width - itemRect.width - amountRect.width - padding * 2, position.height);


            // Draw fields - pass GUIContent.none to each so they are drawn without labels

            EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("item"), GUIContent.none);
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
            EditorGUI.PropertyField(chanceRect, property.FindPropertyRelative("chance"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
