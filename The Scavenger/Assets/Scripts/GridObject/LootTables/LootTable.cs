using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public abstract class LootTable : MonoBehaviour
    {
        public abstract List<ItemStack> GetDrops();
    }
}
