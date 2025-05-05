namespace CatchFire
{
    public interface IMovementEffects
    {
        void HandleHeadBob(IGroundedCheckable grounded);
        void HandleCameraSway();
        void HandleRunFOV(IGroundedCheckable grounded);
    }
}