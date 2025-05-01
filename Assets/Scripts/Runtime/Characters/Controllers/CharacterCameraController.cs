using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterCameraController : MonoBehaviour
    {
        [SerializeField] CinemachineCamera aimCamera;
        [SerializeField] GameObject cameraTarget;
        [SerializeField] bool lockCameraPosition = false;
        [SerializeField] float cameraAngleOverride = 0f;
        [SerializeField] float topClamp = 70f;
        [SerializeField] float bottomClamp = -30f;
        float targetYaw;
        float targetPitch;
        const float threshold = 0.01f;
        IPlayerMapInput inputProvider;
        IInputServices inputServices;

        void Awake()
        {
            AssignVars();
            AssignInput();
        }

        void AssignVars() => targetYaw = cameraTarget.transform.rotation.eulerAngles.y;

        void AssignInput()
        {
            inputServices = new InputServicesProvider();
            inputProvider = new PlayerMapInputProvider();
            inputProvider.LookActionSetup();
            inputProvider.AimActionSetup();
        }

        void OnEnable()
        {
            inputProvider.IsAiming.started += CameraAim;
            inputProvider.IsAiming.performed += CameraAim;
            inputProvider.IsAiming.canceled += CameraAim;
        }

        void OnDisable()
        {
            inputProvider.IsAiming.started -= CameraAim;
            inputProvider.IsAiming.performed -= CameraAim;
            inputProvider.IsAiming.canceled -= CameraAim;
        }

        void CameraAim(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    SetCameraPriority(aimCamera, 10);
                    break;
                case InputActionPhase.Performed:
                    SetCameraPriority(aimCamera, 10);
                    break;
                case InputActionPhase.Canceled:
                    SetCameraPriority(aimCamera, 0);
                    break;
            }
        }

        void SetCameraPriority(CinemachineCamera cinemachine, int priority)
        => cinemachine.Priority = priority;

        void LateUpdate() => CameraRotation();

        void CameraRotation()
        {
            if (inputProvider.LookInput.sqrMagnitude >= threshold && !lockCameraPosition)
            {
                var deltaTimeMultiplier = inputServices.IsUsingMouse ? 1f : Time.deltaTime;
                targetYaw += inputProvider.LookInput.x * deltaTimeMultiplier;
                targetPitch += inputProvider.LookInput.y * deltaTimeMultiplier;
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
    }
}