using UnityEngine;

namespace CatchFire
{
    public class CharacterGravityHandler : ICharacterGravity
    {
        readonly CharacterData data;
        readonly CharacterController controller;

        public bool FreeFall { get; private set; }
        public float VerticalVelocity { get; private set; }
        public float InAirTimer { get; private set; }

        public CharacterGravityHandler(
            CharacterData data,
            CharacterController controller)
        {
            this.data = data;
            this.controller = controller;

            InAirTimer = 0f;
        }

        public void ApplyVerticalVelocity()
        {
            ApplyGravity();
            MoveVerticalVelocity();
        }

        void ApplyGravity()
        {
            if (controller.isGrounded)
            {
                InAirTimer = 0f;
                VerticalVelocity = -data.stickToGroundForce;
            }
            else
            {
                InAirTimer += Time.deltaTime;
                VerticalVelocity += Physics.gravity.y * data.gravityMultiplier * Time.deltaTime;
            }
        }

        void MoveVerticalVelocity() => controller.Move(Time.deltaTime * VerticalVelocity * Vector3.up);
    }
}