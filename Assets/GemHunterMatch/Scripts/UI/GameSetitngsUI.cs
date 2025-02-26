using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class GameSetitngsUI : MonoBehaviour
    {
        [SerializeField] private UIButtonEntry closeButton;
        [SerializeField] private UIButtonEntry menuButton;
        [SerializeField] private OpenWindowButton shopButton;
        [SerializeField] private OpenWindowButton dailyRewardButton;
        [SerializeField] private Toggle[] settingAudio;

        public void Init(bool isGamePlay = false, Action<OpenButtonType> onClick = null)
        {
            settingAudio[0].isOn = AudioManager.instance.isBGMusic;
            settingAudio[0].onValueChanged.AddListener(OnClick);

            settingAudio[1].isOn = AudioManager.instance.isEffectAudio;
            settingAudio[1].onValueChanged.AddListener((value)=> { AudioManager.instance.isEffectAudio = value; });

            closeButton.Init(ButtonType.Close, gameObject);

            if (isGamePlay)
            {
                menuButton.Init(ButtonType.Menu);
                dailyRewardButton.gameObject.SetActive(false);
                InitButton(onClick);
            }
            else
            {
                menuButton.gameObject.SetActive(false);
                InitButton(onClick);
                dailyRewardButton.Init((type) =>
                {
                    if (onClick != null)
                    {
                        AudioManager.instance.PlayEffect(EffectClip.click);
                        gameObject.SetActive(false);
                        onClick.Invoke(type);
                    }
                });
            }
           
        }

        private void InitButton(Action<OpenButtonType> onClick)
        {
            shopButton.Init((type) =>
            {
                if (onClick != null)
                {
                    AudioManager.instance.PlayEffect(EffectClip.click);
                    gameObject.SetActive(false);
                    onClick.Invoke(type);
                }
            });
        }

        private void OnClick(bool isEnable)
        {
            AudioManager.instance.isBGMusic = isEnable;
        }
    }
}