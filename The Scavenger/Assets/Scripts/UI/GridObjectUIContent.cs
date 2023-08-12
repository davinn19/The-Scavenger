namespace Scavenger.UI.UIContent
{
    // TODO add docs?
    public class GridObjectUIContent : UIContent<GridObject>
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
