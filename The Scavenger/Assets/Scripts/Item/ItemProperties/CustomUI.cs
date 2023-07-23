using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scavenger.UI;
using UnityEngine.Search;

namespace Scavenger
{
    public class CustomUI : ItemProperty
    {
        public override string Name => "Custom UI";

        [field: SerializeField, SearchContext("dir:Assets/Prefabs/UI/ItemUI t:ItemUIContent")] public ItemUIContent UI { get; private set; }
    }
}
