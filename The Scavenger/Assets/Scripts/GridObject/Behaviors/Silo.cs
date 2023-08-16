using Leguar.TotalJSON;
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

        // TODO add docs
        public bool IsLocked() => Buffer.IsLocked();

        /// <summary>
        /// Attempts to insert the current held item into the silo, or extract the silo's contents.
        /// </summary>
        /// <param name="inventory">The player's inventory.</param>
        /// <returns>Always returns true.</returns>
        public override bool TryInteract(PlayerInventory inventory, Vector2Int _) // TODO implement
        {
            // TODO redo

            ItemStack heldItem = inventory.GetHeldItem();
            ItemStack storedItem = Buffer.StoredItem;

            if (heldItem)
            {
                ItemTransfer.MoveStackToStack(heldItem, storedItem, heldItem.Amount, Buffer.AcceptsItemStack, false);
            }
            else
            {
                ItemTransfer.MoveStackToBuffer(storedItem, inventory.GetInventory(), storedItem.Amount, inventory.AcceptsItemStack, false);
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

        // TODO add docs
        public override void ReadPersistentData(JSON data)
        {
            base.ReadPersistentData(data);
            if (data.ContainsKey("ItemBuffer"))
            {
                Buffer.ReadPersistentData(data.GetJSON("ItemBuffer"));
            }
        }

        // TODO add docs
        public override JSON WritePersistentData()
        {
            JSON data = base.WritePersistentData();
            JSONHelper.TryAdd(data, "ItemBuffer", Buffer.WritePersistentData());
            return data;
        }
    }
}
