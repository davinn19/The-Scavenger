using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scavenger.GridObjectBehaviors;

namespace Scavenger
{
    // TODO add docs
    public class StaticModel : Model
    {
        [SerializeField] private Sprite sprite;
        private SpriteRenderer spriteRenderer;

        public override void Load(GridObjectBehavior behavior)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

        }

        public override void UpdateAppearance()
        {
            spriteRenderer.sprite = sprite;
        }
    }
}
