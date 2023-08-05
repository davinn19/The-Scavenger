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
        }

        // TODO add docs
        public virtual void ReadPersistentData(PersistentData data)
        {
            foreach (IHasPersistentData dataWriter in GetComponentsInChildren<IHasPersistentData>())
            {
                dataWriter.ReadPersistentData(data);
            }
        }

        // TODO add docs
        public PersistentData WritePersistentData()
        {
            PersistentData data = new PersistentData();

            // Allow addons to write their data first
            foreach (IHasPersistentData dataWriter in GetComponentsInChildren<IHasPersistentData>())
            {
                dataWriter.WritePersistentData(data);
            }
            WritePersistentData(data);

            return data;
        }

        // TODO add docs
        protected virtual PersistentData WritePersistentData(PersistentData data) => data;


        public virtual void OnPlace() { }
        public virtual void OnRemove() { }
        public virtual void OnNeighborPlaced(Vector2Int side) { }
        public virtual void OnNeighborChanged() { }

        public virtual bool TryInteract(ItemBuffer inventory, HeldItemHandler heldItemHandler, Vector2Int sidePressed) => false;
        public virtual bool TryEdit(Vector2Int sidePressed) => false;

        public virtual void TickUpdate() { }
    }
}
