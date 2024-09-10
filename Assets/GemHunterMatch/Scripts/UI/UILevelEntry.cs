using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

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
    
       public void Init(int level,bool isLock)
       {
            this.level = level;
            button.interactable = isLock;
            Lock.gameObject.SetActive(isLock);
            numberLevel.text = this.level.ToString();
            button.onClick.AddListener(OnClick);
       }

        private void OnClick()
        {
            AudioManager.instance.PlayEffect(EffectClip.click);
        }
    }
}