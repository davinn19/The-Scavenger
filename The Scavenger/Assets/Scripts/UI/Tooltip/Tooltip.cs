using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Scavenger.UI
{
    // TODO implement, add docs
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent headerPrefab;
        [SerializeField] private LocalizeStringEvent textPrefab;

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
                    m_tooltipDefinition.ClearTooltip(this);
                    m_tooltipDefinition = null;
                }

                if (value != null)
                {
                    m_tooltipDefinition = value;
                    m_tooltipDefinition.RenderTooltip(this);
                    m_tooltipDefinition.TargetChanged += OnTargetChanged;
                }
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
            Clear();
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
            }
            else
            {
                this.tooltipDefinition = tooltipDefinition;
            }
        }

        /// <summary>
        /// Invokes an apperance update.
        /// </summary>
        /// <param name="slot"></param>
        private void OnTargetChanged()
        {
            if (tooltipDefinition != null)
            {
                tooltipDefinition.ClearTooltip(this);
                tooltipDefinition.RenderTooltip(this);
            }
        }

        public void AddHeader(LocalizedString header)
        {
            gameObject.SetActive(true);
            LocalizeStringEvent newHeader = Instantiate(headerPrefab, transform);
            newHeader.StringReference = header;
        }

        public void AddText(LocalizedString text)
        {
            gameObject.SetActive(true);
            LocalizeStringEvent newText = Instantiate(textPrefab, transform);
            newText.StringReference = text;
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}
