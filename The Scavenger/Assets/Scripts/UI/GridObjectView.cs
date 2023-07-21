using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scavenger.UI
{
    public class GridObjectView : MonoBehaviour
    {
        [SerializeField] private GridObjectViewer gridObjectViewer;
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

            gridObjectViewer.ViewedObjectChanged += OnViewedObjectChanged;
        }

        private void OnViewedObjectChanged()
        {
            GridObject gridObject = gridObjectViewer.GetViewedObject();

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

        private void ShowUI(bool active)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.SetActive(active);
            }

            background.enabled = active;
        }

        private void SetContent(GridObject viewedObject)
        {
            // Remove old content
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
