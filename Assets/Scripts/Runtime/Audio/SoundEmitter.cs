using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CatchFire
{
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        Coroutine playingCoroutine;
        ISoundService soundService;

        public SoundData Data { get; private set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }

        void Start() => ServiceLocator.Global.Get(out soundService);

        public void Initialize(SoundData data)
        {
            Data = data;

            for (var i = 0; i < data.clip.Length; i++)
                audioSource.clip = data.clip[i];
            audioSource.outputAudioMixerGroup = data.mixerGroup;
            audioSource.loop = data.loop;
            audioSource.playOnAwake = data.playOnAwake;

            audioSource.mute = data.mute;
            audioSource.bypassEffects = data.bypassEffects;
            audioSource.bypassListenerEffects = data.bypassListenerEffects;
            audioSource.bypassReverbZones = data.bypassReverbZones;

            audioSource.priority = data.priority;
            audioSource.volume = data.volume;
            audioSource.pitch = data.pitch;
            audioSource.panStereo = data.panStereo;
            audioSource.spatialBlend = data.spatialBlend;
            audioSource.reverbZoneMix = data.reverbZoneMix;
            audioSource.dopplerLevel = data.dopplerLevel;
            audioSource.spread = data.spread;

            audioSource.minDistance = data.minDistance;
            audioSource.maxDistance = data.maxDistance;

            audioSource.ignoreListenerVolume = data.ignoreListenerVolume;
            audioSource.ignoreListenerPause = data.ignoreListenerPause;

            audioSource.rolloffMode = data.rolloffMode;
        }

        public void Play()
        {
            if (playingCoroutine != null)
                StopCoroutine(playingCoroutine);

            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return Helpers.GetWaitForRealtimeSeconds(audioSource.clip.length);
            Stop();
        }

        public void Stop()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }

            audioSource.Stop();
            soundService.ReturnToPool(this);
        }

        // Instead of applying immediately, prepare for next play
        public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
        => audioSource.pitch += Random.Range(min, max);
    }
}