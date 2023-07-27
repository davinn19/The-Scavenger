using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Stores a large amount of one type of item.
    /// </summary>
    [RequireComponent(typeof(ItemBuffer), typeof(GridObject))]
    public class Silo : MonoBehaviour
    {
        private GridObject gridObject;
        public ItemBuffer Buffer { get; private set; }


        private void Awake()
        {
            Buffer = GetComponent<ItemBuffer>();
            gridObject = GetComponent<GridObject>();
            gridObject.TryInteract = Interact;
            gridObject.TryEdit = (_) => { ToggleLock(); return true; };
        }

        /// <summary>
        /// Attempts to insert the current held item into the silo, or extract the silo's contents.
        /// </summary>
        /// <param name="inventory">The player's inventory</param>
        /// <param name="heldItem">The current held item.</param>
        /// <returns>Always returns true.</returns>
        private bool Interact(ItemBuffer inventory, HeldItemHandler heldItem, Vector2Int _) // TODO implement
        {
            // If not holding anything, take from silo
            if (heldItem.IsEmpty())
            {
                heldItem.TakeItemsFrom(Buffer, 0);
                // TODO implement filling inventory once itemSelection is full
            }
            else // Otherwise, try inserting held item into silo
            {
                heldItem.MoveItemsTo(Buffer, 0);
            }
            gridObject.OnSelfChanged();
            return true;
        }

        /// <summary>
        /// Toggles the silo's locked status.
        /// </summary>
        private void ToggleLock()
        {
            Buffer.ToggleLocked(0);
            gridObject.OnSelfChanged();
        }
    }
}
