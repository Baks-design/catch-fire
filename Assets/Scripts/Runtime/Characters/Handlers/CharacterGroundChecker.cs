using UnityEngine;

namespace CatchFire
{
    public class CharacterGroundChecker : IGroundedCheckable
    {
        readonly CharacterData data;
        readonly CharacterController character;
        bool wasGrounded;

        public RaycastHit IsHit { get; private set; }
        public bool IsGrounded { get; private set; } = true;
        public bool IsLanded => IsGrounded && !wasGrounded;

        public CharacterGroundChecker(CharacterData data, CharacterController character)
        {
            this.data = data;
            this.character = character;
        }

        public void CheckGrounded()
        {
            IsGrounded = Physics.SphereCast(
                character.transform.position + (Vector3.down * data.groundedOffset),
                character.radius,
                Vector3.down,
                out var hit,
                data.surfaceCheckDistance,
                data.groundLayers,
                QueryTriggerInteraction.Ignore
            );

            if (IsGrounded && data.maxSlopeAngle < 90f)
            {
                var groundAngle = Vector3.Angle(hit.normal, Vector3.up);
                if (groundAngle > data.maxSlopeAngle)
                    IsGrounded = false; 
            }

            IsHit = hit; 
            wasGrounded = IsGrounded;
        }
    }
}