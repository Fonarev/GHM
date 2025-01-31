using Assets.GameMains.Scripts.AudiosSources;

using System.Collections;

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
            if (AudioManager.instance.isBGMusic)
                settingAudio[0].FindSelectableOnDown();
            else
                settingAudio[0].FindSelectableOnUp();

            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}