using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class PlayerMapInputProvider : IPlayerMapInput
    {
        InputAction moveAction;
        InputAction sprintAction;
        InputAction jumpAction;
        InputAction switchCharacterAction;
        InputAction lookAction;
        InputAction aimAction;
        const string SwitchActionID = "Player/SwitchCharacter";
        const string MoveActionID = "Player/Move";
        const string LookActionID = "Player/Look";
        const string AimActionID = "Player/Aim";
        const string SprintActionID = "Player/Sprint";
        const string JumpActionID = "Player/Jump";

        public Vector2 MoveInput => moveAction.ReadValue<Vector2>();
        public Vector2 LookInput => lookAction.ReadValue<Vector2>();
        public InputAction SwitchCharacterPressed => switchCharacterAction;
        public InputAction IsSprinting => sprintAction;
        public InputAction JumpPressed => jumpAction;
        public InputAction IsAiming => aimAction;
   
        public void SwitchActionSetup() => switchCharacterAction = InputSystem.actions.FindAction(SwitchActionID);
        public void MoveActionSetup() => moveAction = InputSystem.actions.FindAction(MoveActionID);
        public void SprintActionSetup() => sprintAction = InputSystem.actions.FindAction(SprintActionID);
        public void JumpActionSetup() => jumpAction = InputSystem.actions.FindAction(JumpActionID);
        public void LookActionSetup() => lookAction = InputSystem.actions.FindAction(LookActionID);
        public void AimActionSetup() => aimAction = InputSystem.actions.FindAction(AimActionID);
    }
}