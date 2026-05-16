using UnityEngine;

namespace RPGGame
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        public float distance = 2f;
        public float height = 1.2f;

        public float rotationSpeed = 3f;
        public float smoothSpeed = 5f;

        public float minPitch = -20f;
        public float maxPitch = 45f;

        private float currentYaw = 0f;
        private float currentPitch = 15f;

        private float smoothYaw;
        private float smoothPitch;

        void LateUpdate()
        {
            if (!target)
                return;

            // Rotate ONLY while right click held
            if (Input.GetMouseButton(1))
            {
                currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;

                currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

                // Limit vertical angle
                currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
            }

            // Smooth rotations
            smoothYaw = Mathf.LerpAngle(
                smoothYaw,
                currentYaw,
                smoothSpeed * Time.deltaTime);

            smoothPitch = Mathf.LerpAngle(
                smoothPitch,
                currentPitch,
                smoothSpeed * Time.deltaTime);

            // Final rotation
            Quaternion rotation = Quaternion.Euler(
                smoothPitch,
                smoothYaw,
                0);

            // Offset position
            Vector3 offset = rotation * new Vector3(0, 0, -distance);

            // Camera position
            transform.position =
                target.position +
                Vector3.up * height +
                offset;

            // Look at player
            transform.LookAt(target.position + Vector3.up * 2f);
        }
    }
}