using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharacterMovementController : CharacterBase
    {
        [SerializeField] CharacterData data;
        [SerializeField] HeadBobData bobData;
        [SerializeField] CharacterCameraController cameraController;
        [SerializeField] CharacterController controller;
        [SerializeField] Transform yawTransform;
        IRigidBodyPush rigidBodyPush;

        public IMovementEffects MovementEffects { get; private set; }
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
            MovementHandler = new CharacterMovementHandler(
                InputProvider, data, controller, transform, yawTransform);
            GroundChecker = new CharacterGroundChecker(
                data, controller, transform, InputProvider, MovementHandler);
            GravityHandler = new CharacterGravityHandler(data, controller);
            rigidBodyPush = new CharacterRigidBodyPushHandler(data);
            MovementEffects = new MovementEffectsHandler(
                cameraController, bobData, data, yawTransform,
                MovementHandler, InputProvider);
        }

        void SetupStateMachine()
        {
            var grounded = new CharacterGroundedState(this);
            var falling = new CharacterFallingState(this);

            At(grounded, falling, () => !GroundChecker.IsGrounded && controller.velocity.y < 0f);
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
            if (!Application.isPlaying) return;

            //GROUND CHECK
            var spherePos = new Vector3(
                transform.position.x,
                transform.position.y - data.groundedOffset,
                transform.position.z
            );

            Gizmos.color = GroundChecker.IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(spherePos, data.groundedRadius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(spherePos, spherePos + Vector3.down * data.surfaceCheckDistance);

            if (GroundChecker.IsGrounded)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(GroundChecker.IsGroundHit.point, 0.05f);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(GroundChecker.IsGroundHit.point, GroundChecker.IsGroundHit.normal * 0.5f);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(spherePos, GroundChecker.IsGroundHit.point);
            }

            //OBSTACLE CHECK
            var origin = transform.position + controller.center;
            var direction = MovementHandler.FinalMoveDir.normalized;
            var maxDistance = data.rayObstacleLength;

            Gizmos.color = GroundChecker.IsHitWall ? Color.red : Color.green;
            Gizmos.DrawWireSphere(origin, data.rayObstacleSphereRadius);

            if (!GroundChecker.IsHitWall)
                Gizmos.DrawWireSphere(origin + direction * maxDistance, data.rayObstacleSphereRadius);
            else
                Gizmos.DrawWireSphere(origin + direction * GroundChecker.IsObstacleHit.distance, data.rayObstacleSphereRadius);

            Gizmos.DrawLine(origin, origin + direction * (GroundChecker.IsHitWall ? GroundChecker.IsObstacleHit.distance : maxDistance));
        }
    }
}
//TODO: Adjust Configs