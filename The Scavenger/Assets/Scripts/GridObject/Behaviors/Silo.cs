using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    /// <summary>
    /// Stores a large amount of one type of item.
    /// </summary>
    [RequireComponent(typeof(SiloItemBuffer))]
    public class Silo : GridObjectBehavior
    {
        public SiloItemBuffer Buffer { get; private set; }

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            Buffer = GetComponent<SiloItemBuffer>();
        }

        /// <summary>
        /// Attempts to insert the current held item into the silo, or extract the silo's contents.
        /// </summary>
        /// <param name="inventory">The player's inventory</param>
        /// <param name="heldItem">The current held item.</param>
        /// <returns>Always returns true.</returns>
        public override bool TryInteract(ItemBuffer inventory, HeldItemBuffer heldItem, Vector2Int _) // TODO implement
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
        /// <returns>Always returns true.</returns>
        public override bool TryEdit(Vector2Int _)
        {
            Buffer.ToggleLocked();
            gridObject.OnSelfChanged();
            return true;
        }
    }
}
