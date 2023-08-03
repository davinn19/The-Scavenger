using UnityEngine;
using UnityEngine.Search;

namespace Scavenger
{
    /// <summary>
    /// Specifies what gridObject an item places down.
    /// </summary>
    public class PlacedObject : ItemProperty
    {
        public override string Name => "Placed Object";

        [field: SerializeField, SearchContext("p: t:GridObject")] public GridObject Object { get; private set; }
    }
}
