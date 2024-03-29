using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    [RequireComponent(typeof(ProgressBar))]
    public class HPGauge : MonoBehaviour
    {
        public HP HP { get; set; }
        private ProgressBar progressBar;
        private const string textFormat = "HP: {0}/{1}";

        private void Awake()
        {
            progressBar = GetComponent<ProgressBar>();
        }

        private void Update()
        {
            if (HP)
            {
                progressBar.UpdateRatio(HP.Health, HP.GetMaxHP(), textFormat);
            }
            else
            {
                progressBar.UpdateRatio(0, 1, textFormat);
            }    
            
        }
    }
}
