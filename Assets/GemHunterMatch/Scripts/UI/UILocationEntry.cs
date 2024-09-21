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
        [SerializeField] private int amountLevel;
        [SerializeField] private int startcountLevel;
        [SerializeField] private bool completed;
        [SerializeField] private TextMeshProUGUI textLevel;
        [SerializeField] private TextMeshProUGUI barLevels;
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform rootPrefabs;

        private LocalAssetLoader loader;
        private UISelectLevels selectLevels;

        private void OnDisable()
        {
            if (loader != null) 
            { 
                loader.UnLoadAll();
                loader = null;
            }
        }

        public void Init(int numberLoc)
        {
            number = numberLoc;
            int count = startcountLevel + amountLevel-1;
            textLevel.text = "Location " + number.ToString();
            barLevels.text = "Levels " + startcountLevel +"/" + count.ToString();
            if(TryLoc(number))
            {
                button.interactable = true;
                button.onClick.AddListener(OpenPanel);
                if (!completed)
                {
                   OpenPanel();
                }
            }
           
        }
        private bool TryLoc(int loc)
        {
            if (YandexGame.Instance.progressData.locations.ContainsKey(loc))
            {
                completed = YandexGame.Instance.progressData.locations[loc].completed;
                return true;
            }
            return false;
        }

        private void OpenPanel()
        {
            if (selectLevels == null)
            {
                loader = new(true);
                StartCoroutine(LoaderAsset.InstantiateAsset<UISelectLevels>("SelectLevels", rootPrefabs, op => 
                { 
                    selectLevels = op;
                    selectLevels.Init(startcountLevel, amountLevel);
                    rootPrefabs.gameObject.SetActive(true);
                }));
               
                
            }
            else
            {
                selectLevels.gameObject.SetActive(!selectLevels.gameObject.activeSelf);
                rootPrefabs.gameObject.SetActive(!rootPrefabs.gameObject.activeSelf);
            }

            AudioManager.instance.PlayEffect(EffectClip.click);
        }
    }
}