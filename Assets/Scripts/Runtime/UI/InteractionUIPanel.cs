using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CatchFire
{
    public class InteractionUIPanel : MonoBehaviour, IInteractionUIService
    {
        [SerializeField] Image progressBar;
        [SerializeField] TMP_Text tooltipText;

        void Awake() => ServiceLocator.Global.Register<IInteractionUIService>(this);

        public void SetTooltip(string tooltip) => tooltipText.SetText(tooltip);

        public void UpdateProgressBar(float fillAmount) => progressBar.fillAmount = fillAmount;

        public void ResetUI()
        {
            progressBar.fillAmount = 0f;
            tooltipText.SetText("");
        }
    }
}
