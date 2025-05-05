namespace CatchFire
{
    public interface ICharacterGravity
    {
        bool FreeFall { get; }
        float VerticalVelocity { get; }
        float InAirTimer { get; }

        void ApplyVerticalVelocity();
    }
}