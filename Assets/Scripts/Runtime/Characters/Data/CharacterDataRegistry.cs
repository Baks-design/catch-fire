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
            for (var i = 0; i < allCharacters.Count; i++)
                if (allCharacters[i].characterID == id)
                    return allCharacters[i];

            return null;
        }
    }
}