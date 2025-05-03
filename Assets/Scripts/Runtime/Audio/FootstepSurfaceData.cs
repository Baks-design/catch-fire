using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Audio/Library/Footsteps")]
    public class FootstepsSoundLibrary : ScriptableObject
    {
        public SurfaceType surfaceType;
        public SoundData[] footstepSounds;
        public SoundData[] landSounds;
    }
}