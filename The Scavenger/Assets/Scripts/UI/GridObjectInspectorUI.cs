using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scavenger.UI.InspectorContent;
using UnityEngine.Localization.Components;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays information on the current inspected grid object.
    /// </summary>
    public class GridObjectInspectorUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        private GridObjectInspector gridObjectInspector;
        private PlayerInventory inventory;

        [SerializeField] private LocalizeStringEvent gridObjectName;
        [SerializeField] private Image gridObjectIcon;
        private HPGauge hpBar;

        private GridObjectInspectorContent content = null;

        private void Awake()
        {
            hpBar = GetComponentInChildren<HPGauge>();

            gridObjectInspector = gameManager.GridObjectInspector;
            gridObjectInspector.InspectedObjectChanged += UpdateAppearance;

            inventory = gameManager.Inventory;
        }

        private void Start()
        {
            ShowUI(false);
        }

        /// <summary>
        /// Updates the UI based on the new inspected object.
        /// </summary>
        private void UpdateAppearance()
        {
            if (!gridObjectInspector.InspectEnabled)
            {
                ShowUI(false);
                return;
            }

            GridObject gridObject = gridObjectInspector.GetInspectedObject();
            if (!gridObject)
            {
                ShowUI(false);
                return;
            }

            ShowUI(true);

            gridObjectIcon.sprite = gridObject.GetModel().GetThumbnail();
            gridObjectName.StringReference = gridObject.DisplayName;
            hpBar.HP = gridObject.HP;

            SetContent(gridObject);
        }

        /// <summary>
        /// Shows or hides the UI.
        /// </summary>
        /// <param name="active">If true, shows the UI.</param>
        private void ShowUI(bool active)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.SetActive(active);
            }
        }

        /// <summary>
        /// Sets the custom content of the UI.
        /// </summary>
        /// <param name="viewedObject"></param>
        private void SetContent(GridObject viewedObject)
        {
            ClearContent();

            GridObjectInspectorContent gridObjectContent = viewedObject.InspectorContent;
            if (gridObjectContent)
            {
                content = Instantiate(gridObjectContent, transform);
                content.Init(viewedObject, inventory);
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
            }
        }

        // TODO add docs
        private void ClearContent()
        {
            // Removes old content
            if (content)
            {
                Destroy(content.gameObject);
                content = null;
            }
        }
    }
}
