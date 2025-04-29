using UnityEngine;

namespace CatchFire
{
    public class CharacterCameraController : MonoBehaviour
    {
        [SerializeField] GameObject cameraTarget;
        [SerializeField] bool lockCameraPosition = false;
        [SerializeField] float cameraAngleOverride = 0f;
        [SerializeField] float topClamp = 70f;
        [SerializeField] float bottomClamp = -30f;
        float targetYaw;
        float targetPitch;
        const float threshold = 0.01f;
        InputProvider inputProvider;

        void Awake()
        {
            AssignInput();
            AssignVars();
        }

        void AssignInput()
        {
            inputProvider = new InputProvider();
            inputProvider.LookActionSetup();
        }

        void AssignVars() => targetYaw = cameraTarget.transform.rotation.eulerAngles.y;

        void LateUpdate()
        {
            CameraRotation();
            CameraAim();
        }

        void CameraRotation()
        {
            if (inputProvider.LookInput.sqrMagnitude >= threshold && !lockCameraPosition)
            {
                targetYaw += inputProvider.LookInput.x;
                targetPitch += inputProvider.LookInput.y;
            }

            targetYaw = ClampAngle(targetYaw, float.MinValue, float.MaxValue);
            targetPitch = ClampAngle(targetPitch, bottomClamp, topClamp);

            cameraTarget.transform.rotation = Quaternion.Euler(targetPitch + cameraAngleOverride, targetYaw, 0f);
        }

        static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        void CameraAim()
        {
            //TODO: Make Aim
        }
    }
}