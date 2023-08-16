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

        private const string defaultRatioFormat = "{0}/{1}";
        private const string defaultPercentFormat = "{0}%";

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
        /// <param name="textFormat">The format for the progress bar's label.</param>
        public void UpdateRatio(int value, int maxValue, string textFormat = defaultRatioFormat)
        {
            SetValues(value, maxValue);
            text.text = string.Format(textFormat, value, maxValue);
        }

        // TODO add docs
        public void UpdatePercentage(int value, int maxValue, string textFormat = defaultPercentFormat)
        {
            SetValues(value, maxValue);

            int percentage = value * 100 / maxValue;
            text.text = string.Format(textFormat, percentage);

        }

        // TODO add docs
        private void SetValues(int value, int maxValue)
        {
            if (maxValue <= 0)
            {
                maxValue = 1;
            }

            slider.maxValue = maxValue;
            slider.value = value;
        }
    }
}
