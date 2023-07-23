using UnityEngine;
using UnityEngine.Search;

namespace Scavenger
{
    public class PlacedObject : ItemProperty
    {
        public override string Name => "Placed Object";

        [field: SerializeField, SearchContext("dir:Assets/Prefabs/GridObjects t:GridObject")] public GridObject Object { get; private set; }
    }
}
