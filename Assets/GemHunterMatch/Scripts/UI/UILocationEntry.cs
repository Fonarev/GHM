using Assets.AssetLoaders;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UILocationEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI numberLocation;
        [SerializeField] private TextMeshProUGUI barLevels;
        [SerializeField] private Button openselectLevels;
        [SerializeField] private RectTransform rootPrefabs;

        private SelectLevelsUI selectLevels;
        private Location location;

        public void Init(Location location)
        {
            this.location = location;
            //openselectLevels.interactable = location.isLock;
           
            numberLocation.text = "Location " + location.number.ToString();
            barLevels.text = "Levels " + location.startLevel + "/" + location.endNumberLevel.ToString();

            if (location.isSelected)
                OpenPanel();

            openselectLevels.onClick.AddListener(OpenPanel);
        }

        private void OpenPanel()
        {
            AudioManager.instance.PlayEffect(EffectClip.click);

            if (selectLevels != null)
            {
                selectLevels.gameObject.SetActive(!selectLevels.gameObject.activeSelf);
                rootPrefabs.gameObject.SetActive(!rootPrefabs.gameObject.activeSelf);

                YandexGame.Instance.progressData.locations[location.number].isSelected = selectLevels.gameObject.activeSelf;
            }
            else
            {
                StartCoroutine(LoaderAsset.InstantiateAsset<SelectLevelsUI>("SelectLevels", rootPrefabs, op =>
                {
                    selectLevels = op;
                    selectLevels.Init(location);
                }));

                rootPrefabs.gameObject.SetActive(true);
            }
        }
    }
}