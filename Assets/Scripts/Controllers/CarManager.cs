using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IA
{
    public class CarManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject carBody;
        [SerializeField] private GameObject[] wheels;
        [SerializeField] private CameraController cameraController;

        [Header("Buttons")]
        [SerializeField] private Button carBodyButton;
        [SerializeField] private Button carWheelsButton;
        [SerializeField] private Button carAllPartsButton;
        [SerializeField] private Button carPanelCloseButton;

        private bool bodyVisible = true;
        private bool wheelsVisible = true;

        #region Unity Methods
        private void OnEnable()
        {
            carBodyButton.onClick.AddListener(OnCarBodyButton);
            carWheelsButton.onClick.AddListener(OnCarWheelButtons);
            carAllPartsButton.onClick.AddListener(OnCarAllPartsButton);
            carPanelCloseButton.onClick.AddListener(OnCarPanelCloseButton);
        }

        private void OnDisable()
        {
            carBodyButton.onClick.RemoveListener(OnCarBodyButton);
            carWheelsButton.onClick.RemoveListener(OnCarWheelButtons);
            carAllPartsButton.onClick.RemoveListener(OnCarAllPartsButton);
            carPanelCloseButton.onClick.RemoveListener(OnCarPanelCloseButton);
        }

        private void Update()
        {
            HandleClickOnPart();
            HandleClickZoom();
        }
        #endregion

        #region Car buttons Methods
        private void OnCarBodyButton()
        {
            bodyVisible = !bodyVisible;
            carBody.SetActive(bodyVisible);

            if (bodyVisible)
                cameraController.FocusOnPart(carBody.transform);
        }

        private void OnCarWheelButtons()
        {
            wheelsVisible = !wheelsVisible;
            foreach (var wheel in wheels)
            {
                if (wheel != null)
                    wheel.SetActive(wheelsVisible);
            }

            if (wheelsVisible)
                cameraController.FocusOnPart(wheels[0].transform); // Focus on one wheel
        }

        private void OnCarAllPartsButton()
        {
            bool showAll = !bodyVisible || !wheelsVisible;

            bodyVisible = showAll;
            wheelsVisible = showAll;

            carBody.SetActive(bodyVisible);
            foreach (var wheel in wheels)
            {
                if (wheel != null)
                    wheel.SetActive(wheelsVisible);
            }

            if (showAll)
                cameraController.FocusOnPart(carBody.transform);
        }

        private void OnCarPanelCloseButton()
        {
            this.gameObject.SetActive(false);
            GameService.Instance.uiManager.carPanel.SetActive(false);
            GameService.Instance.uiManager.selectionPanel.SetActive(true);
        }

        #endregion

        #region Clicks On Parts Methods

        private void HandleClickOnPart()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    OnCarPartClicked(clickedObject);
                }
            }
        }

        private void HandleClickZoom()
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedObject = hit.collider.gameObject;

                    if (clickedObject == carBody || System.Array.Exists(wheels, wheel => wheel == clickedObject))
                    {
                        FocusZoomOnPart(clickedObject.transform);
                    }
                }
            }
        }

        public void OnCarPartClicked(GameObject clickedObject)
        {
            if (clickedObject == carBody)
            {
                Debug.Log("Clicked on Body");
                OnCarBodyButton();
            }
            else if (System.Array.Exists(wheels, wheel => wheel == clickedObject))
            {
                Debug.Log("Clicked on Wheel");

                OnCarWheelButtons();
            }
        }

        public void FocusZoomOnPart(Transform part)
        {
            if (cameraController == null) return;

            // Set camera pivot to part
            cameraController.FocusOnPart(part);

            // Slight zoom-in (reduce distance)
            cameraController.distance -= cameraController.zoomStep;
            cameraController.distance = Mathf.Clamp(cameraController.distance, cameraController.minZoom, cameraController.maxZoom);
        }
        #endregion
    }
}
