using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO redo, add docs
    public class Recycler : GridObjectBehavior
    {
        [SerializeField] private RecyclerRecipeList recipeList;

        private ItemBuffer itemBuffer;
        private EnergyBuffer energyBuffer;

        [SerializeField] private float recycleProgress = 0;

        [SerializeField]
        [Min(0)]
        private float ticksPerRecycle;

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            itemBuffer = GetComponent<ItemBuffer>();
            energyBuffer = GetComponent<EnergyBuffer>();
        }

        // TODO implement
        protected override void TickUpdate()
        {
            recycleProgress++;
            if (recycleProgress >= ticksPerRecycle)
            {
                recycleProgress = 0;
            }
        }
    }
}
