using UnityEngine;

namespace CatchFire
{
    public class CharacterGravityHandler : ICharacterGravity
    {
        readonly CharacterData data;
        readonly CharacterController controller;
        readonly IGroundedCheckable grounded;

        public bool FreeFall { get; private set; }
        public float VerticalVelocity { get; private set; }
        public float FallTimeoutDelta { get; private set; }

        public CharacterGravityHandler(
            CharacterData data,
            CharacterController controller,
            IGroundedCheckable grounded)
        {
            this.data = data;
            this.controller = controller;
            this.grounded = grounded;

            FallTimeoutDelta = data.fallTimeout;
        }

        public void ApplyVerticalVelocity()
        {
            ApplyGravity();
            MoveVerticalVelocity();
        }

        void ApplyGravity()
        {
            const float terminalVelocity = -53f;

            if (VerticalVelocity > terminalVelocity)
                VerticalVelocity = Mathf.Max(terminalVelocity, VerticalVelocity + data.gravity * Time.deltaTime);
        }

        void MoveVerticalVelocity() => controller.Move(Time.deltaTime * VerticalVelocity * Vector3.up);

        public void HandleStates()
        {
            if (grounded.Grounded)
                HandleGroundedState();
            else
                HandleAirborneState();
        }

        void HandleGroundedState()
        {
            FallTimeoutDelta = data.fallTimeout;
            FreeFall = false;

            if (VerticalVelocity < 0f)
                VerticalVelocity = -2f;
        }

        void HandleAirborneState()
        {
            if (FallTimeoutDelta >= 0f)
                FallTimeoutDelta -= Time.deltaTime;
            else
                FreeFall = true;
        }
    }
}