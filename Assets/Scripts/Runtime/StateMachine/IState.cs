namespace CatchFire
{
    public interface IState
    {
        void OnEnter() { }
        void FixedUpdate() { }
        void Update() { }
        void OnExit() { }
    }
}