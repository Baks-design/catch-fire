using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace CatchFire
{
    public class InputServicesProvider : IInputServices
    {
        public bool IsUsingMouse { get; private set; }
        public bool IsUsingKeyboard { get; private set; }

        public void SetCursor(bool isSet)
        => Cursor.lockState = isSet ? CursorLockMode.Locked : CursorLockMode.None;

        public void OnInputEvent(InputEventPtr _, InputDevice device)
        {
            // Only process state change events from mouse/keyboard
            if (device == null || !(device is Mouse || device is Keyboard)) return;

            // Update device flags
            IsUsingMouse = device is Mouse;
            IsUsingKeyboard = device is Keyboard;
        }
    }
}