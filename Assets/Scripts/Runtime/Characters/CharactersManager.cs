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
        [SerializeField] Transform charactersGroup;
        [SerializeField] CharacterPersistentData[] charactersData;
        [SerializeField] Image fadeOverlay;
        [Header("Settings")]
        [SerializeField] float fadeDuration = 0.3f;
        int currentPlayerIndex = 0;
        Vector3 sharedLastPosition;
        Quaternion sharedLastRotation;
        bool isSwitching = false; // Prevents overlapping switches
        InputProvider inputProvider;

        void Awake()
        {
            inputProvider = new InputProvider(); //TODO: mOVE TO own class

            fadeOverlay.color = new Color(0f, 0f, 0f, 0f);

            sharedLastPosition = defaultSpawn.position;
            sharedLastRotation = defaultSpawn.rotation;

            InitializeCharacters();
        }

        void InitializeCharacters()
        {
            for (var i = 0; i < charactersData.Length; i++)
            {
                // Instantiate all characters but only activate the first one
                var shouldActivate = i == 0;

                charactersData[i].instance = Instantiate(
                    charactersData[i].prefab,
                    defaultSpawn.position,
                    defaultSpawn.rotation,
                    charactersGroup
                );

                charactersData[i].instance.SetActive(shouldActivate);
                charactersData[i].isActive = shouldActivate;

                if (shouldActivate)
                {
                    SetupCameraFollow(charactersData[i].instance);
                    currentPlayerIndex = i;
                }
            }
        }

        void OnEnable() => inputProvider.SwitchCharacterPressed.started += SwitchCharacter;

        void OnDisable() => inputProvider.SwitchCharacterPressed.started -= SwitchCharacter;

        void SwitchCharacter(InputAction.CallbackContext context) => StartCoroutine(SwitchWithTransition());

        IEnumerator SwitchWithTransition()
        {            
            if (charactersData.Length <= 1 || isSwitching) yield break;

            isSwitching = true;

            // 1. Save and deactivate current
            if (charactersData[currentPlayerIndex].instance != null)
            {
                sharedLastPosition = charactersData[currentPlayerIndex].instance.transform.position;
                sharedLastRotation = charactersData[currentPlayerIndex].instance.transform.rotation;
                charactersData[currentPlayerIndex].instance.SetActive(false);
                charactersData[currentPlayerIndex].isActive = false;
            }

            // 2. Fade out
            if (fadeOverlay != null)
                yield return StartCoroutine(FadeScreen(1f));
            else
                yield return Helpers.GetWaitForSeconds(fadeDuration / 2f);

            // 3. Switch index
            currentPlayerIndex = (currentPlayerIndex + 1) % charactersData.Length;

            // 4. Activate new character
            var newChar = charactersData[currentPlayerIndex];
            if (newChar.instance == null)
            {
                newChar.instance = Instantiate(
                    newChar.prefab,
                    sharedLastPosition,
                    sharedLastRotation,
                    charactersGroup
                );
                SetupCameraFollow(newChar.instance);
            }
            else
                newChar.instance.transform.SetPositionAndRotation(sharedLastPosition, sharedLastRotation);

            newChar.instance.SetActive(true);
            newChar.isActive = true;

            // 5. Fade in
            if (fadeOverlay != null)
                yield return StartCoroutine(FadeScreen(0f));

            isSwitching = false;
        }

        void SetupCameraFollow(GameObject target) => cinemachine.Target.TrackingTarget = target.transform;

        IEnumerator FadeScreen(float targetAlpha)
        {
            if (fadeOverlay == null) yield break;

            var fadeColor = fadeOverlay.color;
            var currentAlpha = fadeColor.a;
            var remaining = Mathf.Abs(targetAlpha - currentAlpha);
            // Half-life set to reach 99% complete in fadeDuration
            var halfLife = fadeDuration * 0.1448f; // -ln(0.01)/ln(2)

            const float epsilon = 0.001f; // Completion threshold
            while (remaining > epsilon)
            {
                currentAlpha = Maths.ExpDecay(currentAlpha, targetAlpha, Time.deltaTime, halfLife);
                fadeColor.a = currentAlpha;
                fadeOverlay.color = fadeColor;
                remaining = Mathf.Abs(targetAlpha - currentAlpha);
                yield return null;
            }

            fadeColor.a = targetAlpha;
            fadeOverlay.color = fadeColor;
        }
    }
}

//Implement add/remove characters by conditions
//Implement detection by check others
//Implement UI