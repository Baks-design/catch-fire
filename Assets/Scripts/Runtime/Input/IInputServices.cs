using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace CatchFire
{
    public interface IInputServices
    {
        bool IsUsingMouse { get; }
        bool IsUsingKeyboard { get; }

        void SetCursor(bool isSet);
        void OnInputEvent(InputEventPtr eventPtr, InputDevice device);
    }
}