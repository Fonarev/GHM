using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GameMains.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Assets.GameMains.Scripts.Bank;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonEntry : MonoBehaviour
    {
        [SerializeField] private ButtonType type;
        private Button button => GetComponent<Button>();
        private TextMeshProUGUI nameT => GetComponentInChildren<TextMeshProUGUI>();
        private GameObject window;
        public void Init(ButtonType type, GameObject window = null)
        {
            this.type = type;
            this.window = window;
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

                case ButtonType.Close:
                    if (window != null)
                        window.SetActive(false);

                    AudioManager.instance.PlayEffect(EffectClip.click);
                    break;

                case ButtonType.Restart:
                    AudioManager.instance.PlayEffect(EffectClip.click);
                    break;

           }
        }
    }
}