using UnityEngine;

namespace CatchFire
{    
    public interface IHoverable
    {
        string Tooltip { get; set;}
        Transform TooltipTransform { get; }

        void OnHoverStart(Material hoverMat);
        void OnHoverEnd();
    }
}
