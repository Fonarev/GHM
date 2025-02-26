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
        [SerializeField] private Image Mask;
        [SerializeField] private Image lockMask;
        [SerializeField] private RectTransform rootPrefabs;

        private SelectLevelsUI selectLevels;
        private Location location;

        public void Init(Location location)
        {
            this.location = location;
            openselectLevels.interactable = location.isLock;
            CloseMask(!location.isLock);
            numberLocation.text = Languages.GetContent("Location ") + location.number.ToString();
            barLevels.text = Languages.GetContent("Levels ") + location.startLevel + "/" + location.endNumberLevel.ToString();
            lockMask.gameObject.SetActive(!location.isLock);
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
        public void CloseMask(bool isValue)
        {
           Mask.gameObject.SetActive(isValue);
        }
    }
}