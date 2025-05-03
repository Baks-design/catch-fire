using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CatchFire
{
    public class CharactersManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] Transform defaultSpawn;
        [SerializeField] Transform spawnGroup;
        [SerializeField] CharacterPersistentData[] charactersData;
        [SerializeField] CharacterTransition characterTransition;
        int currentPlayerIndex = 0;
        bool isSwitching = false;
        IPlayerMapInput inputProvider;

        void Awake()
        {
            DontDestroyOnLoad(this);
            AssignInput();
            InitializeCharacters();
        }

        void AssignInput()
        {
            inputProvider = new PlayerMapInputProvider();
            inputProvider.SwitchActionSetup();
        }

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
            if (characterTransition.FadeOverlay != null)
                yield return StartCoroutine(characterTransition.Fade(1f));

            var lastPosition = charactersData[currentPlayerIndex].instance.transform.position;

            //Switch characters
            charactersData[currentPlayerIndex].instance.SetActive(false);
            currentPlayerIndex = (currentPlayerIndex + 1) % charactersData.Length;

            //Activate new character at previous position
            charactersData[currentPlayerIndex].instance.SetActive(true);
            charactersData[currentPlayerIndex].instance.transform.position = lastPosition;

            //Fade in
            if (characterTransition.FadeOverlay != null)
                yield return StartCoroutine(characterTransition.Fade(0f));

            isSwitching = false;
        }
    }
}