using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class UpdateCycle : MonoBehaviour
    {
        [SerializeField]
        [Min(1)]
        private float updatesPerSecond;
        private float secondsPerUpdate;

        private float secondsSinceUpdate;

        private List<Action> callbacks = new List<Action>();


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

                foreach (Action callback in callbacks)
                {
                    callback.Invoke();
                }
            }
        }


        public void Subscribe(Action callback)
        {
            callbacks.Add(callback);
        }

        private void Unsubscribe(Action callback)
        {
            callbacks.Remove(callback);
        }
    }
}
