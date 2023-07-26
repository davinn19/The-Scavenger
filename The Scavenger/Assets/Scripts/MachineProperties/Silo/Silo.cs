using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(ItemBuffer), typeof(GridObject))]
    public class Silo : MonoBehaviour
    {
        private GridObject gridObject;
        private SpriteRenderer icon;
        public ItemBuffer Buffer { get; private set; }


        private void Awake()
        {
            Buffer = GetComponent<ItemBuffer>();
            icon = GetComponent<SpriteRenderer>();
            gridObject = GetComponent<GridObject>();
            gridObject.TryInteract = Interact;
        }

        private bool Interact(ItemBuffer inventory, ItemSelection itemSelection, Vector2Int _) // TODO implement
        {
            // If not holding anything, take from silo
            if (itemSelection.IsEmpty())
            {
                itemSelection.TakeItemsFrom(Buffer, 0);
                // TODO implement filling inventory once itemSelection is full
            }
            else // Otherwise, try inserting held item into silo
            {
                itemSelection.MoveItemsTo(Buffer, 0);
            }
            gridObject.OnSelfChanged();
            return true;
        }


    }
}
