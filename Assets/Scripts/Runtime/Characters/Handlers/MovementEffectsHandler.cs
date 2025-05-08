using UnityEngine;

namespace CatchFire
{
    public class MovementEffectsHandler
    {
        readonly HeadBob headBob;
        readonly CharacterCameraController cameraController;
        readonly CharacterData data;
        readonly Transform yawTransform;
        readonly CharacterMovementHandler movable;
        readonly IPlayerMapInput input;
        readonly CharacterCrouchHandler crouchHandler;
        bool duringRunAnimation;

        public MovementEffectsHandler(
            CharacterCameraController cameraController,
            HeadBob headBob,
            CharacterData data,
            Transform yawTransform,
            CharacterMovementHandler movable,
            IPlayerMapInput input,
            CharacterCrouchHandler crouchHandler)
        {
            this.cameraController = cameraController;
            this.data = data;
            this.yawTransform = yawTransform;
            this.movable = movable;
            this.input = input;
            this.crouchHandler = crouchHandler;

            headBob.CurrentStateHeight = yawTransform.localPosition.y;
        }

        public void HandleHeadBob(CharacterGroundChecker grounded, CharacterObstacleChecker blocked)
        {
            if (input.MoveInput != Vector2.zero && grounded.IsGrounded && !blocked.IsBlocked)
            {
                if (!crouchHandler.IsDuringCrouchAnimation)
                {
                    headBob.ScrollHeadBob(movable.IsRunning && movable.CanRun(), crouchHandler.IsCrouching, input.MoveInput);

                    yawTransform.localPosition = Maths.ExpDecay(
                        yawTransform.localPosition,
                        (Vector3.up * headBob.CurrentStateHeight) + headBob.FinalOffset,
                        data.smoothHeadBobSpeedDecay,
                        Time.deltaTime * data.smoothHeadBobSpeed
                    );
                }
            }
            else
            {
                if (!headBob.Resetted)
                    headBob.ResetHeadBob();

                if (!crouchHandler.IsDuringCrouchAnimation)
                {
                    var bobHeight = new Vector3(0f, headBob.CurrentStateHeight, 0f);
                    yawTransform.localPosition = Maths.ExpDecay(
                        yawTransform.localPosition,
                        bobHeight,
                        data.smoothHeadBobSpeedDecay,
                        Time.deltaTime * data.smoothHeadBobSpeed
                    );
                }
            }
        }

        public void HandleCameraSway()
        => cameraController.HandleSway(movable.SmoothInputVector, input.MoveInput.x);

        public void HandleRunFOV(CharacterGroundChecker grounded, CharacterObstacleChecker blocked)
        {
            if (input.MoveInput != Vector2.zero && grounded.IsGrounded && !blocked.IsBlocked)
            {
                if (movable.IsRunning && movable.CanRun())
                {
                    duringRunAnimation = true;
                    cameraController.ChangeRunFOV(false);
                }

                if (movable.IsRunning && movable.CanRun() && !duringRunAnimation)
                {
                    duringRunAnimation = true;
                    cameraController.ChangeRunFOV(false);
                }
            }

            if (!movable.IsRunning || input.MoveInput == Vector2.zero || blocked.IsBlocked)
            {
                if (duringRunAnimation)
                {
                    duringRunAnimation = false;
                    cameraController.ChangeRunFOV(true);
                }
            }
        }
    }
}