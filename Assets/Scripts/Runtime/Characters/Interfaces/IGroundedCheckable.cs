namespace CatchFire
{
    public interface IGroundedCheckable
    {
        bool Grounded { get; }

        void CheckGrounded();
    }
}