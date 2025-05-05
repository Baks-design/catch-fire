using UnityEngine;

namespace CatchFire
{
    public class CharacterGroundChecker : IGroundedCheckable
    {
        readonly CharacterData data;
        readonly CharacterController character;
        readonly Transform transform;
        readonly IPlayerMapInput input;
        readonly IMovable movable;
        bool wasGrounded;

        public bool IsLanded => IsGrounded && !wasGrounded;
        public bool IsGrounded { get; private set; }
        public RaycastHit IsGroundHit { get; private set; }
        public bool IsHitWall { get; private set; }
        public RaycastHit IsObstacleHit { get; private set; }

        public CharacterGroundChecker(
            CharacterData data,
            CharacterController character,
            Transform transform,
            IPlayerMapInput input,
            IMovable movable)
        {
            this.data = data;
            this.character = character;
            this.transform = transform;
            this.input = input;
            this.movable = movable;

            IsGrounded = true;
            wasGrounded = true;
        }

        public void CheckGrounded()
        {
            IsGrounded = Physics.SphereCast(
                character.transform.position + (Vector3.down * data.groundedOffset),
                character.radius,
                Vector3.down,
                out var groundHit,
                data.surfaceCheckDistance,
                data.groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (IsGrounded)
            {
                var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
                if (groundAngle > data.maxSlopeAngle)
                    IsGrounded = false;
            }

            wasGrounded = IsGrounded;
            IsGroundHit = groundHit;
        }

        public void CheckObstacle()
        {
            if (input.MoveInput == Vector2.zero ||
                movable.FinalMoveDir.sqrMagnitude <= 0f)
                return;
            
            IsHitWall = Physics.SphereCast(
                transform.position + character.center,
                data.rayObstacleSphereRadius,
                movable.FinalMoveDir,
                out var obstacleHit,
                data.rayObstacleLength,
                data.obstacleLayers,
                QueryTriggerInteraction.Ignore
            );
            IsObstacleHit = obstacleHit;
        }
    }
}