using UnityEngine;

namespace CatchFire
{
    public class CharacterMovementHandler : IMovable
    {
        readonly IPlayerMapInput input;
        readonly CharacterData data;
        readonly CharacterController controller;
        readonly Transform transform;
        readonly Transform yawTransform;
        Vector3 smoothFinalMoveDir;
        Vector3 finalMoveVector;
        float currentSpeed;
        float smoothCurrentSpeed;
        float finalSmoothCurrentSpeed;
        readonly float walkRunSpeedDifference;

        public bool IsMoving { get; private set; }
        public bool IsRunning { get; private set; }
        public float CurrentSpeed { get; private set; }
        public float CurrentInputMagnitude { get; private set; }
        public float TargetSpeed { get; private set; }
        public Vector2 SmoothInputVector { get; private set; }
        public Vector3 FinalMoveDir { get; private set; }

        public CharacterMovementHandler(
            IPlayerMapInput input,
            CharacterData data,
            CharacterController controller,
            Transform transform,
            Transform yawTransform)
        {
            this.input = input;
            this.data = data;
            this.controller = controller;
            this.transform = transform;
            this.yawTransform = yawTransform;

            walkRunSpeedDifference = data.sprintSpeed - data.moveSpeed;
            controller.center = new Vector3(0f, controller.height / 2f + controller.skinWidth, 0f);
        }

        public void HandleMovement(IGroundedCheckable grounded)
        {
            CalculateSpeed();
            SmoothInput();
            SmoothSpeed();
            SmoothDir();
            CalculateMovementDirection(grounded);
            CalculateFinalMovement();
            MoveHorizontalVelocity();
            RotateTowardsCamera();
        }

        public void ApplySprint(bool context) => IsRunning = context;

        public bool CanRun()
        {
            var normalizedDir = Vector3.zero;

            if (smoothFinalMoveDir != Vector3.zero)
                normalizedDir = smoothFinalMoveDir.normalized;

            var dot = Vector3.Dot(transform.forward, normalizedDir);
            return dot >= data.canRunThreshold;
        }

        void CalculateSpeed()
        {
            currentSpeed = IsRunning && CanRun() ? data.sprintSpeed : data.moveSpeed;
            currentSpeed = input.MoveInput == Vector2.zero ? 0f : currentSpeed;
            currentSpeed = input.MoveInput.y == -1f ? currentSpeed * data.moveBackwardsSpeedPercent : currentSpeed;
            currentSpeed = input.MoveInput.x != 0f && input.MoveInput.y == 0f ? currentSpeed * data.moveSideSpeedPercent : currentSpeed;
        }

        void SmoothInput() => SmoothInputVector = Maths.ExpDecay(
            SmoothInputVector, input.MoveInput, data.smoothInputDecay, Time.deltaTime * data.smoothInputSpeed);

        void SmoothSpeed()
        {
            smoothCurrentSpeed = Maths.ExpDecay(
                smoothCurrentSpeed, currentSpeed, data.smoothSpeedDecay, Time.deltaTime * data.smoothVelocitySpeed);

            if (IsRunning && CanRun())
            {
                var walkRunPercent = Maths.InvExpDecay(smoothCurrentSpeed, data.moveSpeed, data.sprintSpeed, data.walkToRunDecay);
                finalSmoothCurrentSpeed = data.runTransitionCurve.Evaluate(walkRunPercent) * walkRunSpeedDifference + data.moveSpeed;
            }
            else
                finalSmoothCurrentSpeed = smoothCurrentSpeed;
        }

        void SmoothDir() => smoothFinalMoveDir = Maths.ExpDecay(
            smoothFinalMoveDir, FinalMoveDir, data.smoothDirectionalDecay, Time.deltaTime * data.smoothFinalDirectionSpeed);

        void CalculateMovementDirection(IGroundedCheckable grounded)
        {
            var vDir = transform.forward * SmoothInputVector.y;
            var hDir = transform.right * SmoothInputVector.x;

            var desiredDir = vDir + hDir;
            var flattenDir = FlattenVectorOnSlopes(desiredDir, grounded);

            FinalMoveDir = flattenDir;
        }

        Vector3 FlattenVectorOnSlopes(Vector3 vectorToFlat, IGroundedCheckable grounded)
        {
            if (grounded.IsGrounded)
                vectorToFlat = Vector3.ProjectOnPlane(vectorToFlat, grounded.IsGroundHit.normal);

            return vectorToFlat;
        }

        void CalculateFinalMovement()
        {
            var finalVector = smoothFinalMoveDir * finalSmoothCurrentSpeed;

            finalMoveVector.x = finalVector.x;
            finalMoveVector.z = finalVector.z;

            if (controller.isGrounded)
                finalMoveVector.y += finalVector.y;
        }

        void MoveHorizontalVelocity()
        {
            controller.Move(finalMoveVector * Time.deltaTime);
            IsMoving = controller.velocity.magnitude > 0.1f;
        }

        void RotateTowardsCamera()
        => transform.rotation = Maths.ExpDecay(
            transform.rotation,
            yawTransform.rotation,
            data.rotationDecay,
            Time.deltaTime * data.smoothRotateSpeed
        );
    }
}