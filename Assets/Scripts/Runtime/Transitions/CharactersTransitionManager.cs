using System.Collections;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharactersTransitionManager : MonoBehaviour
    {
        [SerializeField, Child] CharactersVisualTransition visualTransition;
        [SerializeField, Child] CharactersSoundTransition soundTransition;
        int currentPlayerIndex = 0;
        bool isSwitching = false;
        IPlayerMapInput inputProvider;
        ISpawnService spawnService;

        void Awake()
        {
            DontDestroyOnLoad(this);
            AssignInput();
        }

        void AssignInput()
        {
            inputProvider = new PlayerMapInputProvider();
            inputProvider.SwitchActionSetup();
        }

        void Start() => ServiceLocator.Global.Get(out spawnService);

        void OnEnable() => inputProvider.SwitchCharacterPressed.started += SwitchCharacter;

        void OnDisable() => inputProvider.SwitchCharacterPressed.started -= SwitchCharacter;

        void SwitchCharacter(InputAction.CallbackContext context) => StartCoroutine(SwitchCharacterRoutine());

        IEnumerator SwitchCharacterRoutine()
        {
            if (spawnService.GetCharactersData.Length <= 1 || isSwitching) yield break;

            isSwitching = true;

            //Fade out
            if (visualTransition.FadeOverlay != null)
                yield return StartCoroutine(visualTransition.Fade(1f));

            var lastPosition = spawnService.GetCharactersData[currentPlayerIndex].instance.transform.position;

            //Play Sound
            soundTransition.PlayTransitionSound();

            //Switch characters
            spawnService.GetCharactersData[currentPlayerIndex].instance.SetActive(false);
            currentPlayerIndex = (currentPlayerIndex + 1) % spawnService.GetCharactersData.Length;

            //Activate new character at previous position
            spawnService.GetCharactersData[currentPlayerIndex].instance.SetActive(true);
            spawnService.GetCharactersData[currentPlayerIndex].instance.transform.position = lastPosition;

            //Fade in
            if (visualTransition.FadeOverlay != null)
                yield return StartCoroutine(visualTransition.Fade(0f));

            isSwitching = false;
        }
    }
}