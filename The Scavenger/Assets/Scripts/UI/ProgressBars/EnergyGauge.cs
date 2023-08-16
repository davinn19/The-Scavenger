using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    [RequireComponent(typeof(ProgressBar))]
    public class EnergyGauge : MonoBehaviour
    {
        public EnergyBuffer Buffer { get; set; }
        private ProgressBar progressBar;
        private const string textFormat = "Energy: {0}/{1} J";

        private void Awake()
        {
            progressBar = GetComponent<ProgressBar>();
        }

        private void Update()
        {
            progressBar.UpdateRatio(Buffer.Energy, Buffer.Capacity, textFormat);
        }
    }
}
