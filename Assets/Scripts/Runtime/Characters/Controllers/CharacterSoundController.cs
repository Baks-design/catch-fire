using UnityEngine;

namespace CatchFire
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] Transform tr;
        [SerializeField] SoundData[] footstepSoundsData;
        [SerializeField] SoundData[] landSoundsData;
        SoundBuilder soundBuilder;
        ISoundService soundService;

        void Start()
        {
            ServiceLocator.Global.Get(out soundService);
            soundBuilder = soundService.CreateSoundBuilder();
        }

        internal void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && footstepSoundsData.Length > 0)
            {
                var index = Random.Range(0, footstepSoundsData.Length);
                soundBuilder
                    .WithRandomPitch()
                    .WithPosition(tr.position)
                    .Play(footstepSoundsData[index]);
            }
        }

        internal void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && landSoundsData.Length > 0)
            {
                var index = Random.Range(0, landSoundsData.Length);
                soundBuilder
                    .WithRandomPitch()
                    .WithPosition(tr.position)
                    .Play(landSoundsData[index]);
            }
        }
    }
}