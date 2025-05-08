using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterMovementController : CharacterBase
    {
        [SerializeField] CharacterData data;
        [SerializeField] HeadBobData bobData;
        [SerializeField, Parent] CharacterCollisionController collisionController;
        [SerializeField, Child] CharacterCameraController cameraController;
        [SerializeField, Self] CharacterController controller;
        [SerializeField] Transform yawTransform;
        IPlayerMapInput inputProvider;
        HeadBob headBob;

        public CharacterCollisionController CollisionController => collisionController;
        public MovementEffectsHandler MovementEffects { get; private set; }
        public CharacterMovementHandler MovementHandler { get; private set; }
        public CharacterGravityHandler GravityHandler { get; private set; }
        public CharacterJumpHandler JumpHandler { get; private set; }
        public CharacterCrouchHandler CrouchHandler { get; private set; }
        public CharacterLandHandler LandHandler { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            AssignInputs();
            InitComponents();
            SetupStateMachine();
        }

        void AssignInputs()
        {
            inputProvider = new PlayerMapInputProvider();
            inputProvider.MoveActionSetup();
            inputProvider.SprintActionSetup();
            inputProvider.JumpActionSetup();
        }

        void InitComponents()
        {
            LandHandler = new CharacterLandHandler(data, yawTransform);
            headBob = new HeadBob(data, bobData);
            CrouchHandler = new CharacterCrouchHandler(data, controller, yawTransform, headBob, LandHandler);
            MovementHandler = new CharacterMovementHandler(inputProvider, data, controller, transform, yawTransform);
            MovementEffects = new MovementEffectsHandler(
                cameraController, headBob, data, yawTransform, MovementHandler, inputProvider, CrouchHandler);
            GravityHandler = new CharacterGravityHandler(data, controller);
            JumpHandler = new CharacterJumpHandler(data, CrouchHandler, GravityHandler);
        }

        void SetupStateMachine()
        {
            var grounded = new CharacterGroundedState(this);
            var falling = new CharacterFallingState(this);

            At(grounded, falling, () =>
                !collisionController.GroundChecker.IsGrounded &&
                controller.velocity.y < 0f);
            At(falling, grounded, () =>
                collisionController.GroundChecker.IsGrounded);

            stateMachine.SetState(falling);
        }

        void OnEnable()
        {
            inputProvider.IsSprinting.started += SprintInput;
            inputProvider.IsSprinting.performed += SprintInput;
            inputProvider.IsSprinting.canceled += SprintInput;

            inputProvider.JumpPressed.started += JumpInput;

            inputProvider.CrouchPressed.started += CrouchInput;
        }

        void OnDisable()
        {
            inputProvider.IsSprinting.started -= SprintInput;
            inputProvider.IsSprinting.performed -= SprintInput;
            inputProvider.IsSprinting.canceled -= SprintInput;

            inputProvider.JumpPressed.started -= JumpInput;

            inputProvider.CrouchPressed.started -= CrouchInput;
        }

        void SprintInput(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    MovementHandler.ApplySprint(true);
                    break;
                case InputActionPhase.Performed:
                    MovementHandler.ApplySprint(true);
                    break;
                case InputActionPhase.Canceled:
                    MovementHandler.ApplySprint(false);
                    break;
            }
        }

        void JumpInput(InputAction.CallbackContext context)
        {
            if (context.phase is InputActionPhase.Started)
                JumpHandler.ApplyJump(collisionController);
        }

        void CrouchInput(InputAction.CallbackContext context)
        {
            if (context.phase is InputActionPhase.Started)
                CrouchHandler.HandleCrouch(this, collisionController);
        }
    }
}