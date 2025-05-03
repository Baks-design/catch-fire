namespace CatchFire
{
    public interface ICharacterGravity
    {
        bool FreeFall { get; }
        float VerticalVelocity { get; }
        float FallTimeoutDelta { get; }

        void ApplyVerticalVelocity();
    }
}