using KBCore.Refs;
using UnityEngine;

namespace CatchFire
{
    public class Hoverable : MonoBehaviour, IHoverable
    {
        [SerializeField] string tooltip;
        [SerializeField] Transform tooltipTransform;
        [SerializeField, Self] MeshRenderer meshRenderer;
        Material myMat;

        public Transform TooltipTransform => tooltipTransform;
        public string Tooltip
        {
            get => tooltip;
            set => tooltip = value;
        }

        void Start() => myMat = meshRenderer.material;

        public void OnHoverStart(Material hoverMat) => meshRenderer.material = hoverMat;

        public void OnHoverEnd() => meshRenderer.material = myMat;
    }
}
