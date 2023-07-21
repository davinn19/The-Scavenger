using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
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

        private string GetText(int value, int maxValue) 
        {
            return Prefix + value + "/" + maxValue + Suffix;
        }
    }
}
