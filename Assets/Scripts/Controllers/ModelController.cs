using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace IA
{
    public class ModelController : MonoBehaviour
    {
        [Header("Model Settings")]
        [SerializeField] private string modelName; 
        [SerializeField] private GameObject[] modelParts;

        [Header("UI References")]
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private TMP_Text selectedPartText;
        [SerializeField] private Button panelCloseButton;

        [Header("Camera")]
        [SerializeField] private CameraController cameraController;

        [Header("Outline Shader")]
        [SerializeField] private Color highlightColor = Color.yellow;
        [SerializeField] private Color selectionColor = Color.green;

        [SerializeField] private GraphicRaycaster uiRaycaster;
        [SerializeField] private EventSystem eventSystem;

        private GameObject currentlyHovered;
        private GameObject currentlySelected;

        #region Unity Methods

        private void OnEnable()
        {
            GeneratePartButtons();
            HideAllParts();
        }

        private void Update()
        {
            HandleHover();
            HandleLeftClickOnPart();
            HandleRightClickOnPart();
        }

        #endregion

        #region Dynamic Button Generation

        private void GeneratePartButtons()
        {
            foreach (Transform child in buttonContainer)
                Destroy(child.gameObject);

            foreach (GameObject part in modelParts)
            {
                if (part == null) continue;

                GameObject buttonGO = Instantiate(buttonPrefab, buttonContainer);
                TMP_Text label = buttonGO.GetComponentInChildren<TMP_Text>();
                label.text = part.name;

                Button button = buttonGO.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    TogglePartVisibility(part);
                    SelectPart(part);
                    UpdateSelectedText(part.name);
                });
            }
        }

        private void UpdateSelectedText(string partName)
        {
            if (selectedPartText != null)
                selectedPartText.text = $"{partName}";
        }

        #endregion

        #region Part Handling

        private void TogglePartVisibility(GameObject part)
        {
            bool newState = !part.activeSelf;
            part.SetActive(newState);

            if (newState && cameraController != null)
                cameraController.FocusOnPart(part.transform);
        }

        private void HideAllParts()
        {
            foreach (GameObject part in modelParts)
            {
                if (part != null)
                    part.SetActive(false);
            }
        }

        #endregion

        #region Input Handlers

        private void HandleLeftClickOnPart()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (!System.Array.Exists(modelParts, part => part == clickedObject)) return;

                bool newState = !clickedObject.activeSelf;
                clickedObject.SetActive(newState);

                if (newState)
                {
                    SelectPart(clickedObject);
                    UpdateSelectedText(clickedObject.name);
                }
                else
                {
                    ClearSelectedPart(clickedObject);
                    UpdateSelectedText("None");
                }
            }
        }

        private void HandleRightClickOnPart()
        {
            if (!Input.GetMouseButtonDown(1)) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                if (!System.Array.Exists(modelParts, part => part == clickedObject)) return;
                if (!clickedObject.activeSelf) return;

                SelectPart(clickedObject);
                FocusZoomOnPart(clickedObject.transform);
            }
        }

        #endregion

        #region Outline Shader Logic

        private void HandleHover()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject hitObj = hit.collider.gameObject;

                if (hitObj == currentlyHovered || hitObj == currentlySelected)
                    return;

                ClearHighlight();

                if (System.Array.Exists(modelParts, part => part == hitObj))
                {
                    Outline outline = hitObj.GetComponent<Outline>();
                    if (outline != null)
                    {
                        outline.enabled = true;
                        outline.OutlineColor = highlightColor;
                        outline.OutlineWidth = 5f;
                        currentlyHovered = hitObj;
                    }
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
                if (currentlyHovered.activeInHierarchy)
                {
                    Outline outline = currentlyHovered.GetComponent<Outline>();
                    if (outline != null)
                        outline.enabled = false;
                }
            }

            currentlyHovered = null;
        }

        private void ClearSelectedPart(GameObject part)
        {
            if (currentlySelected != part) return;

            Outline outline = part.GetComponent<Outline>();
            if (outline != null)
                outline.enabled = false;

            currentlySelected = null;
        }

        private void SelectPart(GameObject part)
        {
            if (currentlySelected != null && currentlySelected != part)
            {
                Outline prevOutline = currentlySelected.GetComponent<Outline>();
                if (prevOutline != null) prevOutline.enabled = false;
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

        private void FocusZoomOnPart(Transform part)
        {
            if (cameraController == null) return;

            cameraController.FocusOnPart(part);

            // Zoom in slightly
            cameraController.distance -= cameraController.zoomStep;
            cameraController.distance = Mathf.Clamp(cameraController.distance, cameraController.minZoom, cameraController.maxZoom);
        }

        #endregion

        private bool IsPointerOverUIElement()
        {
            PointerEventData eventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventData, results);

            return results.Count > 0;
        }
    }
}
