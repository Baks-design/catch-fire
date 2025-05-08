namespace CatchFire
{
    public class CharacterJumpHandler
    {
        readonly CharacterData characterData;
        readonly CharacterCrouchHandler characterCrouch;
        readonly CharacterGravityHandler gravityHandler;

        public CharacterJumpHandler(
            CharacterData characterData,
            CharacterCrouchHandler characterCrouch,
            CharacterGravityHandler gravityHandler)
        {
            this.characterData = characterData;
            this.characterCrouch = characterCrouch;
            this.gravityHandler = gravityHandler;
        }

        public void ApplyJump(CharacterCollisionController collisionController)
        {
            if (collisionController.GroundChecker.IsGrounded && !characterCrouch.IsCrouching)
            {
                gravityHandler.VerticalVelocity = characterData.jumpSpeed;
                collisionController.GroundChecker.WasGrounded = true;
                collisionController.GroundChecker.IsGrounded = false;
            }
        }
    }
}