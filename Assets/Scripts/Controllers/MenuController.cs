#region
///<summary>
/// Menu Script attact on the Menu GameObject
/// 
///</summary>
#endregion
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IA.Menu
{
    public class MenuController : MonoBehaviour
    {
        [Header("Home Panel")]
        [SerializeField] private GameObject homePanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button setttingsButton;
        [SerializeField] private Button quitButton;

        [Header("Settings Panel")]
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private Button settingscloseButton;

        [Header("Audio")]
        [SerializeField] private AudioManager audioManager;

        private void OnEnable ()
        {
            startButton.onClick.AddListener(OnStartButton);
            setttingsButton.onClick.AddListener(OnSettingsButton);
            quitButton.onClick.AddListener(OnQuitButton);
            
            settingscloseButton.onClick.AddListener(OnSettingsCloseButton);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartButton);
            setttingsButton.onClick.RemoveListener(OnSettingsButton);
            quitButton.onClick.RemoveListener(OnQuitButton);

            settingscloseButton.onClick.AddListener(OnSettingsCloseButton);
        }

        #region Home Panel Methods
        private void OnStartButton()
        {
            SceneManager.LoadScene("Main");
            audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            Debug.Log("Main Scene Active");
        }

        private void OnSettingsButton()
        {
            settingPanel.SetActive(true);
            audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            Debug.Log("Setting Panel Active");
        }

        private void OnQuitButton()
        {
            Application.Quit();
            audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            Debug.Log("Application Quit");
        }
        #endregion

        #region Setting Panel Methods

        private void OnSettingsCloseButton()
        {
            settingPanel.SetActive(false);
            audioManager.PlaySFX(SFXType.OnButtonClickSFX);
        }
        #endregion
    }
}