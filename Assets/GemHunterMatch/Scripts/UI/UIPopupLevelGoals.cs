using Assets.AssetLoaders;
using Assets.GameMains.Scripts.AudiosSources;

using Match3;

using System.Collections;
using System.Collections.Generic;

using TMPro;

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
        //private void Update()
        //{
        //    var maxDistance = GamePlay.Instance.visualSettings.FallAccelerationCurve.Evaluate(0) *
        //                             Time.deltaTime * GamePlay.Instance.visualSettings.FallSpeed * 2.0f;

        //    transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), maxDistance);
        //    if (transform.position == new Vector3(0, 0, 0))
        //    {
        //        transform.position = Vector3.up * GamePlay.Instance.visualSettings.BounceCurve.Evaluate(0);

        //        transform.localScale =
        //            new Vector3(1, GamePlay.Instance.visualSettings.SquishCurve.Evaluate(0), 1);
        //    }
        //}
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