using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterMovementController : CharacterBase
    {
        [SerializeField] CharacterData data;
        [SerializeField] CharacterController controller;
        IRigidBodyPush rigidBodyPush;

        public IPlayerMapInput InputProvider { get; private set; }
        public IGroundedCheckable GroundChecker { get; private set; }
        public IMovable MovementHandler { get; private set; }
        public ICharacterGravity GravityHandler { get; private set; }
        IInputServices services;

        protected override void Awake()
        {
            base.Awake();
            AssignInputs();
            InitComponents();
            SetupStateMachine();
        }

        void AssignInputs()
        {
            services = new InputServicesProvider();
            InputProvider = new PlayerMapInputProvider();
            InputProvider.MoveActionSetup();
            InputProvider.SprintActionSetup();
            InputProvider.JumpActionSetup();
        }

        void InitComponents()
        {
            GroundChecker = new CharacterGroundChecker(data, transform);
            GravityHandler = new CharacterGravityHandler(data, controller, GroundChecker);
            MovementHandler = new CharacterMovementHandler(
                InputProvider, services, data, controller, transform, Camera.main);
            rigidBodyPush = new CharacterRigidBodyPushHandler(data);
        }

        void SetupStateMachine()
        {
            var grounded = new CharacterGroundedState(this);
            var falling = new CharacterFallingState(this);
            var jumping = new CharacterJumpingState(this);

            At(grounded, falling, () => !GroundChecker.Grounded);
            At(jumping, falling, () => !GroundChecker.Grounded && GravityHandler.VerticalVelocity < -0.1f);
            At(falling, grounded, () => GroundChecker.Grounded);

            stateMachine.SetState(falling);
        }

        void OnEnable()
        {
            InputProvider.IsSprinting.started += SprintInput;
            InputProvider.IsSprinting.performed += SprintInput;
            InputProvider.IsSprinting.canceled += SprintInput;
        }

        void OnDisable()
        {
            InputProvider.IsSprinting.started -= SprintInput;
            InputProvider.IsSprinting.performed -= SprintInput;
            InputProvider.IsSprinting.canceled -= SprintInput;
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

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (data.canPush)
                rigidBodyPush.PushRigidBodies(hit);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = GroundChecker.Grounded ? Color.green : Color.red;

            var pos = new Vector3(
                transform.position.x,
                transform.position.y - data.groundedOffset,
                transform.position.z);

            Gizmos.DrawSphere(pos, data.groundedRadius);
        }
    }
}