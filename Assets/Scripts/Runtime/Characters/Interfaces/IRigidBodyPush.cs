using UnityEngine;

namespace CatchFire
{
    public interface IRigidBodyPush
    {
        void PushRigidBodies(ControllerColliderHit hit);
    }
}