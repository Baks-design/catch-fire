namespace CatchFire
{
    public class CharacterFallingState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterFallingState(CharacterMovementController controller) => this.controller = controller;

        public void Update()
        {
            controller.CollisionController.GroundChecker.CheckGrounded();
            controller.GravityHandler.ApplyVerticalVelocity();
            controller.MovementHandler.HandleMovement(controller.CollisionController.GroundChecker);
        }
    }
}