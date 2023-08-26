using Scavenger.GridObjectBehaviors;
using Scavenger.Recipes;
using UnityEngine;

namespace Scavenger.UI.InspectorContent
{
    // TODO add docs
    public class ArcFurnaceUI : GridObjectInspectorContent
    {
        [SerializeField] SlotDisplay inputSlot;
        [SerializeField] SlotDisplay outputSlot;
        [SerializeField] SlotDisplay byproductSlot;
        [SerializeField] private ProgressBar progressBar;

        private ArcFurnace arcFurnace;

        public override void Init(GridObject target, PlayerInventory inventory)
        {
            base.Init(target, inventory);
            arcFurnace = target.GetComponent<ArcFurnace>();

            ArcFurnaceItemBuffer itemBuffer = arcFurnace.GetComponent<ArcFurnaceItemBuffer>();
            inputSlot.SetWatchedBuffer(itemBuffer, itemBuffer.GetInput());
            outputSlot.SetWatchedBuffer(itemBuffer, itemBuffer.GetOutput());
            byproductSlot.SetWatchedBuffer(itemBuffer, itemBuffer.GetByproduct());

            GetComponentInChildren<EnergyGauge>().Buffer = arcFurnace.GetComponent<EnergyBuffer>();
        }

        private void Update()
        {
            FurnaceRecipe currentRecipe = arcFurnace.GetCurrentRecipe();
            if (currentRecipe != null)
            {
                progressBar.UpdateRatio(arcFurnace.GetTemperature(), currentRecipe.requiredTemperature, "Temperature: {0}/{1}");
            }
            else
            {
                progressBar.SetValues(0, 1);
                progressBar.SetText("No valid input");
            }

            
        }
    }
}
