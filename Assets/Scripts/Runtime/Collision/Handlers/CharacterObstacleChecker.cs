using UnityEngine;

namespace CatchFire
{
    public class CharacterObstacleChecker
    {
        readonly CharacterData data;
        readonly CharacterController character;
        readonly Transform transform;
        readonly IPlayerMapInput input;
        readonly CharacterMovementHandler movable;

        public bool IsBlocked { get; private set; }
        public RaycastHit IsObstacleHit { get; private set; }

        public CharacterObstacleChecker(
            CharacterData data,
            CharacterController character,
            Transform transform,
            IPlayerMapInput input,
            CharacterMovementHandler movable)
        {
            this.data = data;
            this.character = character;
            this.transform = transform;
            this.input = input;
            this.movable = movable;
        }

        public void CheckObstacle()
        {
            IsBlocked = Physics.SphereCast(
                transform.position + (Vector3.forward * data.obstacleOffset),
                data.rayObstacleSphereRadius,
                movable.FinalMoveDir.normalized,
                out var obstacleHit,
                data.rayObstacleLength,
                data.obstacleLayers,
                QueryTriggerInteraction.Ignore
            );
            IsObstacleHit = obstacleHit;
        }
    }
}