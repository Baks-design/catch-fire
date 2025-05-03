namespace CatchFire
{
    public interface IMovable
    {
        bool IsMoving { get; }
        bool IsRunning { get; }
        float CurrentSpeed { get; }
        float CurrentInputMagnitude { get; }
        float TargetSpeed { get; }

        void ApplySprint(bool context);
        void HandleMovement();
    }
}