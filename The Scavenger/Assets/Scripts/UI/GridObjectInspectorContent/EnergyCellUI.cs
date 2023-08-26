using UnityEngine;
using Scavenger.GridObjectBehaviors;

namespace Scavenger.UI.InspectorContent
{
    // TODO add docs
    public class EnergyCellUI : GridObjectInspectorContent
    {
        [SerializeField] private EnergyGauge energyGauge;

        public override void Init(GridObject target, PlayerInventory inventory)
        {
            base.Init(target, inventory);
            energyGauge.Buffer = target.GetComponent<EnergyBuffer>();
        }
    }
}
