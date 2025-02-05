using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class LevelButtonSelectUI : MonoBehaviour
    {
        [SerializeField] private Image Lock;

        private Image Icon => GetComponent<Image>();
        private Button Button => GetComponent<Button>();
        private TextMeshProUGUI NumberLevel => GetComponentInChildren<TextMeshProUGUI>();

        public void Init(int location, int level, bool isLock = true, bool isComleted = false)
        {
            Lock.gameObject.SetActive(isLock);
            Button.interactable = !isLock;

            NumberLevel.text = level.ToString();

            if (isComleted)
                Icon.color = Color.green;

            Button.onClick.AddListener(()=>
            {
                GlobalMediator.instance.SelectedLevel(location, level);
                AudioManager.instance.PlayEffect(EffectClip.click);
            });
        }

        public void Updater(bool isLock = true, bool isComleted = false)
        {
            Lock.gameObject.SetActive(isLock);
            Button.interactable = !isLock;
            if (isComleted)
                Icon.color = Color.green;
        }
    }
}