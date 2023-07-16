using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add documentation, test
    public class DataInterface : ConduitInterface<DataBuffer>
    {
        public List<DataBuffer> inputs;
        public List<DataBuffer> outputs;

        public override List<DataBuffer> GetInputs()
        {
            return inputs;
        }

        public override List<DataBuffer> GetOutputs()
        {
            return outputs;
        }
    }
}
