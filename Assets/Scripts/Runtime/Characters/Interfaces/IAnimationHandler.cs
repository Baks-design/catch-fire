namespace CatchFire
{
    public interface IAnimationHandler
    {
        void SetGrounded(bool grounded);
        void SetJump(bool jumping);
        void SetFreeFall(bool freeFall);
        void SetSpeed(float speed);
        void SetMotionSpeed(float motionSpeed);
    }
}