using UnityEngine;

namespace CatchFire
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField] CharacterMovementController movement;
        [SerializeField] Animator animator;
        IAnimationHandler animationHandler;

        void Awake()
        {
            animationHandler = new CharacterAnimationHandler(animator);
            animationHandler.SetGrounded(true);
        }

        void Update()
        {
            animationHandler.SetGrounded(movement.GroundChecker.Grounded);
            animationHandler.SetSpeed(movement.MovementHandler.CurrentSpeed);
            animationHandler.SetMotionSpeed(movement.MovementHandler.CurrentInputMagnitude);
            animationHandler.SetJump(movement.JumpHandler.JumpTriggered);
            animationHandler.SetFreeFall(movement.JumpHandler.FreeFall);
        }
    }
}