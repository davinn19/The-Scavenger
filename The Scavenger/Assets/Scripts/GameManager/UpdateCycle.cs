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

        private readonly Queue<Action> callbackQueue = new();
        private readonly LinkedList<Action> callbacks = new();

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
                ExecuteCallbacks();
            }
        }

        private void ExecuteCallbacks()
        {
            LinkedListNode<Action> node = callbacks.First;


            for (; node != null; node = node.Next)
            {
                //TODO implement, add docs
            }


            while (node != null)
            {
                if (node.Value == null)
                {

                }
            }


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
