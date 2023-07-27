using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Scavenger.UI.UIContent;

namespace Scavenger.UI
{
    /// <summary>
    /// Displays information on the current inspected grid object.
    /// </summary>
    public class GridObjectInspectorUI : MonoBehaviour
    {
        [SerializeField] private GridObjectInspector gridObjectInspector;
        [SerializeField] private TextMeshProUGUI gridObjectName;
        [SerializeField] private Image gridObjectIcon;

        private Image background;
        private ProgressBar healthBar;

        private GridObjectUIContent content = null;

        private void Awake()
        {
            background = GetComponent<Image>();
            healthBar = GetComponentInChildren<ProgressBar>();
            healthBar.Prefix = "HP: ";

            gridObjectInspector.InspectedObjectChanged += UpdateAppearance;

            ShowUI(false);
        }

        /// <summary>
        /// Updates the UI based on the new inspected object.
        /// </summary>
        private void UpdateAppearance()
        {
            GridObject gridObject = gridObjectInspector.GetInspectedObject();

            if (!gridObject)
            {
                ShowUI(false);
            }
            else
            {
                ShowUI(true);

                gridObjectIcon.sprite = gridObject.GetComponent<SpriteRenderer>().sprite;
                gridObjectName.text = gridObject.name;
                healthBar.UpdateAppearance(gridObject.HP.Health, gridObject.HP.MaxHealth);

                SetContent(gridObject);
            }
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

            background.enabled = active;
        }

        /// <summary>
        /// Sets the custom content of the UI.
        /// </summary>
        /// <param name="viewedObject"></param>
        private void SetContent(GridObject viewedObject)
        {
            // Removes old content
            if (content)
            {
                Destroy(content.gameObject);
            }


            if (viewedObject.UIContent == null)
            {
                content = null;
            }
            else
            {
                content = Instantiate(viewedObject.UIContent, transform);
                content.Init(viewedObject);
            }
        }
    }
}
