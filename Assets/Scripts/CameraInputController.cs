using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Cinemachine;

namespace RPGGame
{
    /// <summary>
    /// Mobile-safe camera look controller.
    /// Rotates the camera ONLY when the player slides a finger that did NOT
    /// start on a UI element (joystick, attack button, etc.).
    /// Works exactly like PUBG's look zone.
    /// </summary>
    public class CameraInputController : MonoBehaviour
    {
        [Header("Cinemachine")]
        [SerializeField] private CinemachineCamera followCamera;

        [Header("Sensitivity")]
        [SerializeField] private float sensitivityX = 0.25f;
        [SerializeField] private float sensitivityY = 0.15f;

        // ── internal state ──────────────────────────────────────────────
        private CinemachineOrbitalFollow   _orbital;   // Cinemachine 3 orbiting body
        private CinemachineInputAxisController _axisCtrl; // we'll disable its auto-read
        private int   _camFingerID  = -1;   // which finger owns the camera look
        private bool  _isEditor;

        // ── lifecycle ───────────────────────────────────────────────────
        void Awake()
        {
            // KEY FIX #1 — stop Unity from turning every tap into a mouse event.
            // Without this, tapping the attack button moves Mouse X/Y → Cinemachine spins.
            Input.simulateMouseWithTouches = false;

            _orbital  = followCamera.GetComponent<CinemachineOrbitalFollow>();
            _axisCtrl = followCamera.GetComponent<CinemachineInputAxisController>();

            // KEY FIX #2 — disable Cinemachine's built-in axis reader so it no longer
            // pulls from Input.GetAxis("Mouse X/Y") automatically on mobile.
            if (_axisCtrl != null && !Application.isEditor)
                _axisCtrl.enabled = false;

            _isEditor = Application.isEditor;
        }

        void Update()
        {
            if (_isEditor)
                HandleEditorMouse();   // keep right-click-drag for editor testing
            else
                HandleMobileTouch();   // proper mobile path
        }

        // ── editor helper (right-click to look, same as before) ─────────
        private void HandleEditorMouse()
        {
            if (_axisCtrl != null) _axisCtrl.enabled = Input.GetMouseButton(1);

            // Fallback: drive orbital directly if no axis controller
            if (_axisCtrl == null && Input.GetMouseButton(1))
            {
                ApplyDelta(Input.GetAxis("Mouse X") * 200f * Time.deltaTime,
                           Input.GetAxis("Mouse Y") * 200f * Time.deltaTime);
            }
        }

        // ── mobile core ─────────────────────────────────────────────────
        private void HandleMobileTouch()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);

                // --- Finger just pressed down ---
                if (t.phase == TouchPhase.Began)
                {
                    // Only claim this finger for camera if:
                    //   • no camera finger is claimed yet
                    //   • the touch did NOT land on any UI element
                    if (_camFingerID == -1 && !IsOverUI(t.fingerId))
                    {
                        _camFingerID = t.fingerId;
                    }
                    continue;
                }

                // Ignore fingers that aren't the camera finger
                if (t.fingerId != _camFingerID) continue;

                // --- Camera finger moved → rotate ---
                if (t.phase == TouchPhase.Moved)
                {
                    ApplyDelta(t.deltaPosition.x * sensitivityX,
                               t.deltaPosition.y * sensitivityY);
                }

                // --- Camera finger lifted → release ---
                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    _camFingerID = -1;
                }
            }
        }

        // ── apply rotation to CinemachineOrbitalFollow axes ─────────────
        private void ApplyDelta(float dx, float dy)
        {
            if (_orbital == null) return;

            _orbital.HorizontalAxis.Value += dx;   // left / right look
            _orbital.VerticalAxis.Value   -= dy;   // up / down look (inverted: swipe up = look up)
        }

        // ── UI hit-test ──────────────────────────────────────────────────
        private static bool IsOverUI(int fingerId)
        {
            return EventSystem.current != null
                && EventSystem.current.IsPointerOverGameObject(fingerId);
        }
    }
}