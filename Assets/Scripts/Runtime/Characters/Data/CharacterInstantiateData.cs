using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Data/CharacterInstantiateData")]
    public class CharacterInstantiateData : ScriptableObject
    {
        public GameObject prefab;
        public GameObject instance;
    }
}