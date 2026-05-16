using UnityEngine;
using Unity.Cinemachine;

namespace RPGGame
{
    public class CameraInputController : MonoBehaviour
    {
        // In Cinemachine 3, the FreeLook component is often referenced as CinemachineCamera
        // or CinemachineFreeLook if you are using the compatibility wrapper.
        [SerializeField] private CinemachineCamera followCamera;
        private CinemachineInputAxisController inputAxisController;

        void Start()
        {
            if (followCamera != null)
            {
                inputAxisController = followCamera.GetComponent<CinemachineInputAxisController>();
            }
        }

        void Update()
        {
            if (Input.GetMouseButton(1)) // Detect Right Click
            {
                if (inputAxisController != null) inputAxisController.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                if (inputAxisController != null) inputAxisController.enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}