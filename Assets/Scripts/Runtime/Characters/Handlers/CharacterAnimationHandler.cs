using UnityEngine;

namespace CatchFire
{
    public class CharacterAnimationHandler : IAnimationHandler
    {
        readonly Animator animator;
        readonly int animIDGrounded;
        readonly int animIDJump;
        readonly int animIDFreeFall;
        readonly int animIDSpeed;
        readonly int animIDMotionSpeed;

        public CharacterAnimationHandler(Animator animator)
        {
            this.animator = animator;
            animIDGrounded = Animator.StringToHash("Grounded");
            animIDJump = Animator.StringToHash("Jump");
            animIDFreeFall = Animator.StringToHash("FreeFall");
            animIDSpeed = Animator.StringToHash("Speed");
            animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        public void SetGrounded(bool grounded) => animator.SetBool(animIDGrounded, grounded);
        public void SetJump(bool jumping) => animator.SetBool(animIDJump, jumping);
        public void SetFreeFall(bool freeFall) => animator.SetBool(animIDFreeFall, freeFall);
        public void SetSpeed(float speed) => animator.SetFloat(animIDSpeed, speed);
        public void SetMotionSpeed(float motionSpeed) => animator.SetFloat(animIDMotionSpeed, motionSpeed);
    }
}