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

        private bool Interact(ItemBuffer inventory, int slot, Vector2Int _) // TODO implement
        {
            // If not holding anything, take from silo
            if (inventory.GetItemInSlot(slot).IsEmpty())
            {
                ItemBuffer.MoveItems(Buffer, 0, inventory, slot);
            }
            else // Otherwise, try inserting held item into silo
            {
                ItemBuffer.MoveItems(inventory, slot, Buffer, 0);
            }
            gridObject.OnSelfChanged();
            return true;
        }


    }
}
