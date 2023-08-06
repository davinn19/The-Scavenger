using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scavenger.CustomEditor
{
    // TODO add docs
    [CustomPropertyDrawer(typeof(ItemStack))]
    public class ItemStackDrawer : PropertyDrawer
    {
        // TODO implement
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUILayout.PropertyField(property.FindPropertyRelative("<Item>k__BackingField"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Amount"));

            EditorGUI.EndProperty();
        }
    }
}
