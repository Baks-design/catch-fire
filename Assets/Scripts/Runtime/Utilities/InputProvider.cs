using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class InputProvider : IInputProvider
    {
        readonly InputAction _moveAction;
        readonly InputAction _lookAction;
        readonly InputAction _sprintAction;
        readonly InputAction _jumpAction;
        readonly InputAction _switchCharacterAction;

        public Vector2 MoveInput => _moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        public Vector2 LookInput => _lookAction?.ReadValue<Vector2>() ?? Vector2.zero;
        public bool IsSprinting => _sprintAction?.IsPressed() ?? false;
        public bool JumpPressed => _jumpAction?.WasPressedThisDynamicUpdate() ?? false;
        public InputAction SwitchCharacterPressed => _switchCharacterAction;

        public InputProvider()
        {
            _moveAction = InputSystem.actions.FindAction("Player/Move");
            _lookAction = InputSystem.actions.FindAction("Player/Look");
            _sprintAction = InputSystem.actions.FindAction("Player/Sprint");
            _jumpAction = InputSystem.actions.FindAction("Player/Jump");
            _switchCharacterAction = InputSystem.actions.FindAction("Player/SwitchCharacter");
        }
    }
}