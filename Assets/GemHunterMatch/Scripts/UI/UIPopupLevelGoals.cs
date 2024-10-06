using Assets.AssetLoaders;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIPopupLevelGoals : MonoBehaviour
    {
        public RectTransform container;
        public float timeShow = 1.5f;
        public Animation anim => GetComponent<Animation>();
        private List<UIGoalEntry> goalEntries = new();
        public void Init(LevelConfig level)
        {
            foreach (var goal in level.Goals)
            {
                StartCoroutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", container, op =>
                {
                    op.Init(goal);
                    goalEntries.Add(op);
                }));
            }
            Show();
        }
        public void Show()
        {
            StartCoroutine(TimeDown());
        }
        public IEnumerator TimeDown()
        {
            var timeDown = timeShow;
            gameObject.SetActive(true);

            while(timeDown > 0)
            {
                timeDown -= Time.deltaTime;
                yield return timeDown;
            }
           
            Hide();
        }
        public void Hide() 
        {
            gameObject.SetActive(false);
            foreach (var entry in goalEntries)
            {
                Addressables.ReleaseInstance(entry.gameObject);
            }
            Addressables.ReleaseInstance(gameObject);
        }
    }
}