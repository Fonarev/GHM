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
        private Button button => GetComponent<Button>();
        private TextMeshProUGUI numberLevel => GetComponentInChildren<TextMeshProUGUI>();
       
        public void Init(int number)
        {
            level = number;
            numberLevel.text = number.ToString();

            if (YandexGame.Instance.progressData.levels.ContainsKey(number))
            {
                button.interactable = true;
                Lock.gameObject.SetActive(false);
                button.onClick.AddListener(OnClick);
            }
            else
            {
                button.interactable = false;
                Lock.gameObject.SetActive(true);
            }

        }
       
        private void OnClick()
        {
            GlobalMediator.instance.SelectedLevel(level);
            AudioManager.instance.PlayEffect(EffectClip.click);
        }

        public void UpDateView()
        {
            //bool isLock = TryLevel(level);
            //button.interactable = isLock;
            //Lock.gameObject.SetActive(isLock);
        }
    }
}