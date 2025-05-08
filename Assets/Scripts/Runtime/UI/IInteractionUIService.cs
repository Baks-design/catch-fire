namespace CatchFire
{
    public interface IInteractionUIService
    {
        void SetTooltip(string tooltip);
        void UpdateProgressBar(float fillAmount);
        void ResetUI();
    }
}
