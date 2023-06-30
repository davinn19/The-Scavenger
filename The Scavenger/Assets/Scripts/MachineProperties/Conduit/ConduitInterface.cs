using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public class ConduitInterface : MonoBehaviour
    {
        [SerializeField] private List<ItemBuffer> inputs;
        [SerializeField] private List<ItemBuffer> outputs;
    }

}
