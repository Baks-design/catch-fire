using UnityEngine;

namespace CatchFire
{
    public enum SurfaceType
    {
        Rock,
        Tile
    }

    public class SurfaceTag : MonoBehaviour
    {
        [field: SerializeField]
        public SurfaceType SurfaceType { get; private set; }
    }
}