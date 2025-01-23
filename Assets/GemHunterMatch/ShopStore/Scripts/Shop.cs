using Assets.GameMains.Scripts.Bank;
using Assets.YG.Scripts;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;
namespace Assets.GemHunterMatch.ShopStore.Scripts
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
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

            walletAmount.text = wallet.Coins.ToString();
            wallet.OnValueChanged += Wallet_OnValueChanged;
            closeButton.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        private void Wallet_OnValueChanged(int amount)
        {
            walletAmount.text = amount.ToString();
        }
    }
}