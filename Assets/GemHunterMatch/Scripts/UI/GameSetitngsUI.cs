using Assets.GameMains.Scripts.AudiosSources;

using System;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class GameSetitngsUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Toggle[] settingAudio;

        public void Init()
        {
            settingAudio[0].isOn = AudioManager.instance.isBGMusic;
            settingAudio[0].onValueChanged.AddListener(OnClick);

            settingAudio[1].isOn = AudioManager.instance.isEffectAudio;
            settingAudio[1].onValueChanged.AddListener((value)=> { AudioManager.instance.isEffectAudio = value; });

            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnClick(bool isEnable)
        {
            AudioManager.instance.isBGMusic = isEnable;
        }
    }
}