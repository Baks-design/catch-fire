using UnityEngine;
using System.Collections.Generic;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Data Registry")]
    public class CharacterDataRegistry : ScriptableObject
    {
        public List<CharacterData> allCharacters = new();

        public CharacterData GetCharacterById(string id)
        {
            foreach (var character in allCharacters)
                if (character.characterID == id)
                    return character;
            return null;
        }
    }
}