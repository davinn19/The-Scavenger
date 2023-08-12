using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Denotes an item as being able to handle interation with the map.
    /// </summary>
    public abstract class Interactibe : ItemProperty
    {
        /// <summary>
        /// Leads the interaction with the gridMap.
        /// </summary>
        /// <param name="gameManager">The current game manager.</param>
        /// <param name="pressedPos">The grid position pressed.</param>
        public abstract void Interact(GameManager gameManager, Vector2Int pressedPos);
    }
}
