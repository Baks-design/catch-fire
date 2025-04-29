using UnityEngine;

namespace CatchFire
{
    public class CharacterJumpingState : IState
    {
        readonly CharacterMovementController controller;

        public CharacterJumpingState(CharacterMovementController controller)
        => this.controller = controller;

        public void Update()
        {            
            controller.JumpHandler.HandleJump(
                controller.GroundChecker.Grounded,
                controller.InputProvider.JumpPressed,
                Time.deltaTime);
        }
    }
}