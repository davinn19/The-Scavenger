using Leguar.TotalJSON;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    [RequireComponent(typeof(GridObject))]
    public abstract class GridObjectBehavior : MonoBehaviour    // TODO add docs
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
        public void ReadPersistentData(JSON data)
        {
            if (data == null)
            {
                data = new JSON();
            }

            foreach (IHasPersistentData dataReader in GetComponents<IHasPersistentData>())
            {
                dataReader.Read(data);
            }
        }

        // TODO add docs
        public JSON WritePersistentData()
        {
            JSON data = new();
            // Allow addons to write their data first
            foreach (IHasPersistentData dataWriter in GetComponents<IHasPersistentData>())
            {
                dataWriter.Write(data);
            }
            return data;
        }

        public virtual void OnPlace() { }
        public virtual void OnRemove() { }
        public virtual void OnNeighborPlaced(Vector2Int side) { }
        public virtual void OnNeighborChanged() { }

        public virtual bool TryInteract(ItemBuffer inventory, HeldItemHandler heldItemHandler, Vector2Int sidePressed) => false;
        public virtual bool TryEdit(Vector2Int sidePressed) => false;

        public virtual void TickUpdate() { }
    }
}