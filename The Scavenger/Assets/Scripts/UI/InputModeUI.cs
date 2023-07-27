using UnityEngine;

namespace Scavenger.UI
{
    /// <summary>
    /// Controls UI for the player's current input mode.
    /// </summary>
    public class InputModeUI : MonoBehaviour
    {
        [SerializeField] private RectTransform selectionIndicator;
        [SerializeField] private GameManager gameManager;
        private InputHandler inputHandler;

        private void Awake()
        {
            inputHandler = gameManager.InputHandler;
            inputHandler.InputModeChanged += MoveSelectionIndicator;
        }

        private void Start()
        {
            MoveSelectionIndicator(0);
        }

        /// <summary>
        /// Changes the selected input mode.
        /// </summary>
        /// <param name="mode">The new input mode.</param>
        public void ChangeMode(int mode)
        {
            inputHandler.InputMode = (InputMode)mode;
        }

        /// <summary>
        /// Highlights the button representing the selected input mode.
        /// </summary>
        /// <param name="inputMode">The selected input mode.</param>
        private void MoveSelectionIndicator(InputMode inputMode)
        {
            int index = (int)inputMode;
            selectionIndicator.anchoredPosition = 55 * new Vector2(index % 2, -index / 2);
        }
    }
}
