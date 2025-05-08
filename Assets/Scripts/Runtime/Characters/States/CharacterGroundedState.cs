namespace CatchFire
{
    public class CharacterGroundedState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterGroundedState(CharacterMovementController controller) => this.controller = controller;

        public void Update()
        {
            controller.CollisionController.GroundChecker.CheckGrounded();
            controller.CollisionController.ObstacleChecker.CheckObstacle();
            controller.GravityHandler.ApplyVerticalVelocity();
            controller.MovementHandler.HandleMovement(
                controller.CollisionController.GroundChecker
            );
            controller.MovementEffects.HandleCameraSway();
            controller.MovementEffects.HandleHeadBob(
                controller.CollisionController.GroundChecker,
                controller.CollisionController.ObstacleChecker
            );
            controller.MovementEffects.HandleRunFOV(
                controller.CollisionController.GroundChecker,
                controller.CollisionController.ObstacleChecker
            );
        }
    }
}