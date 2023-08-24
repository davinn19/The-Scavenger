namespace Scavenger.UI.InspectorContent
{
    // TODO add docs?
    public class GridObjectInspectorContent : UIContent<GridObject>
    {
        protected PlayerInventory Inventory { get; private set; }

        public virtual void Init(GridObject target, PlayerInventory inventory)
        {
            Target = target;
            Inventory = inventory;
            enabled = true;
        }
    }
}
