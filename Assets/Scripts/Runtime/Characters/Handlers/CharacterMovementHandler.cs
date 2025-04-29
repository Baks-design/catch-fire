using UnityEngine;

namespace CatchFire
{
    public class CharacterMovementHandler : IMovable
    {
        readonly CharacterData data;
        readonly CharacterController controller;
        readonly Transform transform;
        readonly Camera mainCamera;
        float targetRotation;
        float rotationVelocity;

        public float CurrentSpeed { get; private set; }
        public float CurrentInputMagnitude { get; private set; }

        public CharacterMovementHandler(
            CharacterData data,
            CharacterController controller,
            Transform transform,
            Camera mainCamera)
        {
            this.data = data;
            this.controller = controller;
            this.transform = transform;
            this.mainCamera = mainCamera;
        }

        public void HandleMovement(Vector2 moveInput, bool isSprinting, float deltaTime)
        {
            var targetSpeed = isSprinting ? data.sprintSpeed : data.moveSpeed;
            if (moveInput == Vector2.zero)
                targetSpeed = 0f;

            CurrentSpeed = CalculateSpeed(controller.velocity, targetSpeed, moveInput, deltaTime);

            if (moveInput != Vector2.zero)
                UpdateRotation(moveInput);

            MoveHorizontalVelocity(deltaTime);
        }

        float CalculateSpeed(Vector3 currentVelocity, float targetSpeed, Vector2 moveInput, float deltaTime) //FIXME
        {
            var currentHorizontalSpeed = new Vector3(currentVelocity.x, 0f, currentVelocity.z).magnitude;

            CurrentInputMagnitude = data.analogMovement ? moveInput.magnitude : 1f;

            currentHorizontalSpeed = Maths.ExpDecay(
                currentHorizontalSpeed,
                targetSpeed * CurrentInputMagnitude,
                data.speedChangeRate,
                deltaTime
            );

            return currentHorizontalSpeed;
        }

        void UpdateRotation(Vector2 moveInput)
        {
            var inputDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) *
                            Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            var rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, targetRotation, ref rotationVelocity, data.rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        void MoveHorizontalVelocity(float deltaTime)
        {
            var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            controller.Move(targetDirection.normalized * (CurrentSpeed * deltaTime));
        }
    }
}