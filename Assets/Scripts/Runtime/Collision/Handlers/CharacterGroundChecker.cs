using UnityEngine;

namespace CatchFire
{
    public class CharacterGroundChecker
    {
        readonly CharacterData data;
        readonly CharacterController character;

        public bool IsLanded => IsGrounded && !WasGrounded;
        public bool WasGrounded { get; set; }
        public bool IsGrounded { get; set; }
        public RaycastHit IsGroundHit { get; private set; }

        public CharacterGroundChecker(
            CharacterData data,
            CharacterController character)
        {
            this.data = data;
            this.character = character;

            IsGrounded = true;
            WasGrounded = true;
        }

        public void CheckGrounded()
        {
            IsGrounded = Physics.SphereCast(
                character.transform.position + (Vector3.down * data.groundedOffset),
                character.radius,
                Vector3.down,
                out var groundHit,
                data.surfaceCheckDistance,
                data.groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (IsGrounded)
            {
                var groundAngle = Vector3.Angle(groundHit.normal, Vector3.up);
                if (groundAngle > data.maxSlopeAngle)
                    IsGrounded = false;
            }

            WasGrounded = IsGrounded;
            IsGroundHit = groundHit;
        }
    }
}