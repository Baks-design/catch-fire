using System;
using Unity.Mathematics;
using UnityEngine;

namespace CatchFire
{
    public class CharacterJumpHandler : IJumpable
    {
        readonly CharacterData data;
        readonly CharacterController controller;
        float jumpTimeoutDelta;
        float fallTimeoutDelta;

        public bool JumpTriggered { get; private set; }
        public bool FreeFall { get; private set; }
        public float VerticalVelocity { get; private set; }

        public CharacterJumpHandler(CharacterData data, CharacterController controller)
        {
            this.data = data;
            this.controller = controller;
            jumpTimeoutDelta = data.jumpTimeout;
            fallTimeoutDelta = data.fallTimeout;
        }

        public void HandleJump(bool grounded, bool jumpPressed, float deltaTime)
        {
            if (grounded)
                HandleGroundedState(jumpPressed, deltaTime);
            else
                HandleAirborneState(deltaTime);

            ApplyGravity(deltaTime);
            MoveVerticalVelocity(deltaTime);
        }

        void HandleGroundedState(bool jumpPressed, float deltaTime)
        {
            fallTimeoutDelta = data.fallTimeout;
            FreeFall = false;
            JumpTriggered = false;

            if (VerticalVelocity < 0f)
                VerticalVelocity = -2f;

            if (jumpPressed && jumpTimeoutDelta <= 0f)
            {
                JumpTriggered = true;
                VerticalVelocity = math.sqrt(data.jumpHeight * -2f * data.gravity);
            }

            if (jumpTimeoutDelta >= 0f)
                jumpTimeoutDelta -= deltaTime;
        }

        void HandleAirborneState(float deltaTime)
        {
            jumpTimeoutDelta = data.jumpTimeout;

            if (fallTimeoutDelta >= 0f)
                fallTimeoutDelta -= deltaTime;
            else
                FreeFall = true;
        }

        void ApplyGravity(float deltaTime)
        {
            const float TerminalVelocity = 53f;
            if (VerticalVelocity < TerminalVelocity)
                VerticalVelocity += data.gravity * deltaTime;
        }

        void MoveVerticalVelocity(float deltaTime)
        {
            var verticalVelocity = new Vector3(0f, VerticalVelocity, 0f);
            controller.Move(verticalVelocity * deltaTime);
        }
    }
}