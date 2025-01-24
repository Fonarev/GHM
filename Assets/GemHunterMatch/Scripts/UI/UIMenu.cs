using Assets.AssetLoaders;
using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts.Bank;
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
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private Transform popupContainer;
        [SerializeField] private ButtonEntry openButtonDaily;
        [SerializeField] private ButtonOpenWheel openWheel;
        [SerializeField] private Shop shop;
        [SerializeField] private OpenButtonShop openButtonShop;
        public void Initialize(Wallet wallet, DailyRewards.Scripts.DailyRewardsService _dailyRewards,WheelOfLuckService serviceWheel)
        {
            score.text = "Score: " + YandexGame.Instance.progressData.Score.ToString();
            locationLevels.Init();
            CoroutineHandler.StartRoutine(LoaderAsset.Load<Sprite>("Logo", op => { logo.sprite = op; Addressables.Release(op); }));
            if (_dailyRewards.TryState())
            {
                _dailyRewards.OpenPopupWin(popupContainer);
            }
            openButtonDaily.Init(_dailyRewards, popupContainer);
            shop.Init(wallet);
            openButtonShop.Init(shop);
            openWheel.Init(serviceWheel, popupContainer);
        }

    }
}