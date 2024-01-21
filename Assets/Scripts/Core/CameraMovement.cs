using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Cinemachine;
using RPG.Utils;

namespace RPG.Core
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera = null;
        [SerializeField] private float cameraZoomSpeed = 6f;
        [SerializeField] private float cameraRotationSpeed = 6f;
        [SerializeField] private float edgeSize = 10f;
        [Range(0, 1)]
        [SerializeField] private float outOfScreenRotationPersentageSpeed = 0.9f;
        private CinemachineComponentBase componentBase;
        private readonly Timer mouseDoubleClickTimer = new Timer();
        private float defaultCameraDistance;
        private Vector3 defaultCameraRotation;
        private Dictionary<object, bool> openUIElements = new Dictionary<object, bool>();

        public void UIElementStatus(object uiElement, bool status) => openUIElements[uiElement] = status;

        private void Awake()
        {
            if (virtualCamera) componentBase = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

            mouseDoubleClickTimer.Interval = 300;
            mouseDoubleClickTimer.Elapsed += (sender, e) => mouseDoubleClickTimer.Stop();

            defaultCameraDistance = (componentBase as CinemachineFramingTransposer).m_CameraDistance;
            defaultCameraRotation = virtualCamera.transform.eulerAngles;
        }

        private void LateUpdate()
        {
            if (openUIElements.ContainsValue(true)) return;

            if (componentBase is CinemachineFramingTransposer)
            {
                if ((componentBase as CinemachineFramingTransposer).m_CameraDistance.IsBetweenRange(4, 10))
                    (componentBase as CinemachineFramingTransposer).m_CameraDistance += Input.mouseScrollDelta.y * Time.deltaTime * cameraZoomSpeed * 5f;
                else if ((componentBase as CinemachineFramingTransposer).m_CameraDistance < 4) (componentBase as CinemachineFramingTransposer).m_CameraDistance = 4;
                else if ((componentBase as CinemachineFramingTransposer).m_CameraDistance > 10) (componentBase as CinemachineFramingTransposer).m_CameraDistance = 10;

                if (Input.GetMouseButtonDown(1))
                {
                    if (!mouseDoubleClickTimer.Enabled) mouseDoubleClickTimer.Start();
                    else
                    {
                        (componentBase as CinemachineFramingTransposer).m_CameraDistance = defaultCameraDistance;
                        virtualCamera.transform.eulerAngles = defaultCameraRotation;
                        mouseDoubleClickTimer.Stop();
                    }

                    return;
                }

                if (Input.GetMouseButton(1))
                {
                    RotateCamera(-Input.GetAxis("Mouse X") * Time.deltaTime * cameraRotationSpeed * 3f);

                    if (Input.mousePosition.x > Screen.width - edgeSize) RotateCamera(-outOfScreenRotationPersentageSpeed * Time.deltaTime * cameraRotationSpeed);

                    if (Input.mousePosition.x < edgeSize) RotateCamera(outOfScreenRotationPersentageSpeed * Time.deltaTime * cameraRotationSpeed);
                }
            }
        }

        private void RotateCamera(float rotateY)
        {
            virtualCamera.gameObject.transform.eulerAngles = new Vector3(virtualCamera.transform.eulerAngles.x,
            virtualCamera.transform.eulerAngles.y + rotateY,
            virtualCamera.transform.rotation.z);
        }
    }
}