using UnityEngine;

namespace CatchFire
{
    public interface IMovable
    {
        bool IsMoving { get; }
        bool IsRunning { get; }
        float CurrentSpeed { get; }
        float CurrentInputMagnitude { get; }
        float TargetSpeed { get; }
        Vector2 SmoothInputVector { get; }
        Vector3 FinalMoveDir { get; }

        bool CanRun();
        void ApplySprint(bool context);
        void HandleMovement(IGroundedCheckable grounded);
    }
}