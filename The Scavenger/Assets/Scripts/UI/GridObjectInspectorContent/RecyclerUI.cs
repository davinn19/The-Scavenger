using Scavenger.GridObjectBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI.InspectorContent
{
    // TODO add docs
    public class RecyclerUI : GridObjectInspectorContent
    {
        private Recycler recycler;
        private RecyclerItemBuffer buffer;

        private EnergyGauge energyGauge;
        [SerializeField] ProgressBar recycleProgressBar;

        [SerializeField] private SlotDisplay inputSlot;
        [SerializeField] private GameObject outputSlotGroup;

        private void Awake()
        {
            energyGauge = GetComponentInChildren<EnergyGauge>();
        }

        public override void Init(GridObject target, PlayerInventory inventory)
        {
            base.Init(target, inventory);
            recycler = target.GetComponent<Recycler>();

            buffer = target.GetComponent<RecyclerItemBuffer>();
            InitSlots();

            energyGauge.Buffer = target.GetComponent<EnergyBuffer>();
        }

        private void InitSlots()
        {
            inputSlot.SetWatchedBuffer(buffer, buffer.GetInput());

            SlotDisplay[] outputSlotDisplays = outputSlotGroup.GetComponentsInChildren<SlotDisplay>();
            List<ItemStack> outputSlots = buffer.GetOutputSlots();
            for (int i = 0; i < outputSlotDisplays.Length; i++)
            {
                outputSlotDisplays[i].SetWatchedBuffer(buffer, outputSlots[i]);
            }
        }

        private void Update()
        {
            recycleProgressBar.UpdatePercentage(recycler.RecycleProgress, recycler.TicksPerRecycle, "Progress: {0}%");
        }
    }
}
