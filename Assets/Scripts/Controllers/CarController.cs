using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IA
{
    public class CarController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject carBody;
        [SerializeField] private GameObject[] carwheels;
        [SerializeField] private CameraController cameraController;

        [Header("Buttons")]
        [SerializeField] private Button carBodyButton;
        [SerializeField] private Button carWheelsButton;
        [SerializeField] private Button carAllPartsButton;
        [SerializeField] private Button carPanelCloseButton;

        [Header("Outline Shaders")]
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color selectionColor = Color.green;

        private bool bodyVisible = true;
        private bool wheelsVisible = true;

        private GameObject currentlyHovered;
        private GameObject currentlySelected;

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

        private void Start()
        {
            carBody.SetActive(false);
            bodyVisible = false;

            foreach (var wheel in carwheels)
            {
                if (wheel != null)
                    wheel.SetActive(false);
            }
            wheelsVisible = false;
        }

        private void Update()
        {
            HandleHover();
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
            foreach (var wheel in carwheels)
            {
                if (wheel != null)
                    wheel.SetActive(wheelsVisible);
            }

            if (wheelsVisible)
                cameraController.FocusOnPart(carwheels[0].transform); // Focus on one wheel
        }

        private void OnCarAllPartsButton()
        {
            bool showAll = !bodyVisible || !wheelsVisible;

            bodyVisible = showAll;
            wheelsVisible = showAll;

            carBody.SetActive(bodyVisible);
            foreach (var wheel in carwheels)
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
                    SelectPart(clickedObject);

                    if (clickedObject == carBody || System.Array.Exists(carwheels, wheel => wheel == clickedObject))
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
            else if (System.Array.Exists(carwheels, wheel => wheel == clickedObject))
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

        #region Outline Shader
        private void HandleHover()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;

                // Skip if same as before
                if (hitObj == currentlyHovered || hitObj == currentlySelected)
                    return;

                ClearHighlight();

                // Highlight new part
                Outline outline = hitObj.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                    outline.OutlineColor = highlightColor;
                    outline.OutlineWidth = 5f;
                    currentlyHovered = hitObj;
                }
            }
            else
            {
                ClearHighlight();
            }
        }

        private void ClearHighlight()
        {
            if (currentlyHovered != null && currentlyHovered != currentlySelected)
            {
                Outline outline = currentlyHovered.GetComponent<Outline>();
                if (outline != null)
                    outline.enabled = false;
            }

            currentlyHovered = null;
        }

        private void SelectPart(GameObject part)
        {
            if (currentlySelected != null)
            {
                Outline prev = currentlySelected.GetComponent<Outline>();
                if (prev != null)
                    prev.enabled = false;
            }

            Outline newOutline = part.GetComponent<Outline>();
            if (newOutline != null)
            {
                newOutline.enabled = true;
                newOutline.OutlineColor = selectionColor;
                newOutline.OutlineWidth = 8f;
            }

            currentlySelected = part;
        }
        #endregion
    }
}
