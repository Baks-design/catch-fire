using UnityEngine;

namespace CatchFire
{
    public enum SurfaceType
    {
        Concrete,
        Grass
    }

    public class SurfaceTag : MonoBehaviour
    {
        [field: SerializeField]
        public SurfaceType SurfaceType { get; set; } = SurfaceType.Concrete;
    }
}