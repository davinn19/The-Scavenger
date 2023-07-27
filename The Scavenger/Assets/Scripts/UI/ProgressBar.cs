using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    /// <summary>
    /// UI element for a progress bar.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class ProgressBar : MonoBehaviour
    {
        private Slider slider;
        private TextMeshProUGUI text;

        public string Prefix { get; set; }
        public string Suffix { get; set; }

        private void Awake()
        {
            slider = GetComponent<Slider>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Moves the slider and updates the labels based on the new value/max value.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <param name="maxValue">The new max value.</param>
        public void UpdateAppearance(int value, int maxValue)
        {
            if (maxValue <= 0)
            {
                maxValue = 1;
            }

            slider.maxValue = maxValue;
            slider.value = value;

            text.text = GetText(value, maxValue);
        }

        /// <summary>
        /// Gets the text for the progress bar's label.
        /// </summary>
        /// <param name="value">The current value.</param>
        /// <param name="maxValue">The current max value.</param>
        /// <returns>Formatted string with the new values.</returns>
        private string GetText(int value, int maxValue)
        {
            return Prefix + value + "/" + maxValue + Suffix;
        }
    }
}
