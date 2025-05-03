using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Data")]
    public class CharacterData : CharacterDataBase
    {
        [Header("Camera")]
        public float topClamp = 70f;
        public float bottomClamp = -30f;
        [Header("Movement")]
        public float moveSpeed = 2f;
        public float sprintSpeed = 5.335f;
        public float speedDecay = 16f;
        public float speedChangeRate = 25f;
        public float rotationSpeed = 2f;
        public float maxSlopeAngle = 60f;
        [Header("Falling")]
        public float gravity = -15f;
        public float fallTimeout = 0.15f;
        [Header("Grounding")]
        public LayerMask groundLayers;
        public float groundedOffset = -0.14f;
        public float surfaceCheckDistance = 0.1f;
        public float groundedRadius = 0.28f;
        [Header("Push")]
        public LayerMask pushLayers;
        public bool canPush = true;
        [Range(0.5f, 5f)] public float strength = 1.1f;
        [Header("Audio")]
        public float walkStepInterval = 0.5f;
        public float runStepInterval = 0.3f;
    }
}