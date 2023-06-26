using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class Recycler : MonoBehaviour
    {
        [SerializeField] private RecyclerRecipes recipes;

        [SerializeField] private ItemBuffer input;
        [SerializeField] private ItemBuffer output;
        [SerializeField] private EnergyBuffer energy;

        [SerializeField] private float recycleProgress = 0;

        [SerializeField]
        [Min(0)]
        private float ticksPerRecycle;

        
        private void TickUpdate()
        {
            recycleProgress++;
            if (recycleProgress >= ticksPerRecycle)
            {
                recycleProgress = 0;
            }

            ItemStack recycledItem = input.FindFirst(recipes.IsRecyclable);
        }
    }
}
