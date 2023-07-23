using UnityEngine;

namespace Scavenger.UI
{
    public class InputModeUI : MonoBehaviour
    {
        [SerializeField] private RectTransform selectionIndicator;
        [SerializeField] private GameManager gameManager;

        private void Start()
        {
            UpdateAppearance();
        }

        public void ChangeMode(bool interact)
        {
            InputMode mode = interact ? InputMode.Interact : InputMode.Edit;
            gameManager.InputMode = mode;
            UpdateAppearance();
        }

        private void UpdateAppearance()
        {
            selectionIndicator.anchoredPosition = new Vector2(0, -55 * (int)gameManager.InputMode);
        }
    }
}
