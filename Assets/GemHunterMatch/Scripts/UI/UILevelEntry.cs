using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using System;
using System.Collections;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UILevelEntry : MonoBehaviour
    {
        [SerializeField] private Image Lock;
        private TextMeshProUGUI numberLevel=> GetComponentInChildren<TextMeshProUGUI>();
        private int level;
       private Button button => GetComponent<Button>();
    
       public void Init(int level)
       {
            this.level = level;
            bool isLock = TryLevel(level);
            button.interactable = isLock;
            Lock.gameObject.SetActive(isLock);
            numberLevel.text = this.level.ToString();
            button.onClick.AddListener(OnClick);
       }
        private bool TryLevel(int level)
        {
            if (YandexGame.Instance.progressData.levels.ContainsKey(level))
            {
                return false;
            }
            return true;
        }
        private void OnClick()
        {
            AudioManager.instance.PlayEffect(EffectClip.click);
        }

        public void UpDateView()
        {
            bool isLock = TryLevel(level);
            button.interactable = isLock;
            Lock.gameObject.SetActive(isLock);
        }
    }
}