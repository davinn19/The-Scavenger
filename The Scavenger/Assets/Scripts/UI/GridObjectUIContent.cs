namespace Scavenger.UI.UIContent
{
    // TODO add docs?
    public class GridObjectUIContent : UIContent<GridObject>
    {
        protected ItemBuffer Inventory { get; private set; }

        public virtual void Init(GridObject target, ItemBuffer inventory)
        {
            Target = target;
            Inventory = inventory;
            enabled = true;
        }
    }
}
