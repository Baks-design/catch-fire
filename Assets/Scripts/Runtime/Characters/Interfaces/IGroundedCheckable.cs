using UnityEngine;

namespace CatchFire
{
    public interface IGroundedCheckable
    {
        RaycastHit IsHit { get; }
        bool IsGrounded { get; }
        bool IsLanded { get; }

        void CheckGrounded();
    }
}