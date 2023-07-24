using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    public abstract class Interactibe : ItemProperty
    {
        public abstract void Interact(GameManager gameManager, int inventorySlot, Vector2Int pressedPos);
    }
}
