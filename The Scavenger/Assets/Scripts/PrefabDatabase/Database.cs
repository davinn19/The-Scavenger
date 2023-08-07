using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    // TODO add docs
    public class Database : MonoBehaviour
    {
        public static Database Instance { get; private set; }

        [field: SerializeField] public ItemDatabase Item { get; private set; }
        [field: SerializeField] public ModelDatabase Model { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
