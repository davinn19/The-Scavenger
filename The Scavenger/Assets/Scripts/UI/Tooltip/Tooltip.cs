using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scavenger.UI
{
    // TODO implement, add docs
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI description;
        private GameObject content;
        private RectTransform rectTransform;
        private GameUI gameUI;

        private TooltipDefinition m_tooltipDefinition;
        private TooltipDefinition tooltipDefinition
        {
            get { return m_tooltipDefinition; }
            set
            {
                if (value == m_tooltipDefinition)
                {
                    return;
                }

                if (m_tooltipDefinition != null)
                {
                    m_tooltipDefinition.TargetChanged -= OnTargetChanged;
                    m_tooltipDefinition = null;
                }

                if (value != null)
                {
                    m_tooltipDefinition = value;
                    m_tooltipDefinition.TargetChanged += OnTargetChanged;
                }

                UpdateAppearance();
            }
        }

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
        /// Starts tracking the newly hovered element with a tooltip, or turns invisible if it does not exist.
        /// </summary>
        /// <param name="hoveredElement">The hovered UI element.</param>
        private void OnHoveredElementChanged(GameObject hoveredElement)
        {
            if (hoveredElement == null || !hoveredElement.TryGetComponent(out TooltipDefinition tooltipDefinition))
            {
                this.tooltipDefinition = null;
                return;
            }

            this.tooltipDefinition = tooltipDefinition;
        }

        /// <summary>
        /// Invokes an apperance update.
        /// </summary>
        /// <param name="slot"></param>
        private void OnTargetChanged()
        {
            if (tooltipDefinition)
            {
                UpdateAppearance();
            }
        }

        /// <summary>
        /// Draws a new tooltip based on the tracked tooltip definition.
        /// </summary>
        private void UpdateAppearance()
        {
            // TODO implement, add comments
            ClearCustomContent();

            if (!tooltipDefinition || !tooltipDefinition.IsVisible())
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);

            SetHeader();
            SetDescription();
            SetCustomContent();
        }

        /// <summary>
        /// Writes the header based on the tooltip definition.
        /// </summary>
        private void SetHeader()
        {
            header.text = tooltipDefinition.GetHeader();
            description.gameObject.SetActive(header.text != "");
        }

        /// <summary>
        /// Writes the description based on the tooltip definition.
        /// </summary>
        private void SetDescription()
        {
            description.text = tooltipDefinition.GetDescription();
            description.gameObject.SetActive(description.text != "");
        }

        /// <summary>
        /// Draws the custom tooltip content, if it has one.
        /// </summary>
        private void SetCustomContent()
        {
            content = tooltipDefinition.GetCustomContent();
            if (content)
            {
                content.transform.SetParent(transform, false);
            }
        }

        /// <summary>
        /// Erases any existing custom tooltip content.
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
