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
        private HealthBar healthBar;

        private void Awake()
        {
            background = GetComponent<Image>();
            healthBar = GetComponentInChildren<HealthBar>();
        }

        void Update()
        {
            // TODO link to event instead
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

                healthBar.UpdateAppearance(gridObject.HP);
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
        
    }
}
