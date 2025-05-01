namespace CatchFire
{
    public interface IMovable
    {
        float CurrentSpeed { get; }
        float CurrentInputMagnitude { get; }
        float TargetSpeed { get; }

        void ApplySprint(bool context);
        void HandleMovement();
    }
}