using UnityEngine;

namespace CatchFire
{
    public class MovementEffectsHandler : IMovementEffects
    {
        readonly HeadBob headBob;
        readonly CharacterCameraController cameraController;
        readonly CharacterData data;
        readonly Transform yawTransform;
        readonly IMovable movable;
        readonly IPlayerMapInput input;
        bool duringRunAnimation;

        public MovementEffectsHandler(
            CharacterCameraController cameraController,
            HeadBobData bobData,
            CharacterData data,
            Transform yawTransform,
            IMovable movable,
            IPlayerMapInput input)
        {
            this.cameraController = cameraController;
            this.data = data;
            this.yawTransform = yawTransform;
            this.movable = movable;
            this.input = input;

            headBob = new HeadBob(bobData, data.moveBackwardsSpeedPercent, data.moveSideSpeedPercent)
            {
                CurrentStateHeight = yawTransform.localPosition.y
            };
        }

        public void HandleHeadBob(IGroundedCheckable grounded) //FIXME
        {
            if (input.MoveInput != Vector2.zero && grounded.IsGrounded && !grounded.IsHitWall)
            {
                headBob.ScrollHeadBob(movable.IsRunning && movable.CanRun(), false, input.MoveInput);

                yawTransform.localPosition = Maths.ExpDecay(
                    yawTransform.localPosition,
                    (Vector3.up * headBob.CurrentStateHeight) + headBob.FinalOffset,
                    data.smoothHeadBobSpeedDecay,
                    Time.deltaTime * data.smoothHeadBobSpeed
                );
            }
            else
            {
                if (!headBob.Resetted)
                    headBob.ResetHeadBob();
            }
        }

        public void HandleCameraSway() => cameraController.ApplySway(movable.SmoothInputVector, input.MoveInput.x); //FIXME

        public void HandleRunFOV(IGroundedCheckable grounded) //FIXME
        {
            if (input.MoveInput != Vector2.zero && grounded.IsGrounded && !grounded.IsHitWall)
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

            if (input.MoveInput == Vector2.zero || !movable.IsRunning || grounded.IsHitWall)
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