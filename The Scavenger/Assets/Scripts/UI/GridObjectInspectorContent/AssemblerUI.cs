using Scavenger.GridObjectBehaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI.InspectorContent
{
    public class AssemblerUI : GridObjectInspectorContent
    {
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private SlotDisplay baseItemDisplay;
        [SerializeField] private GameObject addonItemDisplayGroup;
        private SlotDisplay[] addonItemDisplays;
        [SerializeField] private SlotDisplay outputItemDisplay;

        private Assembler assembler;
        private AssemblerItemBuffer itemBuffer;

        public override void Init(GridObject target, PlayerInventory inventory)
        {
            assembler = target.GetComponent<Assembler>();
            itemBuffer = target.GetComponent<AssemblerItemBuffer>();

            addonItemDisplays = addonItemDisplayGroup.GetComponentsInChildren<SlotDisplay>();
            ItemStack[] addons = itemBuffer.GetAddons();
            ItemStack[] filters = itemBuffer.GetFilters();

            baseItemDisplay.SetWatchedBuffer(itemBuffer, itemBuffer.GetBase());
            baseItemDisplay.GetComponent<FilteredDisplay>().SetFilter(filters[0]);

            outputItemDisplay.SetWatchedBuffer(itemBuffer, itemBuffer.GetOutput());

            for (int i = 0; i < addonItemDisplays.Length; i++)
            {
                addonItemDisplays[i].SetWatchedBuffer(itemBuffer, addons[i]);
                addonItemDisplays[i].GetComponent<FilteredDisplay>().SetFilter(filters[i + 1]);
            }

            GetComponentInChildren<EnergyGauge>().Buffer = target.GetComponent<EnergyBuffer>();
            base.Init(target, inventory);
        }

        private void Update()
        {
            progressBar.UpdatePercentage(assembler.CurrentProgress, assembler.ProgressPerBuild, "Build Progress: {0}%");
        }

    }
}
