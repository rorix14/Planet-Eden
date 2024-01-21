using System.Collections.Generic;
using RPG.Quest;
using UnityEngine.Playables;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class StoryManager : SceneTransition, ISaveable
    {
        [SerializeField] GameObject cutSceneConteiner = null;
        private List<QuestGiver> questGivers;
        private bool hasPlayedCutScene = false;

        private void Awake() => questGivers = new List<QuestGiver>(FindObjectsOfType<QuestGiver>());

        private void Start()
        {
            if (!hasPlayedCutScene)
            {
                cutSceneConteiner.GetComponentInChildren<PlayableDirector>().Play();
                hasPlayedCutScene = true;
            }
        }

        private void ChangeStoryScene()
        {
            foreach (QuestGiver questGiver in questGivers)
                if (!questGiver.QuestCompleated) return;

            StartCoroutine(Transition());
        }

        private void OnEnable() => QuestGiver.OnquestCompleated += ChangeStoryScene;

        private void OnDisable() => QuestGiver.OnquestCompleated -= ChangeStoryScene;

        public object CaptureState() => hasPlayedCutScene;

        public void RestoreState(object state) => hasPlayedCutScene = (bool)state;
    }
}