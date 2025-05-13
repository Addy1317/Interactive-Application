using Unity.Properties;
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
        [SerializeField] private Button carPanelCloseButton;
        private void OnEnable()
        {
            carButton.onClick.AddListener(OnCarButton);
            planeButton.onClick.AddListener(OnplaneButton);
        }

        private void OnDisable()
        {
            carButton.onClick.RemoveListener(OnCarButton);
            planeButton.onClick.RemoveListener(OnplaneButton);
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

    }
}
