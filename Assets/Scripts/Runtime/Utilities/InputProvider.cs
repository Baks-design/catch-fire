using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class InputProvider : IInputProvider
    {
        InputAction moveAction;
        InputAction lookAction;
        InputAction sprintAction;
        InputAction jumpAction;
        InputAction switchCharacterAction;
        const string SwitchActionID = "Player/SwitchCharacter";
        const string MoveActionID = "Player/Move";
        const string LookActionID = "Player/Look";
        const string SprintActionID = "Player/Sprint";
        const string JumpActionID = "Player/Jump";

        public Vector2 MoveInput => moveAction.ReadValue<Vector2>();
        public Vector2 LookInput => lookAction.ReadValue<Vector2>();
        public bool IsSprinting => sprintAction.IsPressed();
        public bool JumpPressed => jumpAction.WasPressedThisDynamicUpdate();
        public InputAction SwitchCharacterPressed => switchCharacterAction;

        public void SetCursor(bool isSet) => Cursor.lockState = isSet ? CursorLockMode.Locked : CursorLockMode.None;
        public void SwitchActionSetup() => switchCharacterAction = InputSystem.actions.FindAction(SwitchActionID);
        public void MoveActionSetup() => moveAction = InputSystem.actions.FindAction(MoveActionID);
        public void LookActionSetup() => lookAction = InputSystem.actions.FindAction(LookActionID);
        public void SprintActionSetup() => sprintAction = InputSystem.actions.FindAction(SprintActionID);
        public void JumpActionSetup() => jumpAction = InputSystem.actions.FindAction(JumpActionID);
    }
}