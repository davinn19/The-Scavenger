using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class UpdateCycle : MonoBehaviour
    {
        [SerializeField]
        [Min(1)]
        private float updatesPerSecond;
        private float secondsPerUpdate;

        private float secondsSinceUpdate;

        public event Action TickUpdate;


        private void Awake()
        {
            secondsPerUpdate = 1 / updatesPerSecond;
            secondsSinceUpdate = 0;
        }

        private void Update()
        {
            secondsSinceUpdate += Time.deltaTime;
            if (secondsSinceUpdate >= secondsPerUpdate)
            {
                secondsSinceUpdate = 0;
                TickUpdate?.Invoke();
            }
        }
    }
}
