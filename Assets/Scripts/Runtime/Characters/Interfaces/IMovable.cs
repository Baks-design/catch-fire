using UnityEngine;

namespace CatchFire
{
    public interface IMovable
    {
        float CurrentSpeed { get; }
        float CurrentInputMagnitude { get; }
        float TargetSpeed { get; }

        void HandleMovement(Vector2 moveInput, bool isSprinting, float deltaTime);
    }
}