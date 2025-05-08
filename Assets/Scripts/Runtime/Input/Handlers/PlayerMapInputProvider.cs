using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class PlayerMapInputProvider : IPlayerMapInput
    {
        InputAction moveAction;
        InputAction sprintAction;
        InputAction jumpAction;
        InputAction crouchAction;
        InputAction switchCharacterAction;
        InputAction lookAction;
        InputAction aimAction;
        InputAction interactionAction;
        const string SwitchActionID = "Player/SwitchCharacter";
        const string MoveActionID = "Player/Move";
        const string LookActionID = "Player/Look";
        const string AimActionID = "Player/Aim";
        const string InteractionActionID = "Player/Interaction";
        const string SprintActionID = "Player/Sprint";
        const string JumpActionID = "Player/Jump";
        const string CrouchActionID = "Player/Crouch";

        public Vector2 MoveInput => moveAction.ReadValue<Vector2>();
        public Vector2 LookInput => lookAction.ReadValue<Vector2>();
        public InputAction SwitchCharacterPressed => switchCharacterAction;
        public InputAction IsSprinting => sprintAction;
        public InputAction JumpPressed => jumpAction;
        public InputAction CrouchPressed => crouchAction;
        public InputAction IsAiming => aimAction;
        public InputAction InteractionPressed => interactionAction;

        public void SwitchActionSetup() => switchCharacterAction = InputSystem.actions.FindAction(SwitchActionID);
        public void MoveActionSetup() => moveAction = InputSystem.actions.FindAction(MoveActionID);
        public void SprintActionSetup() => sprintAction = InputSystem.actions.FindAction(SprintActionID);
        public void JumpActionSetup() => jumpAction = InputSystem.actions.FindAction(JumpActionID);
        public void CrouchActionSetup() => crouchAction = InputSystem.actions.FindAction(CrouchActionID);
        public void LookActionSetup() => lookAction = InputSystem.actions.FindAction(LookActionID);
        public void AimActionSetup() => aimAction = InputSystem.actions.FindAction(AimActionID);
        public void InteractionActionSetup() => interactionAction = InputSystem.actions.FindAction(InteractionActionID);
    }
}