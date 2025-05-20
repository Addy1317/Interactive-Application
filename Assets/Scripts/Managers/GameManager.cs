using UnityEngine;

namespace IA
{
    public enum ModelType
    {
        Car,
        Plane
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject carObject;
        [SerializeField] private GameObject planeObject;

        private void Start()
        {
            carObject.SetActive(false);
            planeObject.SetActive(false);
            Init();
        }

        private void Init()
        {
            if (GameService.Instance == null || GameService.Instance.eventManager == null)
            {
                Debug.LogError("❌ GameService or EventManager is not initialized!");
                return;
            }
            GameService.Instance.eventManager.OnModelSelectedEvent.AddListeners(HandleModelSelection);
        }

        private void OnDisable()
        {
            GameService.Instance.eventManager.OnModelSelectedEvent.RemoveListeners(HandleModelSelection);
        }

        private void HandleModelSelection(ModelType modelType)
        {
            Debug.Log($"Model Selected: {modelType}");

            switch (modelType)
            {
                case ModelType.Car:
                    carObject.SetActive(true);
                    planeObject.SetActive(false);
                    break;
                case ModelType.Plane:
                    carObject.SetActive(false);
                    planeObject.SetActive(true);
                    break;
                default:
                    Debug.LogWarning("Unhandled model type: " + modelType);
                    break;
            }
        }
    }
}
