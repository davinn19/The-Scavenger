namespace Scavenger.UI.InspectorContent
{
    // TODO add docs
    public class ItemUIContent : UIContent<Item>
    {
        public void Init(Item item)
        {
            Target = item;
            enabled = true;
        }

    }
}
