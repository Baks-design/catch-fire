using System.Collections.Generic;
using UnityEngine;

namespace CatchFire
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayers;
        [SerializeField] float surfaceCheckDistance = 0.1f;
        [SerializeField] float surfaceCheckRadius = 0.1f;
        [SerializeField] FootstepSurfaceData[] surfaceData;
        Dictionary<SurfaceType, FootstepSurfaceData> surfaceLookup;
        Transform tr;
        SoundBuilder soundBuilder;
        ISoundService soundService;
        RaycastHit cachedHit;
        FootstepSurfaceData cachedSurface;
        float lastCheckTime;
        const float SURFACECHECKCOOLDOWN = 0.1f;

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

        void PlayFootstepSound(FootstepSurfaceData surface)
        {
            if (surface == null) return;

            var clip = surface.footstepSounds[Random.Range(0, surface.footstepSounds.Length)];
            soundBuilder
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(clip);
        }

        void PlayLandSound(FootstepSurfaceData surface)
        {
            if (surface == null) return;

            var clip = surface.landSounds[Random.Range(0, surface.landSounds.Length)];
            soundBuilder
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(clip);
        }

        FootstepSurfaceData GetCurrentSurface() //FIXME: iNIT Pool weird
        {
            if (Time.time - lastCheckTime < SURFACECHECKCOOLDOWN)
                return cachedSurface;

            lastCheckTime = Time.time;

            if (surfaceLookup == null || surfaceLookup.Count == 0)
            {
                cachedSurface = default;
                return default;
            }

            if (Physics.SphereCast(
                tr.position,
                surfaceCheckRadius,
                Vector3.down,
                out cachedHit,
                surfaceCheckDistance,
                groundLayers))
            {
                if (cachedHit.collider.TryGetComponent(out SurfaceTag surfaceTag) &&
                    surfaceLookup.TryGetValue(surfaceTag.SurfaceType, out var data))
                {
                    cachedSurface = data;
                    return data;
                }
            }

            cachedSurface = default;
            return default;
        }
    }
}