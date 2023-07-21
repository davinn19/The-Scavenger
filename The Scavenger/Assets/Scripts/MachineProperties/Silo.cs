using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    [RequireComponent(typeof(ItemBuffer))]
    public class Silo : MonoBehaviour
    {
        private SpriteRenderer icon;
        public ItemBuffer ItemBuffer { get; private set; }


        private void Awake()
        {
            ItemBuffer = GetComponent<ItemBuffer>();
            icon = GetComponent<SpriteRenderer>();
        }

        

    }
}
