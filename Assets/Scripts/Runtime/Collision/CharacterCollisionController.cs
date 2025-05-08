using KBCore.Refs;
using UnityEngine;

namespace CatchFire
{
    public class CharacterCollisionController : MonoBehaviour
    {
        [SerializeField] CharacterData data;
        [SerializeField, Self] CharacterController character;
        [SerializeField, Child] CharacterMovementController movementController;
        CharacterRigidBodyPushHandler rigidBodyPush;
        IPlayerMapInput playerMapInput;

        public CharacterGroundChecker GroundChecker { get; private set; }
        public CharacterObstacleChecker ObstacleChecker { get; private set; }
        public CharacterRoofChecker RoofChecker { get; private set; }

        void Awake()
        {
            InputSetup();
            InitClasses();
        }

        void InputSetup()
        {
            playerMapInput = new PlayerMapInputProvider();
            playerMapInput.MoveActionSetup();
        }

        void InitClasses()
        {
            rigidBodyPush = new CharacterRigidBodyPushHandler(data);
            GroundChecker = new CharacterGroundChecker(data, character);
            ObstacleChecker = new CharacterObstacleChecker(
                data, character, transform, playerMapInput, movementController.MovementHandler);
            RoofChecker = new CharacterRoofChecker(data, character);
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (data.canPush)
                rigidBodyPush.PushRigidBodies(hit);
        }

        void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;

            DebugGroundCheck();
            DebugObstacleCheck();
            DebugRoofCheck();
        }

        void DebugGroundCheck()
        {
            var spherePos = new Vector3(
                transform.position.x,
                transform.position.y - data.groundedOffset,
                transform.position.z
            );

            Gizmos.color = GroundChecker.IsGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(spherePos, character.radius);

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
        }

        void DebugObstacleCheck()
        {
            var spherePos = new Vector3(
                transform.position.x,
                transform.position.y - data.obstacleOffset,
                transform.position.z
            );

            var direction = transform.forward;
            var maxDistance = data.rayObstacleLength;

            Gizmos.color = ObstacleChecker.IsBlocked ? Color.green : Color.red;
            Gizmos.DrawWireSphere(spherePos, data.rayObstacleSphereRadius);

            if (!ObstacleChecker.IsBlocked)
                Gizmos.DrawWireSphere(spherePos + direction * maxDistance, data.rayObstacleSphereRadius);
            else
                Gizmos.DrawWireSphere(spherePos + direction * ObstacleChecker.IsObstacleHit.distance, data.rayObstacleSphereRadius);

            Gizmos.DrawLine(spherePos, spherePos + direction * (ObstacleChecker.IsBlocked ? ObstacleChecker.IsObstacleHit.distance : maxDistance));
        }

        void DebugRoofCheck()
        {
            var spherePos = new Vector3(
                transform.position.x,
                transform.position.y - data.roofOffset,
                transform.position.z
            );

            Gizmos.color = RoofChecker.IsRoofed ? Color.green : Color.red;
            Gizmos.DrawWireSphere(spherePos, character.radius);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(spherePos, spherePos + Vector3.up * data.surfaceCheckDistance);

            if (RoofChecker.IsRoofed)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(RoofChecker.IsRoofHit.point, 0.05f);

                Gizmos.color = Color.blue;
                Gizmos.DrawRay(RoofChecker.IsRoofHit.point, RoofChecker.IsRoofHit.normal * 0.5f);

                Gizmos.color = Color.white;
                Gizmos.DrawLine(spherePos, RoofChecker.IsRoofHit.point);
            }
        }
    }
}
