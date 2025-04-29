using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CatchFire
{
    public class CharactersManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] Transform defaultSpawn;
        [SerializeField] Transform spawnGroup;
        [SerializeField] CharacterPersistentData[] charactersData;
        [Header("VFX")]
        [SerializeField] float fadeDuration = 0.3f;
        [SerializeField] Image fadeOverlay;
        int currentPlayerIndex = 0;
        bool isSwitching = false;
        InputProvider inputProvider;

        void Awake()
        {
            AssignInput();
            SetVars();
            InitializeCharacters();
        }

        void AssignInput()
        {
            inputProvider = new InputProvider();
            inputProvider.SwitchActionSetup();
        }

        void SetVars() => fadeOverlay.color = new Color(0f, 0f, 0f, 0f);

        void InitializeCharacters()
        {
            for (var i = 0; i < charactersData.Length; i++)
            {
                charactersData[i].instance = Instantiate(
                    charactersData[i].prefab,
                    defaultSpawn.position,
                    defaultSpawn.rotation,
                    spawnGroup);
                charactersData[i].instance.SetActive(i == 0);
            }
        }

        void OnEnable() => inputProvider.SwitchCharacterPressed.started += SwitchCharacter;

        void OnDisable() => inputProvider.SwitchCharacterPressed.started -= SwitchCharacter;

        void SwitchCharacter(InputAction.CallbackContext context) => StartCoroutine(SwitchCharacterRoutine());

        IEnumerator SwitchCharacterRoutine()
        {
            if (charactersData.Length <= 1 || isSwitching) yield break;

            isSwitching = true;

            //Fade out
            if (fadeOverlay != null)
                yield return StartCoroutine(Fade(1f)); 

            var lastPosition = charactersData[currentPlayerIndex].instance.transform.position;

            //Switch characters
            charactersData[currentPlayerIndex].instance.SetActive(false);
            currentPlayerIndex = (currentPlayerIndex + 1) % charactersData.Length;

            //Activate new character at previous position
            charactersData[currentPlayerIndex].instance.SetActive(true);
            charactersData[currentPlayerIndex].instance.transform.position = lastPosition;

            //Fade in
            if (fadeOverlay != null)
                yield return StartCoroutine(Fade(0f));

            isSwitching = false;
        }

        IEnumerator Fade(float targetAlpha)
        {
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
    }
}

//Implement add/remove characters by conditions
//Implement detection by check others
//Implement UI