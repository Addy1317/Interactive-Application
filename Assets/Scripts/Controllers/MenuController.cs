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
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private void OnEnable ()
        {
            startButton.onClick.AddListener(OnStartButton);
            quitButton.onClick.AddListener(OnQuitButton);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveListener(OnStartButton);
            quitButton.onClick.RemoveListener(OnQuitButton);
        }

        private void OnStartButton()
        {
            SceneManager.LoadScene("Main");
            Debug.Log("Main Scene Active");
        }

        private void OnQuitButton()
        {
            Application.Quit();
            Debug.Log("Application Quit");
        }

    }
}