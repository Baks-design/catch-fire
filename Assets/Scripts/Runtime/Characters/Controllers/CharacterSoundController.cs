using System.Collections.Generic;
using UnityEngine;

namespace CatchFire
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] CharacterMovementController movement;
        [SerializeField] CharacterData characterData;
        [SerializeField] FootstepsSoundLibrary[] surfaceData;
        Dictionary<SurfaceType, FootstepsSoundLibrary> surfaceLookup;
        Transform tr;
        SoundBuilder soundBuilder;
        ISoundService soundService;
        float stepTimer;

        void Start()
        {
            GetReference();
            InitializeSurfaceDictionary();
        }

        void GetReference()
        {
            tr = transform;
            ServiceLocator.Global.Get(out soundService);
            soundBuilder = soundService.CreateSoundBuilder();
        }

        void InitializeSurfaceDictionary()
        {
            surfaceLookup = new Dictionary<SurfaceType, FootstepsSoundLibrary>();
            for (var i = 0; i < surfaceData.Length; i++)
                surfaceLookup[surfaceData[i].surfaceType] = surfaceData[i];
        }

        void Update()
        {
            FootstepsHandler();
            LandHandler();
        }

        void FootstepsHandler()
        {
            if (movement.GroundChecker.IsGrounded && movement.MovementHandler.IsMoving)
            {
                var currentStepInterval = GetCurrentStepInterval();
                stepTimer += Time.deltaTime;
                if (stepTimer >= currentStepInterval)
                {
                    PlayFootstepSound(GetCurrentSurface());
                    stepTimer = 0f;
                }
            }
            else
                stepTimer = 0f;
        }

        void LandHandler()
        {
            if (!movement.GroundChecker.IsLanded)
                return;
            PlayLandSound(GetCurrentSurface());
        }

        float GetCurrentStepInterval()
        {
            if (movement.MovementHandler.IsRunning)
                return characterData.runStepInterval;
            return characterData.walkStepInterval;
        }

        void PlayFootstepSound(FootstepsSoundLibrary surface)
        {
            if (surface == null) return;

            var clip = surface.footstepSounds[Random.Range(0, surface.footstepSounds.Length)];
            soundBuilder
                .WithAssignParent(SoundBuilder.SoundTypes.Footstep)
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(clip);
        }

        void PlayLandSound(FootstepsSoundLibrary surface)
        {
            if (surface == null) return;

            var clip = surface.landSounds[Random.Range(0, surface.landSounds.Length)];
            soundBuilder
                .WithAssignParent(SoundBuilder.SoundTypes.Land)
                .WithRandomPitch()
                .WithPosition(tr.position)
                .Play(clip);
        }

        FootstepsSoundLibrary GetCurrentSurface()
        {
            if (surfaceLookup == null || surfaceLookup.Count == 0)
                return default;

            if (movement.GroundChecker.IsGrounded)
            {
                var isGetComponent = movement.GroundChecker.IsHit.collider.TryGetComponent(out SurfaceTag surfaceTag);
                var isGetValue = surfaceLookup.TryGetValue(surfaceTag.SurfaceType, out var data);
                if (isGetComponent && isGetValue)
                    return data;
            }

            return default;
        }
    }
}