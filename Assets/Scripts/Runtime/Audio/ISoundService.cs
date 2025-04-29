namespace CatchFire
{
    public interface ISoundService 
    {
        SoundBuilder CreateSoundBuilder();
        SoundEmitter Get();
        void ReturnToPool(SoundEmitter soundEmitter);
    }
}