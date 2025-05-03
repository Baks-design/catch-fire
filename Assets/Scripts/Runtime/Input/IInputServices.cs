using UnityEngine.InputSystem;

namespace CatchFire
{
    public interface IInputServices
    {
        void SetCursor(bool isSet);
        void OnDeviceChanged(InputDevice device, InputDeviceChange change); 
    }
}