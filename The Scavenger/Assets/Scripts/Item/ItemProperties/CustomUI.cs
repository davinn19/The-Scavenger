using Scavenger.UI;
using UnityEngine;
using UnityEngine.Search;
using Scavenger.UI.InspectorContent;

namespace Scavenger
{
    /// <summary>
    /// Gives items a custom tooltip UI.
    /// </summary>
    public class CustomUI : ItemProperty    // TODO separate item properties into different namespace
    {
        public override string Name => "Custom UI";

        [field: SerializeField, SearchContext("dir:Assets/Prefabs/UI/ItemUI t:ItemUIContent")] public ItemUIContent UI { get; private set; }
    }
}
