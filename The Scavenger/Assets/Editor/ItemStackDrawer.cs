using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Scavenger.CustomEditor
{
    // TODO add docs
    [CustomPropertyDrawer(typeof(ItemStack))]
    public class ItemStackDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement container = new();

            PropertyField itemField = new PropertyField(property.FindPropertyRelative("<Item>k__BackingField"));
            container.Add(itemField);

            PropertyField amountField = new PropertyField(property.FindPropertyRelative("Amount"));
            container.Add(amountField);

            return container;
        }
    }
}
