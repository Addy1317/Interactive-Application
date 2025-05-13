using UnityEngine;
using UnityEngine.UI;

namespace IA
{
    public class UIManager : MonoBehaviour
    {
        [Header("SelectionPanel")]
        [SerializeField] internal GameObject selectionPanel;
        [SerializeField] private Button carButton;
        [SerializeField] private Button planeButton;

        [Header("Car Panel")]
        [SerializeField] internal GameObject carPanel;
        [SerializeField] private GameObject car;

        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button infoPanelButton;

        private void OnEnable()
        {
            carButton.onClick.AddListener(OnCarButton);
            planeButton.onClick.AddListener(OnplaneButton);
            infoPanelButton.onClick.AddListener(OnInfoPanelButton);
        }

        private void OnDisable()
        {
            carButton.onClick.RemoveListener(OnCarButton);
            planeButton.onClick.RemoveListener(OnplaneButton);
            infoPanelButton.onClick.RemoveListener(OnInfoPanelButton);
        }

        #region SelectionPanel Methods

        private void OnCarButton()
        {
            carPanel.SetActive(true);
            selectionPanel.SetActive(false);
            car.SetActive(true);
            Debug.Log("Car Panel Active");
        }

        private void OnplaneButton()
        {
            selectionPanel.SetActive(false);
            Debug.Log("Plane Panel Active");
        }

        #endregion

        #region Info Panel Methods
        private void OnInfoPanelButton()
        {
            bool isActive = infoPanel.activeSelf;
            infoPanel.SetActive(!isActive);
        }
        #endregion

    }
}
