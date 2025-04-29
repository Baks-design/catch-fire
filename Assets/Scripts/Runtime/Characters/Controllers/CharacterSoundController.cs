using System.Collections.Generic;
using UnityEngine;

namespace CatchFire
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] float surfaceCheckDistance = 0.2f;
        [SerializeField] Vector3 footstepCheckOffset = new(0f, 0.1f, 0f);
        [SerializeField] FootstepSurfaceData[] surfaceData;
        Dictionary<SurfaceType, FootstepSurfaceData> surfaceLookup;
        Transform tr;
        SoundBuilder soundBuilder;
        ISoundService soundService;

        void Awake()
        {
            tr = transform;
            InitializeSurfaceDictionary();
        }

        void Start()
        {
            ServiceLocator.Global.Get(out soundService);
            soundBuilder = soundService.CreateSoundBuilder();
        }

        void InitializeSurfaceDictionary()
        {
            surfaceLookup = new Dictionary<SurfaceType, FootstepSurfaceData>();

            foreach (var data in surfaceData)
                surfaceLookup[data.surfaceType] = data;
        }

        internal void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight <= 0.5f) return;

            PlayFootstepSound(GetCurrentSurface());
        }

        internal void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight <= 0.5f) return;

            PlayLandSound(GetCurrentSurface());
        }

        FootstepSurfaceData GetCurrentSurface()
        {
            FootstepSurfaceData surfaceData = default;
            
            if (Physics.Raycast(
                tr.position + footstepCheckOffset,
                Vector3.down,
                out var hit,
                surfaceCheckDistance + footstepCheckOffset.y,
                Physics.AllLayers))
            {
                hit.collider.TryGetComponent<SurfaceTag>(out var surfaceTag);

                if (surfaceTag != null && surfaceLookup.ContainsKey(surfaceTag.SurfaceType))
                    surfaceData = surfaceLookup[surfaceTag.SurfaceType];
            }

            return surfaceData;
        }

        void PlayFootstepSound(FootstepSurfaceData surface)
        {
            var clip = surface.footstepSounds[Random.Range(0, surface.footstepSounds.Length)];
            soundBuilder
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(clip);
        }

        void PlayLandSound(FootstepSurfaceData surface)
        {
            var clip = surface.landSounds[Random.Range(0, surface.landSounds.Length)];
            soundBuilder
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(clip);
        }
    }
}