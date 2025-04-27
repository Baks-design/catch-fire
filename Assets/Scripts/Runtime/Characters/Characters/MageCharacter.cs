using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class MageCharacter : CharacterBase
    {
        [SerializeField] MageData mageData;
        Vector3 playerVelocity;
        InputAction moveAction;

        protected override void Initialize(CharacterData data) => base.Initialize(data);

        void Awake() => moveAction = InputSystem.actions.FindAction("Player/Move");

        void Update()
        {
            var groundedPlayer = character.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0f)
                playerVelocity.y = 0f;

            playerVelocity.y += Physics.gravity.y * Time.deltaTime;

            var direction = new Vector3(moveAction.ReadValue<Vector2>().x, 0f, moveAction.ReadValue<Vector2>().y);
            var finalMove = (direction * mageData.baseSpeed) + (playerVelocity.y * Vector3.up);

            character.Move(finalMove * Time.deltaTime);
        }
    }
}