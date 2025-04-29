using UnityEngine;

namespace CatchFire
{
    public class CharacterMovementController : CharacterBase
    {
        [SerializeField] CharacterData data;
        [SerializeField] CharacterController controller;
        IRigidBodyPush rigidBodyPush;

        public IInputProvider InputProvider { get; private set; }
        public IGroundedCheckable GroundChecker { get; private set; }
        public IMovable MovementHandler { get; private set; }
        public IJumpable JumpHandler { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            AssignInputs();
            InitComponents();
            SetupStateMachine();
        }

        void AssignInputs()
        {
            InputProvider = new InputProvider();
            InputProvider.MoveActionSetup();
            InputProvider.SprintActionSetup();
            InputProvider.JumpActionSetup();
            InputProvider.SetCursor(true);
        }

        void InitComponents()
        {
            GroundChecker = new CharacterGroundChecker(data, transform);
            JumpHandler = new CharacterJumpHandler(data, controller);
            MovementHandler = new CharacterMovementHandler(data, controller, transform, Camera.main);
            rigidBodyPush = new CharacterRigidBodyPushHandler(data);
        }

        void SetupStateMachine()
        {
            var grounded = new CharacterGroundedState(this);
            var falling = new CharacterFallingState(this);
            var jumping = new CharacterJumpingState(this);

            At(grounded, falling, () => !GroundChecker.Grounded);
            At(grounded, jumping, () => InputProvider.JumpPressed);
            At(falling, grounded, () => GroundChecker.Grounded);

            stateMachine.SetState(falling);
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

//TODO: Separate Gravity and jump
//TODO: Switch Lerp by Decay