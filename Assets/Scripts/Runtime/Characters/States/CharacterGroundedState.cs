using UnityEngine;

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