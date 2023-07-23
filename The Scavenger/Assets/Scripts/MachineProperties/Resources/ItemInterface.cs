using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ItemInterface : ConduitInterface<ItemBuffer>
    {
        [SerializeField] private List<ItemBuffer> inputs;
        [SerializeField] private List<ItemBuffer> outputs;

        public override List<ItemBuffer> GetInputs()
        {
            return new List<ItemBuffer>(inputs);
        }

        public override List<ItemBuffer> GetOutputs()
        {
            return new List<ItemBuffer>(outputs);
        }
    }
}
