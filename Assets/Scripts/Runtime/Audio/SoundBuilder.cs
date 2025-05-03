using UnityEngine;

namespace CatchFire
{
    public class SoundBuilder
    {
        public enum SoundTypes
        {
            Footstep,
            Land
        }
        readonly SoundManager soundManager;
        Transform instantatiateParent;
        Vector3 position = Vector3.zero;
        bool randomPitch = false;
        bool assignParent = false;

        public SoundBuilder(SoundManager soundManager) => this.soundManager = soundManager;

        public SoundBuilder WithPosition(Vector3 position)
        {
            this.position = position;
            return this;
        }

        public SoundBuilder WithRandomPitch()
        {
            randomPitch = true;
            return this;
        }

        public SoundBuilder WithAssignParent(SoundTypes types)
        {
            assignParent = true;

            switch (types)
            {
                case SoundTypes.Footstep:
                    instantatiateParent = soundManager.ParentGroups.footstepsGroup;
                    break;
                case SoundTypes.Land:
                    instantatiateParent = soundManager.ParentGroups.landGroup;
                    break;
            }

            return this;
        }

        public void Play(SoundData soundData)
        {
            if (soundData == null)
            {
                Debug.LogError("SoundData is null");
                return;
            }
            if (!soundManager.CanPlaySound(soundData)) return;

            var soundEmitter = soundManager.Get();
            soundEmitter.Initialize(soundData);
            soundEmitter.transform.position = position;
            if (assignParent)
                soundEmitter.transform.parent = instantatiateParent.transform;
            else
                soundEmitter.transform.parent = soundManager.transform;
            if (randomPitch)
                soundEmitter.WithRandomPitch();
            if (soundData.frequentSound)
                soundEmitter.Node = soundManager.FrequentSoundEmitters.AddLast(soundEmitter);
            soundEmitter.Play();
        }
    }
}