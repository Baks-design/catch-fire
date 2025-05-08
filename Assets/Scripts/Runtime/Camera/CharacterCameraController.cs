using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterCameraController : MonoBehaviour
    {
        [SerializeField] CharacterData data;
        [SerializeField, Child] CinemachineCamera cinemachine;
        [SerializeField, Anywhere] Transform yawTransform;
        [SerializeField, Anywhere] Transform pitchTransform;
        PlayerMapInputProvider inputProvider;

        public CameraRotation CameraRotation { get; private set; }
        public CameraSway CameraSway { get; private set; }
        public CameraZoom CameraZoom { get; private set; }
        public CameraBreath CameraBreath { get; private set; }

        void Awake()
        {
            InitInputs();
            InitClasses();
        }

        void InitInputs()
        {
            inputProvider = new PlayerMapInputProvider();
            inputProvider.LookActionSetup();
            inputProvider.AimActionSetup();
        }

        void InitClasses()
        {
            CameraRotation = new CameraRotation(data, inputProvider, yawTransform, pitchTransform);
            CameraSway = new CameraSway(data, Camera.main.transform);
            CameraZoom = new CameraZoom(data, cinemachine);
            CameraBreath = new CameraBreath(data, cinemachine.transform);
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
            if (context.phase is InputActionPhase.Performed ||
                context.phase is InputActionPhase.Canceled)
                CameraZoom.ChangeFOV(this);
        }

        void LateUpdate()
        {
            CameraRotation.HandleRotation();
            CameraBreath.ApplyBreathing();
        }

        public void HandleSway(Vector3 inputVector, float rawXInput)
        => CameraSway.ApplySway(inputVector, rawXInput);

        public void ChangeRunFOV(bool returning)
        => CameraZoom.ChangeRunFOV(returning, this);
    }
}