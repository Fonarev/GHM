using Assets.AssetLoaders;
using Assets.GameMains.Scripts.AudiosSources;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIPopupLevelGoals : MonoBehaviour,IPointerClickHandler
    {
        public RectTransform container;
        public float timeShow = 3.0f;
        public Animation anim => GetComponent<Animation>();
        private List<UIGoalEntry> goalEntries = new();

        public void Init(GamePlay gamePlay)
        {
            foreach (var goal in gamePlay.Goals)
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
            anim.Play();
            StartCoroutine(TimeDown());
        }
        public IEnumerator TimeDown()
        {
            var timeDown = timeShow;
            gameObject.SetActive(true);
           
            while (timeDown > 0)
            {
                timeDown -= Time.deltaTime;
                yield return timeDown;
            }
           
            Hide();
        }

        public void Hide() 
        {
            anim.PlayQueued("ClosePopupGoalsLevel");
        }

        public void AnimEventClose()
        {
            gameObject.SetActive(false);
            foreach (var entry in goalEntries)
            {
                Addressables.ReleaseInstance(entry.gameObject);
            }
            Addressables.ReleaseInstance(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioManager.instance.PlayEffect("bubble");
            Hide();
        }
    }
}