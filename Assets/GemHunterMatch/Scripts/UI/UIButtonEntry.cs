using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GameMains.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Assets.GameMains.Scripts.Bank;
using Assets.YG.Scripts;

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
                    int nextLevel = GlobalMediator.instance.SelectLevel + 1;
                    Location currentLocation = YandexGame.Instance.progressData.locations[GlobalMediator.instance.SelectLocation];

                    if (nextLevel > currentLocation.endNumberLevel)
                    {
                        currentLocation.completed = true;
                        currentLocation.isSelected = false;
                        int nextLoc = currentLocation.number + 1;
                        Location location = YandexGame.Instance.progressData.locations[currentLocation.number + 1];
                        location.isSelected = true;
                        location.isLock = true;
                        GlobalMediator.instance.SelectedLevel(nextLoc, nextLevel);
                        YandexGame.Instance.Save();
                    }
                    else
                    {
                        GlobalMediator.instance.SelectedLevel(GlobalMediator.instance.SelectLocation, nextLevel);
                    }
                   
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