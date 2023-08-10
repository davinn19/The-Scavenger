using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Scavenger.CustomEditor
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            GUI.enabled = false;
            EditorGUILayout.PropertyField(property);
            GUI.enabled = true;
            EditorGUI.EndProperty();
        }
    }
}
