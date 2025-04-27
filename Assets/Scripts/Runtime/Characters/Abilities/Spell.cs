using System;
using UnityEngine;

namespace CatchFire
{
    [Serializable]
    public class Spell
    {
        public string spellName;
        public GameObject spellPrefab;
        public float manaCost;
    }
}