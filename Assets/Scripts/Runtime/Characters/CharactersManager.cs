using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CatchFire
{
    public class CharactersManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] CinemachineCamera cinemachine;
        [SerializeField] Transform defaultSpawn;
        [SerializeField] PlayerData[] players;
        [SerializeField] Image fadeOverlay; // Optional UI fade overlay
        [Header("Settings")]
        [SerializeField] float fadeDuration = 0.3f;
        int currentPlayerIndex = 0;
        InputAction switchCharacter;
        Vector3 sharedLastPosition;
        Quaternion sharedLastRotation;
        bool isSwitching = false; // Prevents overlapping switches

        void Awake()
        {
            sharedLastPosition = defaultSpawn.position;
            sharedLastRotation = defaultSpawn.rotation;
            SpawnPlayer(0);
        }

        void OnEnable()
        {
            switchCharacter = InputSystem.actions.FindAction("Player/SwitchCharacter");
            switchCharacter.started += SwitchCharacter;
        }

        void OnDisable() => switchCharacter.started -= SwitchCharacter;

        void SwitchCharacter(InputAction.CallbackContext context)
        {
            if (players.Length <= 1 || isSwitching) return;

            StartCoroutine(SwitchWithTransition());
        }

        IEnumerator SwitchWithTransition()
        {
            isSwitching = true;

            // 1. Save current position before switching
            if (players[currentPlayerIndex].instance != null)
            {
                sharedLastPosition = players[currentPlayerIndex].instance.transform.position;
                sharedLastRotation = players[currentPlayerIndex].instance.transform.rotation;
            }

            // 2. Optional: Fade out
            if (fadeOverlay != null)
                yield return StartCoroutine(FadeScreen(1f)); // Fade to black
            else
                yield return new WaitForSeconds(fadeDuration / 2); // Minimal delay

            // 3. Destroy old character
            if (players[currentPlayerIndex].instance != null)
                Destroy(players[currentPlayerIndex].instance);

            // 4. Switch to next character
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            // 5. Spawn new character at shared position
            SpawnPlayer(currentPlayerIndex);

            // 6. Optional: Fade in
            if (fadeOverlay != null)
                yield return StartCoroutine(FadeScreen(0f)); // Fade back in

            isSwitching = false;
        }

        IEnumerator FadeScreen(float targetAlpha)
        {
            if (fadeOverlay == null) yield break;

            var startAlpha = fadeOverlay.color.a;
            var elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                var newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
                fadeOverlay.color = new Color(0f, 0f, 0f, newAlpha);
                yield return null;
            }
        }

        void SpawnPlayer(int index)
        {
            if (index < 0 || index >= players.Length) return;

            players[index].instance = Instantiate(players[index].prefab, sharedLastPosition, sharedLastRotation);
            players[index].isActive = true;

            SetupCameraFollow(players[index].instance);
        }

        void SetupCameraFollow(GameObject target) => cinemachine.Target.TrackingTarget = target.transform;
    }
}

//Implement add/remove characters by conditions
//Implement detection by check others
//Implement UI
//Add WaitFor class
//Add FadeIn Render