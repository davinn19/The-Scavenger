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

        private Queue<Action> callbackQueue = new();


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
                ExecuteCallbacks();
            }
        }

        // TODO remove queue system and scan the gridmap instead
        private void ExecuteCallbacks()
        {
            int queueSize = callbackQueue.Count;
            for (int _ = 0; _ < queueSize; _++)
            {
                Action nextCallback = callbackQueue.Dequeue();
                nextCallback.Invoke();
            }
        }

        public void QueueUpdate(Action callback)
        {
            callbackQueue.Enqueue(callback);
        }
    }
}
