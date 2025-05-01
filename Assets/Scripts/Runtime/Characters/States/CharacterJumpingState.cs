namespace CatchFire
{
    public class CharacterJumpingState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterJumpingState(CharacterMovementController controller)
        => this.controller = controller;

        public void Update()
        {
            controller.GravityHandler.ApplyVerticalVelocity();
            controller.MovementHandler.HandleMovement();
        }
    }
}