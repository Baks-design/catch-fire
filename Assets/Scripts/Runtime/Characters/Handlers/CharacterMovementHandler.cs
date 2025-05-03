using UnityEngine;

namespace CatchFire
{
    public class CharacterMovementHandler : IMovable
    {
        readonly IPlayerMapInput input;
        readonly CharacterData data;
        readonly CharacterController controller;
        readonly Transform transform;
        Vector3 inputDirection;

        public bool IsMoving { get; private set; }
        public bool IsRunning { get; private set; }
        public float CurrentSpeed { get; private set; }
        public float CurrentInputMagnitude { get; private set; }
        public float TargetSpeed { get; private set; }

        public CharacterMovementHandler(
            IPlayerMapInput input,
            CharacterData data,
            CharacterController controller,
            Transform transform)
        {
            this.input = input;
            this.data = data;
            this.controller = controller;
            this.transform = transform;
        }

        public void ApplySprint(bool context) => IsRunning = context;

        public void HandleMovement()
        {
            CalculateSpeed(controller.velocity, TargetSpeed);
            CalculateDirection();
            MoveHorizontalVelocity();
        }

        void CalculateSpeed(Vector3 currentVelocity, float targetSpeed)
        {
            TargetSpeed = IsRunning ? data.sprintSpeed : data.moveSpeed;
            if (input.MoveInput == Vector2.zero)
                TargetSpeed = 0f;

            CurrentSpeed = new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude;
            CurrentInputMagnitude = input.MoveInput.magnitude;

            if (CurrentSpeed < targetSpeed - 0.1f || CurrentSpeed > targetSpeed + 0.1f)
            {
                CurrentSpeed = Maths.ExpDecay(
                    CurrentSpeed,
                    targetSpeed * CurrentInputMagnitude,
                    data.speedDecay,
                    data.speedChangeRate * Time.deltaTime
                );
                CurrentSpeed = Mathf.Round(CurrentSpeed * 1000f) / 1000f;
            }
            else
                CurrentSpeed = targetSpeed;
        }

        void CalculateDirection()
        {
            inputDirection = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y).normalized;
            if (input.MoveInput != Vector2.zero)
                inputDirection = transform.right * input.MoveInput.x + transform.forward * input.MoveInput.y;
        }

        void MoveHorizontalVelocity()
        {
            controller.Move(inputDirection.normalized * (CurrentSpeed * Time.deltaTime));

            IsMoving = controller.velocity.magnitude > 0.1f; 
        }
    }
}