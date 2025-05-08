using UnityEngine;

namespace CatchFire
{
    public class CharactersSoundTransition : MonoBehaviour
    {
        [SerializeField] SoundData transitionSound;
        Transform tr;
        SoundBuilder soundBuilder;
        ISoundService soundService;

        void Start() => GetReference();

        void GetReference()
        {
            tr = transform;
            ServiceLocator.Global.Get(out soundService);
            soundBuilder = soundService.CreateSoundBuilder();
        }

        public void PlayTransitionSound()
        {
            soundBuilder
                .WithAssignParent(SoundBuilder.SoundTypes.Land)
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(transitionSound);
        }
    }
}