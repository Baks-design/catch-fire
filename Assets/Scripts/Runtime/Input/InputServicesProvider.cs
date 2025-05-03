using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class InputServicesProvider : IInputServices
    {
        public void SetCursor(bool isSet) => Cursor.lockState = isSet ? CursorLockMode.Locked : CursorLockMode.None;

        public void OnDeviceChanged(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Added: 
                    break;
                case InputDeviceChange.Reconnected:
                    break;
                case InputDeviceChange.Removed:
                    break;
            }
        }
    }
}