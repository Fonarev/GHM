using Assets.GameMains.Scripts.Bank;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.ShopStore.Scripts
{
    public class ShopItemEntry : MonoBehaviour
    {
        public Button buy;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private TextMeshProUGUI itemName;

        private ShopItem shopItem;
        private Wallet wallet;

        private void OnDisable()
        {
            wallet.OnValueChanged -= UpdateView;
        }

        public void Init(ShopItem shopItem, Wallet wallet)
        {
            this.wallet = wallet;
            this.shopItem = shopItem;

            View();

            wallet.OnValueChanged += UpdateView;

            buy.onClick.AddListener(() =>
            {
                wallet.Spend(shopItem.price);
                YandexGame.Instance.progressData.AddBonusGem(shopItem.bonusGem.GemType);
            });
        }

        private void UpdateView(int value)
        {
            buy.interactable = wallet.Check(shopItem.price);
        }

        private void View()
        {
            icon.sprite = shopItem.itemIcon;

            price.text = shopItem.price.ToString();
            itemName.text = shopItem.itemName;

            buy.interactable = wallet.Check(shopItem.price);
        }
    }
}