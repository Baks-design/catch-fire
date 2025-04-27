using UnityEngine;

namespace CatchFire
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] protected CharacterController character;
        
        public string CharacterID { get; private set; }
        public CharacterData CharacterData { get; private set; }

        protected virtual void Initialize(CharacterData data)
        {
            CharacterData = data;
            CharacterID = data.characterID;
        }
    }
}