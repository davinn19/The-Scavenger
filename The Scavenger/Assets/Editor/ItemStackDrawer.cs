using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scavenger
{
    [CustomPropertyDrawer(typeof(ItemStack))]
    public class ItemStackDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new();

            PropertyField itemField = new PropertyField(property.FindPropertyRelative("<Item>k__BackingField"));
            container.Add(itemField);

            PropertyField amountField = new PropertyField(property.FindPropertyRelative("amount"));
            container.Add(amountField);    

            return container;
        }
    }
}