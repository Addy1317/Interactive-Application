using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace IA
{
    public class UIManager : MonoBehaviour
    {
        [Header("Selection Panel")]
        [SerializeField] internal GameObject modeselectionPanel;
        [SerializeField] private Button carButton;
        [SerializeField] private Button planeButton;

        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button settingsCloseButton;

        [Header("Model Panel")]
        [SerializeField] internal GameObject modelPanel;

        [Header("Model Info Display")]
        [SerializeField] private TextMeshProUGUI modelNameText;

        [Header("Info Panel")]
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private Button infoPanelButton;

        [Header("Home Button")]
        [SerializeField] private Button homeButton;
        [SerializeField] private Button backButton;

        private void Awake()
        {
            modeselectionPanel.SetActive(true);
            settingsPanel.SetActive(false);
            modelPanel.SetActive(false);
        }
        private void Start()
        {
            GameService.Instance.eventManager.OnModelSelectedEvent.AddListeners(UpdateModelNameText);
        }

        private void OnEnable()
        {
            carButton.onClick.AddListener(OnCarButton);
            planeButton.onClick.AddListener(OnplaneButton);

            settingsButton.onClick.AddListener(OnSettingsPanelButton);
            settingsCloseButton.onClick.AddListener(OnSettingsClosePanelButton);

            infoPanelButton.onClick.AddListener(OnInfoPanelButton);
            homeButton.onClick.AddListener(OnHomeButton);
            backButton.onClick.AddListener(OnBackButton);
        }

        private void OnDisable()
        {
            carButton.onClick.RemoveListener(OnCarButton);
            planeButton.onClick.RemoveListener(OnplaneButton);

            settingsButton.onClick.RemoveListener(OnSettingsPanelButton);
            settingsCloseButton.onClick.RemoveListener(OnSettingsClosePanelButton);

            GameService.Instance.eventManager.OnModelSelectedEvent.RemoveListeners(UpdateModelNameText);

            infoPanelButton.onClick.RemoveListener(OnInfoPanelButton);
            homeButton.onClick.RemoveListener(OnHomeButton);
            backButton.onClick.RemoveListener(OnBackButton);
        }


        #region SelectionPanel Methods

        private void OnCarButton()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            GameService.Instance.eventManager.OnModelSelectedEvent.InvokeEvents(ModelType.Car);

            modelPanel.SetActive(true);
            modeselectionPanel.SetActive(false);
            //car.SetActive(true);
            Debug.Log("Car Panel Active");
        }

        private void OnplaneButton()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            GameService.Instance.eventManager.OnModelSelectedEvent.InvokeEvents(ModelType.Plane);

            modelPanel.SetActive(true);
            modeselectionPanel.SetActive(false);
            Debug.Log("Plane Panel Active");
        }

        private void UpdateModelNameText(ModelType model)
        {
            modelNameText.text = $"{model}";
        }
        #endregion

        #region SettingPanelMethods

        private void OnSettingsPanelButton()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive);
            Debug.Log("Opneing Settings Panel");
        }

        private void OnSettingsClosePanelButton()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            settingsPanel.SetActive(false);
            Debug.Log("Closing Settings Panel");
        }
        #endregion

        #region Info Panel Methods
        private void OnInfoPanelButton()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            bool isActive = infoPanel.activeSelf;
            infoPanel.SetActive(!isActive);
        }

        private void OnHomeButton()
        {
            SceneManager.LoadScene("Menu");
        }

        private void OnBackButton()
        {
            modelPanel.SetActive(false);
            modeselectionPanel.SetActive(true);
        }
        #endregion

    }
}
