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

        protected override void Awake()
        {
            base.Awake();
            AssignInputs();
            InitComponents();
            SetupStateMachine();
        }

        void AssignInputs()
        {
            InputProvider = new PlayerMapInputProvider();
            InputProvider.MoveActionSetup();
            InputProvider.SprintActionSetup();
            InputProvider.JumpActionSetup();
        }

        void InitComponents()
        {
            GroundChecker = new CharacterGroundChecker(data, controller);
            GravityHandler = new CharacterGravityHandler(data, controller, GroundChecker);
            MovementHandler = new CharacterMovementHandler(InputProvider, data, controller, transform);
            rigidBodyPush = new CharacterRigidBodyPushHandler(data);
        }

        void SetupStateMachine()
        {
            var grounded = new CharacterGroundedState(this);
            var falling = new CharacterFallingState(this);

            At(grounded, falling, () => !GroundChecker.IsGrounded);
            At(falling, grounded, () => GroundChecker.IsGrounded);

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
            var spherePos = new Vector3(
                transform.position.x,
                transform.position.y - data.groundedOffset,
                transform.position.z
            );

            Gizmos.color = GroundChecker.IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(spherePos, data.groundedRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                spherePos,
                spherePos + Vector3.down * data.surfaceCheckDistance
            );

            if (GroundChecker.IsGrounded)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GroundChecker.IsHit.point, 0.05f);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(
                    GroundChecker.IsHit.point,
                    GroundChecker.IsHit.normal * 0.5f
                );

                Gizmos.color = Color.white;
                Gizmos.DrawLine(spherePos, GroundChecker.IsHit.point);
            }
        }
    }
}
//FIXME: Finished check controller charcter