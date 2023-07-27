using Scavenger.UI;
using UnityEngine;
using UnityEngine.Search;
using Scavenger.UI.UIContent;

namespace Scavenger
{
    /// <summary>
    /// Gives items a custom tooltip UI.
    /// </summary>
    public class CustomUI : ItemProperty
    {
        public override string Name => "Custom UI";

        [field: SerializeField, SearchContext("dir:Assets/Prefabs/UI/ItemUI t:ItemUIContent")] public ItemUIContent UI { get; private set; }
    }
}
