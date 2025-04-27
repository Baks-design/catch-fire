using UnityEngine;
using System.Collections.Generic;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Warrior Data")]
    public class WarriorData : CharacterData
    {
        [Header("Warrior Specific")]
        public float attackDamage = 20f;
        public float defense = 15f;
    }
}