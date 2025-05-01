using UnityEngine;

namespace CatchFire
{
    public class CharacterMovementHandler : IMovable
    {
        readonly IPlayerMapInput input;
        private readonly IInputServices services;
        readonly CharacterData data;
        readonly CharacterController controller;
        readonly Transform transform;
        readonly Camera mainCamera;
        bool isRunning;
        float targetRotation;
        float rotationVelocity;
        const float offset = 0.1f;
        const float round = 1000f;

        public float CurrentSpeed { get; set; }
        public float CurrentInputMagnitude { get; set; }
        public float TargetSpeed { get; set; }

        public CharacterMovementHandler(
            IPlayerMapInput input,
            IInputServices services,
            CharacterData data,
            CharacterController controller,
            Transform transform,
            Camera mainCamera)
        {
            this.input = input;
            this.services = services;
            this.data = data;
            this.controller = controller;
            this.transform = transform;
            this.mainCamera = mainCamera;
        }

        public void ApplySprint(bool context) => isRunning = context;

        public void HandleMovement()
        {
            CalculateSpeed(controller.velocity, TargetSpeed);
            UpdateRotation();
            MoveHorizontalVelocity();
        }

        void CalculateSpeed(Vector3 currentVelocity, float targetSpeed)
        {
            TargetSpeed = isRunning ? data.sprintSpeed : data.moveSpeed;
            if (input.MoveInput == Vector2.zero)
                TargetSpeed = 0f;

            CurrentSpeed = new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude;

            CurrentInputMagnitude = services.IsUsingKeyboard ? 1f : input.MoveInput.magnitude;

            if (CurrentSpeed < targetSpeed - offset || CurrentSpeed > targetSpeed + offset)
            {
                CurrentSpeed = Maths.ExpDecay(
                    CurrentSpeed,
                    targetSpeed * CurrentInputMagnitude,
                    data.speedDecay,
                    data.speedChangeRate * Time.deltaTime
                );
                CurrentSpeed = Mathf.Round(CurrentSpeed * round) / round;
            }
            else
                CurrentSpeed = targetSpeed;
        }

        void UpdateRotation()
        {
            var inputDirection = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y).normalized;

            if (input.MoveInput != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) *
                                Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

                var rotation = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y, targetRotation, ref rotationVelocity, data.rotationSmoothTime);

                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
            }
        }

        void MoveHorizontalVelocity()
        {
            var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            controller.Move(targetDirection.normalized * (CurrentSpeed * Time.deltaTime));
        }
    }
}