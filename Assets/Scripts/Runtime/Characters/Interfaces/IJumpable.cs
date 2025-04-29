namespace CatchFire
{
    public interface IJumpable
    {
        float VerticalVelocity { get; }
        bool JumpTriggered { get; }
        bool FreeFall { get; }

        void HandleJump(bool grounded, bool jumpPressed, float deltaTime);
    }
}