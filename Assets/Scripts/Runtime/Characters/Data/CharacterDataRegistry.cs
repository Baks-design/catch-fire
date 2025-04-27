using UnityEngine;
using System.Collections.Generic;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Data Registry")]
    public class CharacterDataRegistry : ScriptableObject
    {
        public List<CharacterData> allCharacters = new();

        public CharacterData GetCharacterById(string id) => allCharacters.Find(c => c.characterID == id);
    }
}