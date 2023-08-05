using UnityEngine;

namespace Scavenger.UI.UIContent
{
    // TODO add docs
    public class EnergyCellUI : GridObjectUIContent
    {
        [SerializeField] private EnergyGauge energyGauge;

        public override void Init(GridObject target, ItemBuffer inventory)
        {
            base.Init(target, inventory);
            energyGauge.Buffer = target.GetComponent<EnergyBuffer>();
        }
    }
}
