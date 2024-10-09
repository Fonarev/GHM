using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GameMains.Scripts;
using Assets.YG.Scripts;

using System;
using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonEntry : MonoBehaviour
    {
        [SerializeField] private ButtonType type;
        private Button button => GetComponent<Button>();
        private TextMeshProUGUI nameT => GetComponentInChildren<TextMeshProUGUI>();

        internal void Init(ButtonType type)
        {
            Debug.Log(type.ToString());
            this.type = type;
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
           switch(type) 
           {
                case ButtonType.Next:
                    var nextLevel = GlobalMediator.instance.SelectLevel + 1;
                    GlobalMediator.instance.SelectedLevel(nextLevel);
                    AudioManager.instance.PlayEffect(EffectClip.click);
                    break;
                case ButtonType.Menu:
                    GlobalMediator.instance.ExitMenu();
                    AudioManager.instance.PlayEffect(EffectClip.click);
                    break;

            }
        }
    }
}