using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    public class ItemView : MonoBehaviour
    {
        private TextMeshProUGUI itemName;
        private ItemUIContent content;
        private RectTransform rectTransform;

        private SlotDisplay m_hoveredDisplay;
        private SlotDisplay HoveredDisplay
        {
            get { return m_hoveredDisplay; }
            set
            {
                if (value == HoveredDisplay)
                {
                    return;
                }

                if (m_hoveredDisplay != null)
                {
                    m_hoveredDisplay.Buffer.SlotChanged -= OnSlotChanged;
                    m_hoveredDisplay = null;
                }

                if (value != null)
                {
                    m_hoveredDisplay = value;
                    HoveredDisplay.Buffer.SlotChanged += OnSlotChanged;
                }

                UpdateAppearance();
            }
        }

        private GameUI gameUI;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            itemName = GetComponentInChildren<TextMeshProUGUI>();
            gameUI = GetComponentInParent<GameUI>();
            gameUI.HoveredElementChanged += OnHoveredElementChanged;
        }

        private void Update()
        {
            rectTransform.position = gameUI.MousePos + Vector2.one * 10;
        }


        private void OnHoveredElementChanged(GameObject hoveredElement)
        {
            if (hoveredElement == null)
            {
                HoveredDisplay = null;
                return;
            }

            HoveredDisplay = hoveredElement.GetComponent<SlotDisplay>();
        }


        private void OnSlotChanged(int slot)
        {
            if (HoveredDisplay == null)
            {
                return;
            }

            if (HoveredDisplay.Slot == slot)
            {
                UpdateAppearance();
            }
        }

        private void UpdateAppearance()
        {
            ClearContent();
            itemName.text = "";

            if (!HoveredDisplay)
            {
                gameObject.SetActive(false);
                return;
            }

            ItemStack itemStack = HoveredDisplay.GetItemInSlot();
            if (!itemStack)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            itemName.text = itemStack.Item.DisplayName;
            SetContent(itemStack);
        }


        private void SetContent(ItemStack itemStack)
        {
            if (itemStack.Item.TryGetProperty(out CustomUI ui))
            {
                content = Instantiate(ui.UI, transform);
                content.Init(itemStack.Item);
            }
        }

        private void ClearContent()
        {
            if (content)
            {
                Destroy(content.gameObject);
                content = null;
            }
        }
    }
}
