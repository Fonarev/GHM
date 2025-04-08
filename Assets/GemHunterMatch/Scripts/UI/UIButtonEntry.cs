using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GameMains.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
using Assets.YG.Scripts;
using UnityEditor;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButtonEntry : MonoBehaviour
    {
        private GlobalMediator mediator;
        [SerializeField] private ButtonType type;
        private Button button => GetComponent<Button>();
        private TextMeshProUGUI nameT => GetComponentInChildren<TextMeshProUGUI>();
        private GameObject window;
        public void Init(GlobalMediator mediator, ButtonType type, GameObject window = null)
        {
            this.mediator = mediator;
            this.type = type;
            this.window = window;
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
           switch(type) 
           {
                case ButtonType.Next:
                    int nextLevel = GlobalMediator.SelectLevel + 1;
                    Location currentLocation = YandexGame.Instance.progressData.locations[mediator.SelectLocation];
                    
                    if (nextLevel > currentLocation.endNumberLevel)
                    {
                        currentLocation.completed = true;
                        currentLocation.isSelected = false;
                        int nextLoc = currentLocation.number + 1;

                        if (!YandexGame.Instance.progressData.locations.ContainsKey(nextLoc))
                        {
                            YandexGame.Instance.progressData.locations[nextLoc] = new Location(nextLoc, nextLevel, true, true);
                        }
                        else
                        {
                            Location location = YandexGame.Instance.progressData.locations[nextLoc];
                            location.isSelected = true;
                            location.isLock = true;
                        }


                        mediator.SelectedLevel(nextLoc, nextLevel);
                        YandexGame.Instance.Save();
                    }
                    else
                    {
                        mediator.SelectedLevel(mediator.SelectLocation, nextLevel);
                    }
                   
                    AudioManager.instance.PlayEffect(EffectClip.click);
                    break;

                case ButtonType.Menu:
                    mediator.ExitMenu();
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