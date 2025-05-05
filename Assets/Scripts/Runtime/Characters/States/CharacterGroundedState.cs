namespace CatchFire
{
    public class CharacterGroundedState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterGroundedState(CharacterMovementController controller) => this.controller = controller;

        public void Update()
        {
            controller.GroundChecker.CheckGrounded();
            controller.GroundChecker.CheckObstacle();
            controller.GravityHandler.ApplyVerticalVelocity();
            controller.MovementHandler.HandleMovement(controller.GroundChecker);
            controller.MovementEffects.HandleCameraSway();
            controller.MovementEffects.HandleHeadBob(controller.GroundChecker);
            controller.MovementEffects.HandleRunFOV(controller.GroundChecker);
        }
    }
}