using UnityEngine;

namespace CatchFire
{
    public class CharacterAnimationHandler : IAnimationHandler
    {
        readonly CharacterData data;
        readonly Animator animator;
        readonly int animIDGrounded;
        readonly int animIDJump;
        readonly int animIDFreeFall;
        readonly int animIDSpeed;
        readonly int animIDMotionSpeed;
        float blend;

        public CharacterAnimationHandler(CharacterData data, Animator animator)
        {
            this.data = data;
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

        public void SetSpeed(float targetSpeed)
        {
            blend = Maths.ExpDecay(blend, targetSpeed, data.speedChangeRate, Time.deltaTime);
            if (blend < 0.01f) blend = 0f;
            animator.SetFloat(animIDSpeed, blend);
        }

        public void SetMotionSpeed(float motionSpeed) => animator.SetFloat(animIDMotionSpeed, motionSpeed);
    }
}