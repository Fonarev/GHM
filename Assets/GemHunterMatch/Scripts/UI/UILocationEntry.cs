using Assets.AssetLoaders;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.Loaders;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UILocationEntry : MonoBehaviour
    {
        private int number;
        [SerializeField] private int maxLevel=20;
        [SerializeField] private int startcountLevel;
        [SerializeField] private bool completed;
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private TextMeshProUGUI barLevels;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform rootPrefabs;
        public int endLevel { get => startcountLevel + maxLevel - 1; }
        private UISelectLevels selectLevels;
        private Location location;

        public void Init(int numberLoc)
        {
            number = numberLoc;
            button.interactable = false;
            int count = startcountLevel + maxLevel - 1;
            textLevel.text = "Location " + number.ToString();
            barLevels.text = "Levels " + startcountLevel + "/" + count.ToString();

            if (YandexGame.Instance.progressData.locations.TryGetValue(number, out var location))
            {
                this.location = location;
                button.interactable = true;
                button.onClick.AddListener(OpenPanel);
               
                if (location.isSelected)
                {
                    OpenPanel();
                }
            }
 
        }

        private void OpenPanel()
        {
            if (selectLevels == null)
            {
                StartCoroutine(LoaderAsset.InstantiateAsset<UISelectLevels>("SelectLevels", rootPrefabs, op =>
                {
                    selectLevels = op;
                    selectLevels.Init(startcountLevel, maxLevel);
                }));

                rootPrefabs.gameObject.SetActive(true);
            }
            else
            {
                selectLevels.gameObject.SetActive(!selectLevels.gameObject.activeSelf);
                rootPrefabs.gameObject.SetActive(!rootPrefabs.gameObject.activeSelf);
                YandexGame.Instance.progressData.locations[number].isSelected = selectLevels.gameObject.activeSelf;

                AudioManager.instance.PlayEffect(EffectClip.click);
            }
        }
    }
}