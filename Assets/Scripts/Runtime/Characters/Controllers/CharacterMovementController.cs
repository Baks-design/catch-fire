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
            InitComponents();
            SetupStateMachine();
        }

        void InitComponents()
        {
            InputProvider = new InputProvider();
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

        protected override void Update()
        {
            base.Update();

            Debug.Log("Current" + stateMachine.CurrentState);
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (data.canPush)
                rigidBodyPush.PushRigidBodies(hit);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = GroundChecker.Grounded ?
                new Color(0f, 1f, 0f, 0.35f) :
                new Color(1f, 0f, 0f, 0.35f);

            var pos = new Vector3(
                controller.transform.position.x,
                controller.transform.position.y - data.groundedOffset,
                controller.transform.position.z);

            Gizmos.DrawSphere(pos, data.groundedRadius);
        }
    }
}