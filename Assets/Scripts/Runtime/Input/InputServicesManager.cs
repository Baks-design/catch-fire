using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class InputServicesManager : MonoBehaviour
    {
        IInputServices inputServices;

        void Awake()
        {
            inputServices = new InputServicesProvider();
            inputServices.SetCursor(true);
        }

        void OnEnable() => InputSystem.onEvent += inputServices.OnInputEvent;

        void OnDestroy() => InputSystem.onEvent -= inputServices.OnInputEvent;

        void Update()
        {
            Debug.Log("IsUsingMouse: " + inputServices.IsUsingMouse); //FIXME
            Debug.Log("IsUsingKeyboard: " + inputServices.IsUsingKeyboard);
        }
    }
}