using UnityEngine;

namespace CatchFire
{
    public class CharacterGroundChecker : IGroundedCheckable
    {
        readonly CharacterData data;
        readonly Transform transform;

        public bool Grounded { get; private set; }

        public CharacterGroundChecker(CharacterData data, Transform transform)
        {
            this.data = data;
            this.transform = transform;
        }

        public void CheckGrounded()
        {
            var spherePosition = new Vector3(
                transform.position.x,
                transform.position.y - data.groundedOffset,
                transform.position.z
            );
            Grounded = Physics.CheckSphere(
                spherePosition,
                data.groundedRadius,
                data.groundLayers,
                QueryTriggerInteraction.Ignore
            );
        }
    }
}