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
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            int padding = 5;
            Rect buttonRect = new Rect(position.xMax - position.height, position.y, position.height, position.height);

            Rect amountRect = new Rect(buttonRect.x, position.y, 50, position.height);
            amountRect.x -= amountRect.width + padding;

            Rect itemRect = Rect.MinMaxRect(position.x, position.y, amountRect.x - padding, position.yMax);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("itemID"), GUIContent.none);
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Amount"), GUIContent.none);

            Object selectedObject = EditorGUI.ObjectField(buttonRect, GUIContent.none, default(Object), typeof(Item), true);
            if (selectedObject != null)
            {
                property.FindPropertyRelative("itemID").stringValue = selectedObject.name;
                property.serializedObject.ApplyModifiedProperties();
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}
