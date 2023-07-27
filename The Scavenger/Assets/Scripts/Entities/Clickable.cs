using System;
using UnityEngine;

namespace Scavenger
{
    /// <summary>
    /// Allows an object outside the gridMap to be detected by mouse clicks. Mostly used on entities.
    /// </summary>
    public class Clickable : MonoBehaviour
    {
        public event Action<GameManager> Clicked;

        public void OnClick(GameManager gameManager)
        {
            Clicked.Invoke(gameManager);
        }
    }
}
