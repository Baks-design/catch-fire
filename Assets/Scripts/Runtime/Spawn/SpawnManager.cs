using KBCore.Refs;
using UnityEngine;

namespace CatchFire
{
    public class SpawnManager : MonoBehaviour, ISpawnService
    {
        [SerializeField, Anywhere] Transform spawnGroup;
        [SerializeField] Transform defaultSpawn;
        [SerializeField] CharacterInstantiateData[] charactersData;

        public CharacterInstantiateData[] GetCharactersData => charactersData;

        void Awake()
        {
            DontDestroyOnLoad(this);
            ServiceLocator.Global.Register<ISpawnService>(this);
            InitializeCharacters();
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
    }
}