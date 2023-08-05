using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.GridObjectBehaviors
{
    // TODO add docs, implement

    [RequireComponent(typeof(EnergyBuffer))]
    public class EnergyCell : GridObjectBehavior
    {
        protected EnergyBuffer energyBuffer;

        [SerializeField] private int maxInsertLimit;
        [SerializeField] private int maxExtractLimit;   // TODO resolve throughput limit between energy cell and energy interface

        [SerializeField] public int insertLimit;
        [SerializeField] public int extractLimit;

        // TODO add docs
        protected override void Init()
        {
            base.Init();
            energyBuffer = GetComponent<EnergyBuffer>();
        }
    }
}
