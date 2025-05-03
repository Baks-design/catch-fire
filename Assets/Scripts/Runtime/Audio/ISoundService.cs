namespace CatchFire
{
    public interface ISoundService
    {
        SoundBuilder CreateSoundBuilder();
        SoundEmitter Get();
        bool CanPlaySound(SoundData data);
        void StopAll();
        void ReturnToPool(SoundEmitter soundEmitter);
    }
}