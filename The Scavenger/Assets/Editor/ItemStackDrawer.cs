using UnityEditor;
using UnityEngine;

namespace Scavenger.CustomEditor
{
    // TODO add docs
    [CustomPropertyDrawer(typeof(ItemStack))]
    public class ItemStackDrawer : PropertyDrawer
    {
        // TODO implement, use objectpicker instead of object field, special sprite is targethover
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUILayout.PropertyField(property.FindPropertyRelative("itemID"));
            EditorGUILayout.PropertyField(property.FindPropertyRelative("Amount"));

            Object selectedObject = EditorGUILayout.ObjectField("Select items here ->", default(Object), typeof(Item), true);
            if (selectedObject != null)
            {
                property.FindPropertyRelative("itemID").stringValue = selectedObject.name;
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
