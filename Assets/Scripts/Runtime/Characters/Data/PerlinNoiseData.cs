using UnityEngine;

namespace CatchFire
{
    public enum TransformTarget
    {
        Position,
        Rotation,
        Both
    }
    
    [CreateAssetMenu(menuName = "CharacterData/PerlinNoiseData")]
    public class PerlinNoiseData : ScriptableObject
    {
        public TransformTarget transformTarget = TransformTarget.Rotation;
        public float amplitude = 1f;
        public float frequency = 0.5f;
    }
}