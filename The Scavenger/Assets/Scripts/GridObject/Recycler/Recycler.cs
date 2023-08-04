using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO redo, add docs
    public class Recycler : GridObjectBehavior
    {
        [SerializeField] private RecyclerRecipes recipes;

        [SerializeField] private ItemBuffer input;
        [SerializeField] private ItemBuffer output;
        [SerializeField] private EnergyBuffer energy;

        [SerializeField] private float recycleProgress = 0;

        [SerializeField]
        [Min(0)]
        private float ticksPerRecycle;


        // TODO implement
        public override void TickUpdate()
        {
            recycleProgress++;
            if (recycleProgress >= ticksPerRecycle)
            {
                recycleProgress = 0;
            }
        }
    }
}
