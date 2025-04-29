using UnityEngine;

namespace CatchFire
{
    public interface IInputProvider
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        bool IsSprinting { get; }
        bool JumpPressed { get; }

        void SetCursor(bool set);
        void MoveActionSetup();
        void LookActionSetup();
        void SprintActionSetup();
        void JumpActionSetup();
    }
}