using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    [CreateAssetMenu(fileName = "RecyclerRecipe", menuName = "Scavenger/Recipe/Recycler")]
    public class RecyclerRecipe : ScriptableObject
    {
        public Item input;
        public List<RecyclerDrop> output;
    }

    [System.Serializable]
    public class RecyclerDrop
    {
        public Item item;
        [Min(1)] public int amount = 1;
        [Range(0, 1)] public float chance;
    }
}
