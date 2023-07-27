using TMPro;
using UnityEngine;
using Scavenger.UI.UIContent;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays information on the itemStack in a hovered slot display.
    /// </summary>
    public class ItemTooltip : MonoBehaviour
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
                if (value == m_hoveredDisplay)
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

        /// <summary>
        /// Moves the tooltip to where the pointer is.
        /// </summary>
        private void Update()
        {
            rectTransform.position = gameUI.PointerPos + new Vector2(0, 10);
        }

        /// <summary>
        /// Starts tracking the newly hovered slotDisplay, or turns invisible if a slotDisplay is not hovered over.
        /// </summary>
        /// <param name="hoveredElement">The hovered display.</param>
        private void OnHoveredElementChanged(GameObject hoveredElement)
        {
            if (hoveredElement == null || !hoveredElement.TryGetComponent(out ClickableSlotDisplay clickedDisplay))
            {
                HoveredDisplay = null;
                return;
            }

            HoveredDisplay = clickedDisplay.SlotDisplay;
        }

        /// <summary>
        /// Invokes an update when the slot display's contents changes.
        /// </summary>
        /// <param name="slot"></param>
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

        /// <summary>
        /// Draws the tooltip to represent the currently tracked slot display.
        /// </summary>
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

        /// <summary>
        /// Writes the description based on the itemStack's item.
        /// </summary>
        /// <param name="itemStack">The current itemStack.</param>
        private void SetDescription(ItemStack itemStack)
        {
            itemDescription.text = itemStack.Item.Description;
            itemDescription.gameObject.SetActive(itemDescription.text != "");
        }

        /// <summary>
        /// Draws the itemStack's custom tooltip UI, it it has one.
        /// </summary>
        /// <param name="itemStack">The current itemStack.</param>
        private void SetCustomContent(ItemStack itemStack)
        {
            if (itemStack.Item.TryGetProperty(out CustomUI ui))
            {
                content = Instantiate(ui.UI, transform);
                content.Init(itemStack.Item);
            }
        }

        /// <summary>
        /// Erases any existing custom UI.
        /// </summary>
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
