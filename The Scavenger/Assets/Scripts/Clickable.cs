using System;
using UnityEngine;

namespace Scavenger
{
    public class Clickable : MonoBehaviour
    {
        public event Action<GameManager> Clicked;

        public void OnClick(GameManager gameManager)
        {
            Clicked.Invoke(gameManager);
        }
    }
}
