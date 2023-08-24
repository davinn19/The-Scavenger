using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO add docs
    public class CategoryItemStackDisplayTimer : MonoBehaviour
    {
        private const int timeBetweenSwitches = 1;
        private float timeSinceLastSwitch = 0;
        public event Action SwitchTime;

        private void Update()
        {
            timeSinceLastSwitch += Time.deltaTime;
            if (timeSinceLastSwitch >= timeBetweenSwitches)
            {
                timeSinceLastSwitch = 0;
                SwitchTime?.Invoke();
            }
        }
    }
}
