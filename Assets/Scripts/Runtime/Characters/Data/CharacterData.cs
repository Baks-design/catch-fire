using UnityEngine;

namespace CatchFire
{
    public abstract class CharacterData : ScriptableObject
    {
        [Header("Base Properties")]
        public string characterID;
        public GameObject prefab;
        public Sprite icon;
        public float baseHealth = 100f;
        public float baseSpeed = 5f;
        public AudioClip spawnSound;
    }
}