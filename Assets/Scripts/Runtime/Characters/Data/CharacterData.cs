using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Data")]
    public class CharacterData : CharacterDataBase
    {
        [Header("Movement")]
        public float moveSpeed = 2f;
        public float sprintSpeed = 5.335f;
        public float speedDecay = 16f;
        public float speedChangeRate = 25f;
        [Range(0f, 0.3f)] public float rotationSmoothTime = 0.12f;
        [Header("Jumping")]
        public float jumpHeight = 1.2f;
        public float gravity = -15f;
        public float jumpTimeout = 0.5f;
        public float fallTimeout = 0.15f;
        [Header("Grounding")]
        public LayerMask groundLayers;
        public float groundedOffset = -0.14f;
        public float groundedRadius = 0.28f;
        [Header("Grounding")]
        public LayerMask pushLayers;
        public bool canPush = true;
        [Range(0.5f, 5f)] public float strength = 1.1f;
    }
}