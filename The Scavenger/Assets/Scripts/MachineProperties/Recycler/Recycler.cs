using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(GridObject))]
    public class Recycler : MonoBehaviour
    {
        [SerializeField] private RecyclerRecipes recipes;

        [SerializeField] private ItemBuffer input;
        [SerializeField] private ItemBuffer output;
        [SerializeField] private EnergyBuffer energy;

        private GridObject gridObject;

        [SerializeField] private float recycleProgress = 0;

        [SerializeField]
        [Min(0)]
        private float ticksPerRecycle;

        private void Awake()
        {
            gridObject = GetComponent<GridObject>();
        }

        private void Start()
        {
            gridObject.QueueTickUpdate(ProcessRecycle);
        }

        private void ProcessRecycle()
        {
            // TODO finish
            recycleProgress++;
            if (recycleProgress >= ticksPerRecycle)
            {
                recycleProgress = 0;
            }

            ItemStack recycledItem = input.FindFirst(recipes.IsRecyclable);
        }
    }
}
