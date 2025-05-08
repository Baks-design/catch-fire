using UnityEngine;

namespace CatchFire
{
    [CreateAssetMenu(menuName = "Characters/Data")]
    public class CharacterData : CharacterDataBase
    {
        #region Camera
        [Header("Camera Movement Settings")]
        [Min(30f)] public float topClamp = 70f;
        [Min(0f)] public float bottomClamp = -30f;
        [Min(1f)] public float smoothAmount = 40f;
        [Range(1f, 25f)] public float cameraSpeedDecay = 16f;

        [Header("Camera Zoom Settings")]
        [Range(20f, 60f)] public float zoomFOV = 40f;
        public float runFOVDecay = 16f;
        public AnimationCurve zoomCurve = new();
        public float zoomTransitionDuration = 0.25f;
        [Range(60f, 100f)] public float runFOV = 70f;
        public float zoomFOVDecay = 16f;
        public AnimationCurve runCurve = new();
        public float runTransitionDuration = 0.75f;
        public float runReturnTransitionDuration = 0.5f;

        [Header("Camera Sway Settings")]
        public float swayAmount = 1f;
        public float swaySpeed = 1f;
        public float returnSpeed = 3f;
        public float changeDirectionMultiplier = 4f;
        public float swayDecay = 16f;
        [Range(0f, 1f)] public float moveBackwardsSpeedPercent = 0.5f;
        [Range(0f, 1f)] public float moveSideSpeedPercent = 0.7f;
        public AnimationCurve swayCurve = new();

        [Header("Breathing Settings")]
        public PerlinNoiseData noiseData;
        public bool x = false;
        public bool y = true;
        public bool z = false;
        #endregion

        #region Movement
        [Header("Move")]
        [Min(2f)] public float moveSpeed = 3f;
        [Min(5f)] public float sprintSpeed = 6f;
        [Min(5f)] public float jumpSpeed = 3f;

        [Header("Rotation")]
        public float smoothRotateSpeed = 5f;
        public float rotationDecay = 16f;

        [Header("Run")]
        [Range(-1f, 1f)] public float canRunThreshold = 0.7f;
        [Range(0.1f, 1f)] public float walkToRunDecay = 0.5f;
        public AnimationCurve runTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Falling")]
        public float gravityMultiplier = 2.5f;
        public float stickToGroundForce = 5f;

        [Header("Smooth")]
        public float smoothFinalDirectionSpeed = 10f;
        [Range(1f, 25f)] public float smoothDirectionalDecay = 16f;
        public float smoothInputSpeed = 10f;
        [Range(1f, 25f)] public float smoothInputDecay = 16f;
        public float smoothVelocitySpeed = 3f;
        [Range(1f, 25f)] public float smoothSpeedDecay = 16f;
        public float smoothHeadBobSpeed = 5f;
        [Range(1f, 25f)] public float smoothHeadBobSpeedDecay = 16f;

        [Header("Crouch Settings")]
        [Range(0.2f, 0.9f)] public float crouchPercent = 0.6f;
        [Min(1f)] public float crouchDecay = 16f;
        public float crouchTransitionDuration = 1f;
        public AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Landing Settings")]
        [Range(0.05f, 0.5f)] public float lowLandAmount = 0.1f;
        [Range(0.2f, 0.9f)] public float highLandAmount = 0.6f;
        public float landTimer = 0.5f;
        public float landDuration = 1f;
        public AnimationCurve landCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        #endregion

        #region Checks
        [Header("Grounding")]
        public LayerMask groundLayers;
        public float groundedOffset = -0.25f;
        public float surfaceCheckDistance = 0.1f;
        [Min(50)] public float maxSlopeAngle = 60f;

        [Header("Obstacle")]
        public LayerMask obstacleLayers = ~0;
        public float obstacleOffset = 0.4f;
        public float rayObstacleLength = 0.4f;
        public float rayObstacleSphereRadius = 0.2f;

        [Header("Roof")]
        public LayerMask roofLayer = ~0;
        public float roofOffset = -0.25f;
        public float roofCheckDistance = 0.1f;

        [Header("Push")]
        public LayerMask pushLayers;
        public bool canPush = true;
        [Range(0.5f, 5f)] public float strength = 5f;
        #endregion

        #region Audio
        [Header("Audio")]
        [Range(0.5f, 1f)] public float walkStepInterval = 0.5f;
        [Range(0.3f, 1f)] public float runStepInterval = 0.3f;
        #endregion

        #region Audio
        [Header("Interaction")]
        public LayerMask interactableLayer = ~0;
        public float rayDistance = 0.3f;
        public float raySphereRadius = 0.3f;
        #endregion
    }
}