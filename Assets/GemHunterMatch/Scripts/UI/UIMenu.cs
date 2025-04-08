using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.ShopStore.Scripts;
using Assets.WheelOfLuck.Scripts;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private UISelectLocation locationLevels;
        [SerializeField] private Image logo;
        //[SerializeField] private TextMeshProUGUI score;
        [SerializeField] private Transform popupContainer;
        //[SerializeField] private ButtonOpenWheel openWheel;
        [SerializeField] private OpenWindowButtonGroup openWindowButtonGroup;
        [SerializeField] private LBUI lb;
       
        public void Initialize(GlobalMediator mediator, DailyRewards.Scripts.DailyRewardsService dailyRewards,WheelOfLuckService serviceWheel)
        {
            //score.text = Languages.GetContent("Score: ")+ YandexGame.Instance.progressData.Score.ToString();
            locationLevels.Init(mediator);
            CoroutineHandler.StartRoutine(LoaderAsset.Load<Sprite>("Logo", op => { logo.sprite = op; Addressables.Release(op); }));
            openWindowButtonGroup.Init(mediator, dailyRewards, popupContainer);
            
           if(dailyRewards.TryState())
           {
                openWindowButtonGroup.Open(OpenButtonType.Reward);
           }

            lb.Init();
            //openWheel.Init(serviceWheel, popupContainer);

        }

    }
}