using UnityEngine;

namespace CatchFire
{
    public interface IMovable
    {
        float CurrentSpeed { get; }
        float CurrentInputMagnitude { get; }
        
        void HandleMovement(Vector2 moveInput, bool isSprinting, float deltaTime);
    }
}