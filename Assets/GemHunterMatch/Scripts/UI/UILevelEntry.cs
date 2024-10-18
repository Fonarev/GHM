using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class UILevelEntry : MonoBehaviour
    {
        [SerializeField] private Image Lock;

        private int level;

        private Image view => GetComponent<Image>();
        private Button button => GetComponent<Button>();
        private TextMeshProUGUI numberLevel => GetComponentInChildren<TextMeshProUGUI>();
       
        public void Init(int number)
        {
            level = number;
            button.interactable = false;
            Lock.gameObject.SetActive(true);
            numberLevel.text = number.ToString();

            if (YandexGame.Instance.progressData.levels.TryGetValue(number,out var levelData))
            {
                button.interactable = true;
                Lock.gameObject.SetActive(false);
               if(levelData.isCompleted)
               {
                    view.color = Color.green;
               }
                button.onClick.AddListener(OnClick);
            }

        }
       
        private void OnClick()
        {
            GlobalMediator.instance.SelectedLevel(level);
            AudioManager.instance.PlayEffect(EffectClip.click);
        }

    }
}