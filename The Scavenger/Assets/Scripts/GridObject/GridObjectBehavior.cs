using Leguar.TotalJSON;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(GridObject)), DisallowMultipleComponent]
    public abstract class GridObjectBehavior : MonoBehaviour, IHasPersistentData    // TODO add docs
    {
        protected GridObject gridObject;

        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            gridObject = GetComponent<GridObject>();
            gridObject.SubscribeTickUpdate(TickUpdate);
        }

        // TODO add docs
        public virtual void ReadPersistentData(JSON data) 
        { 
            if (data.ContainsKey("HP"))
            {
                gridObject.HP.ReadPersistentData(data.GetJSON("HP"));
            }
        }

        // TODO add docs
        public virtual JSON WritePersistentData()
        {
            JSON data = new JSON();

            JSON hpData = gridObject.HP.WritePersistentData();
            JSONHelper.TryAdd(data, "HP", hpData);

            return data;
        }

        public virtual List<ItemStack> GetRemoveDrops()
        {
            return new List<ItemStack>();
        }

        public virtual void OnPlace() { }
        public virtual void OnRemove() { }
        public virtual void OnNeighborPlaced(Vector2Int side) { }
        public virtual void OnNeighborChanged() { }

        public virtual bool TryInteract(ItemBuffer inventory, HeldItemBuffer heldItemBuffer, Vector2Int sidePressed) => false;
        public virtual bool TryEdit(Vector2Int sidePressed) => false;

        protected virtual void TickUpdate() { }
    }
}