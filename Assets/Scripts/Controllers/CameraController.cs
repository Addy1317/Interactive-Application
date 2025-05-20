using UnityEngine;

namespace IA
{
    public class CameraController : MonoBehaviour
    {
        [Header("Transform Reference")]
        [SerializeField] private Transform targetObject;
        [SerializeField] private Transform cameraTransform;

        [Header("Zoom Settings")]
        [SerializeField] internal float distance = 5f;
        [SerializeField] internal float zoomSpeed = 10f;
        [SerializeField] internal float minZoom = 2f;
        [SerializeField] internal float maxZoom = 20f;
        [SerializeField] internal float zoomStep = 2f;

        [Header("Orbit Settings")]
        [SerializeField] private float orbitSpeed = 5f;
        [SerializeField] private float yaw = 0f;
        [SerializeField] private float pitch = 20f;
        [SerializeField] private float minPitch = -40f;
        [SerializeField] private float maxPitch = 80f;

        [Header("Pan Settings")]
        [SerializeField] private float panSpeed = 0.005f;

        private Vector3 lastMousePosition;

        private void LateUpdate()
        {
            if (targetObject == null || cameraTransform == null) return;

            HandleOrbit();
            HandleZoom();
            HandlePan();

            UpdateCameraPosition();
        }

        private void HandleOrbit()
        {
            if (Input.GetMouseButton(0))
            {
                yaw += Input.GetAxis("Mouse X") * orbitSpeed;
                pitch -= Input.GetAxis("Mouse Y") * orbitSpeed;
                pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            }

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            cameraTransform.position = targetObject.position + offset;
            cameraTransform.LookAt(targetObject);
        }

        private void HandleZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                distance -= scroll * zoomSpeed;
                distance = Mathf.Clamp(distance, minZoom, maxZoom);
            }
        }

        private void HandlePan()
        {
            if (Input.GetMouseButton(2))
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 move = -transform.right * delta.x * panSpeed * Time.deltaTime
                             - transform.up * delta.y * panSpeed * Time.deltaTime;

                transform.position += move;
            }

            lastMousePosition = Input.mousePosition;
        }

        private void UpdateCameraPosition()
        {
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 offset = rotation * new Vector3(0, 0, -distance);
            cameraTransform.position = transform.position + offset;
            cameraTransform.LookAt(transform.position);
        }

        public void FocusOnPart(Transform part)
        {
            transform.position = part.position;
        }

    }
}