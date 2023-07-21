using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        private Slider slider;
        private TextMeshProUGUI text;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void UpdateAppearance(HP hp)
        {
            int maxHP = hp.MaxHealth;
            if (maxHP <= 0)
            {
                maxHP = 1;
            }

            slider.maxValue = maxHP;
            slider.value = hp.Health;

            text.text = GetHPText(hp.Health, maxHP);
        }

        private string GetHPText(int hp, int maxHP)
        {
            return "HP: " + hp + "/" + maxHP;
        }
    }
}
