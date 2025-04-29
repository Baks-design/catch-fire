using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Audio/SurfaceSound")]
    public class FootstepSurfaceData : ScriptableObject
    {
        public SurfaceType surfaceType;
        public SoundData[] footstepSounds;
        public SoundData[] landSounds;
    }
}