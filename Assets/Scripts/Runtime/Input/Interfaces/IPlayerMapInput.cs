using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public interface IPlayerMapInput
    {
        Vector2 MoveInput { get; }
        Vector2 LookInput { get; }
        InputAction IsSprinting { get; }
        InputAction JumpPressed { get; }
        InputAction CrouchPressed { get; }
        InputAction SwitchCharacterPressed { get; }
        InputAction IsAiming { get; }
        InputAction InteractionPressed { get; }

        void SwitchActionSetup();
        void MoveActionSetup();
        void SprintActionSetup();
        void JumpActionSetup();
        void LookActionSetup();
        void AimActionSetup();
        void InteractionActionSetup();
    }
}