using System;
using UnityEngine;

namespace CatchFire
{
    [Serializable]
    public struct PlayerData
    {
        public GameObject prefab;
        public GameObject instance;
        public bool isActive;
    }
}