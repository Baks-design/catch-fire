using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Mage Data")]
    public class MageData : CharacterData
    {
        [Header("Mage Specific")]
        public float mana = 100f;
        public float spellPower = 25f;
    }
}