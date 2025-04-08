using Assets.GameMains.Scripts;
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
        [SerializeField] private Toggle[] settingAudio;
        private GlobalMediator mediator;

        public void Init(GlobalMediator mediator,  bool isGamePlay = false, Action<OpenButtonType> onClick = null)
        {
            this.mediator = mediator;
            settingAudio[0].isOn = mediator.IsEnableBackgroundMusic;
            settingAudio[0].onValueChanged.AddListener(OnClick);

            settingAudio[1].isOn = mediator.IsEnableAudioEffect;
            settingAudio[1].onValueChanged.AddListener((value)=> { mediator.IsEnableAudioEffect = value; });

            closeButton.Init(mediator,ButtonType.Close, gameObject);

            if (isGamePlay)
            {
                menuButton.Init(mediator,ButtonType.Menu);
            }
            else
            {
                menuButton.gameObject.SetActive(false);
            }
           
        }


        private void OnClick(bool isEnable)
        {
            mediator.IsEnableBackgroundMusic = isEnable;
            AudioManager.instance.PlayBackgroundMusic("harp", false);
        }
    }
}