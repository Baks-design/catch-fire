using UnityEngine;

namespace CatchFire
{
    public class CharacterFallingState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterFallingState(CharacterMovementController controller)
        => this.controller = controller;

        public void Update()
        {
            controller.GroundChecker.CheckGrounded();

            controller.JumpHandler.HandleJump(
                controller.GroundChecker.Grounded,
                controller.InputProvider.JumpPressed,
                Time.deltaTime
            );

            controller.MovementHandler.HandleMovement(
                controller.InputProvider.MoveInput,
                controller.InputProvider.IsSprinting,
                Time.deltaTime
            );
        }
    }
}