using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class InputServicesManager : MonoBehaviour
    {
        IInputServices inputServices;

        void Awake()
        {
            DontDestroyOnLoad(this);
            inputServices = new InputServicesProvider();
        }

        void OnEnable() => InputSystem.onDeviceChange += inputServices.OnDeviceChanged;

        void OnDestroy() => InputSystem.onDeviceChange -= inputServices.OnDeviceChanged;

        void Start() => inputServices.SetCursor(true);
    }
}