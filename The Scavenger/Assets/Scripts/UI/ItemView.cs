using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;
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
            rectTransform = transform as RectTransform;

            gameUI = GetComponentInParent<GameUI>();
            gameUI.HoveredElementChanged += OnHoveredElementChanged;
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            rectTransform.position = gameUI.PointerPos + Vector2.one * 10;
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
            ClearCustomContent();

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

            SetDescription(itemStack);
            SetCustomContent(itemStack);
        }

        private void SetDescription(ItemStack itemStack)
        {
            itemDescription.text = itemStack.Item.Description;
            itemDescription.gameObject.SetActive(itemDescription.text != "");
        }

        private void SetCustomContent(ItemStack itemStack)
        {
            if (itemStack.Item.TryGetProperty(out CustomUI ui))
            {
                content = Instantiate(ui.UI, transform);
                content.Init(itemStack.Item);
            }
        }

        private void ClearCustomContent()
        {
            if (content)
            {
                Destroy(content.gameObject);
                content = null;
            }
        }
    }
}
