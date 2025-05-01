namespace CatchFire
{
    public class CharacterGroundedState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterGroundedState(CharacterMovementController controller)
        => this.controller = controller;

        public void Update()
        {
            controller.GroundChecker.CheckGrounded();
            controller.GravityHandler.ApplyVerticalVelocity();
            controller.MovementHandler.HandleMovement();
        }
    }
}