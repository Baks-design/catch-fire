using UnityEngine;

namespace CatchFire
{
    public interface IGroundedCheckable
    {
        bool IsLanded { get; }
        bool IsGrounded { get; }
        RaycastHit IsGroundHit { get; }
        bool IsHitWall { get; }
        RaycastHit IsObstacleHit { get; }

        void CheckObstacle();
        void CheckGrounded();
    }
}