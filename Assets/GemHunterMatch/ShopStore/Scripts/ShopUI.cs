using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;
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

        private Wallet wallet;

        private void OnDisable()
        {
            wallet.OnValueChanged -= Wallet_OnValueChanged;
        }

        public void Init(Wallet wallet)
        {
            this.wallet = wallet;

            foreach (var item in ShopItemData.items)
            {
                var pref = Instantiate(previwItemPrefab,GridRootItem);
                pref.Init(item,wallet);
            }
            rewardButton.onClick.AddListener(() => { YandexGame.Instance.RewardShow(1); });
            walletAmount.text = wallet.Coins.ToString();
            wallet.OnValueChanged += Wallet_OnValueChanged;
            closeButton.onClick.AddListener(() => { AudioManager.instance.PlayEffect(EffectClip.click); gameObject.SetActive(false); });
        }

        private void Wallet_OnValueChanged(int amount)
        {
            walletAmount.text = amount.ToString();
        }
    }
}