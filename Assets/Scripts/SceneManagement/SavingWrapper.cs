using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;
using RPG.Quest;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] private float startGameFadeInTime = 0.2f;
        private const string defaultSaveFile = "save";
        private SavingSystem savingSystem;
        private QuestsManager questsManager = null;

        public SavingSystem SavingSystem => savingSystem;
        public string DefaultSaveFile => defaultSaveFile;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            questsManager = FindObjectOfType<QuestsManager>();
            // LOAD CURRENT SCENE TO AVOID RACE CONDITIONS
            //StartCoroutine(LoadLastScene());
        }

        public void LoadSceneFromMenu(float fadeIn = -1f)
        {
            if (fadeIn == -1) fadeIn = startGameFadeInTime;
            StartCoroutine(LoadLastScene(fadeIn));
        }

        public void GoToMainMenu() => StartCoroutine(LoadMainMenu());

        public IEnumerator LoadLastScene(float fadeIn)
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.Fade(1, fadeIn);

            yield return savingSystem.LoadLastScene(defaultSaveFile);
            //PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            //player.enabled = false;
            Save();
            yield return fader.Fade(0, startGameFadeInTime);
            // player.enabled = true;
        }

        public IEnumerator LoadMainMenu()
        {
            Fader fader = FindObjectOfType<Fader>();

            yield return fader.Fade(1, startGameFadeInTime);

            yield return SceneManager.LoadSceneAsync((int)RPGScenes.MAIN_MENU);

            questsManager.ResetQuestManager();
            yield return fader.Fade(0, startGameFadeInTime);
        }

        void Update()
        {
            // FOR TESTING PORPUSES 
            if (Input.GetKeyDown(KeyCode.Z)) Load();

            if (Input.GetKeyDown(KeyCode.X)) Save();

            if (Input.GetKeyDown(KeyCode.C)) Delete();
        }

        public void Delete() => savingSystem.Delete(defaultSaveFile);

        public void Save() => savingSystem.Save(defaultSaveFile);

        public void Load() => savingSystem.Load(defaultSaveFile);
    }
}