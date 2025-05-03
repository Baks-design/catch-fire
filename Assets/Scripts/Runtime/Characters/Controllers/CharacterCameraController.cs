using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterCameraController : MonoBehaviour
    {
        [SerializeField] CharacterData characterData;
        [SerializeField] GameObject cameraTarget;
        IPlayerMapInput inputProvider;
        float targetPitch;
        float rotationVelocity;

        void Awake()
        {
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
                    break;
                case InputActionPhase.Performed:
                    break;
                case InputActionPhase.Canceled:
                    break;
            }
        }

        void LateUpdate()
        {
            if (inputProvider.LookInput.sqrMagnitude < 0.01f) return;

            targetPitch += inputProvider.LookInput.y * characterData.rotationSpeed;
            rotationVelocity = inputProvider.LookInput.x * characterData.rotationSpeed;

            targetPitch = Maths.ClampAngle(targetPitch, characterData.bottomClamp, characterData.topClamp);

            cameraTarget.transform.localRotation = Quaternion.Euler(targetPitch, 0f, 0f);

            transform.Rotate(Vector3.up * rotationVelocity);
        }
    }
}