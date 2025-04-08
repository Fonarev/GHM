using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
namespace Assets.GemHunterMatch.ShopStore.Scripts
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button rewardButton;
        [SerializeField] private Transform GridRootItem;
        [SerializeField] private ShopItemData ShopItemData;
        [SerializeField] private ShopItemEntry previwItemPrefab;
        [SerializeField] private TextMeshProUGUI walletAmount;
        private GlobalMediator mediator;
        private void OnEnable()
        {
            if(mediator!= null)
            {
                walletAmount.text = mediator.Coins.ToString();
                mediator.OnCoinsChanged += Wallet_OnValueChanged;
                walletAmount.text = mediator.Coins.ToString();
            }
        }
        private void OnDisable()
        {
            mediator.OnCoinsChanged -= Wallet_OnValueChanged;
        }

        public void Init(GlobalMediator mediator)
        {
            this.mediator = mediator;

            foreach (var item in ShopItemData.items)
            {
                var pref = Instantiate(previwItemPrefab,GridRootItem);
                pref.Init(mediator,item);
            }
            rewardButton.onClick.AddListener(() => { AudioManager.instance.PlayEffect(EffectClip.click); YandexGame.Instance.RewardShow(1); });
            walletAmount.text = mediator.Coins.ToString();
            mediator.OnCoinsChanged += Wallet_OnValueChanged;
            closeButton.onClick.AddListener(() => { AudioManager.instance.PlayEffect(EffectClip.click); gameObject.SetActive(false); });
        }

        private void Wallet_OnValueChanged(int amount)
        {
            walletAmount.text = amount.ToString();
        }
    }
}