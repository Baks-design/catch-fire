using UnityEngine;

namespace CatchFire
{
    public interface IPickable
    {
        Rigidbody Rigid { get; set; }

        void OnPickUp();
        void OnHold();
        void OnRelease();
    }
}
