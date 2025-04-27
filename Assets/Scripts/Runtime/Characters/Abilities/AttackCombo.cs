using System;
using UnityEngine;

namespace CatchFire
{
    [Serializable]
    public class AttackCombo
    {
        public string comboName;
        public KeyCode[] sequence;
        public float damageMultiplier;
    }
}