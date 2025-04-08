using Assets.GameMains.Scripts;
using Assets.YG.Scripts;

using TMPro;

using UnityEditor;

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
        private GlobalMediator mediator;
        private ShopItem shopItem;

        private void OnDisable()
        {
            if (mediator != null)
                mediator.OnCoinsChanged -= UpdateView;
        }

        public void Init(GlobalMediator mediator, ShopItem shopItem)
        {
            this.mediator = mediator;
            this.shopItem = shopItem;

            View();

            mediator.OnCoinsChanged += UpdateView;

            buy?.onClick.AddListener(() =>
                {
                    mediator.Spend(shopItem.price);
                    YandexGame.Instance.progressData.AddBonusGem(shopItem.bonusGem.GemType,5);
                    mediator.AddBonus(shopItem.bonusGem.GemType, 5);
                    UpdateView(shopItem.price);
                });
               
        }

        private void UpdateView(int value)
        {
            if (buy != null)
                buy.interactable = mediator.Check(shopItem.price);
        }

        private void View()
        {
            icon.sprite = shopItem.itemIcon;

            price.text = shopItem.price.ToString();
            itemName.text = Languages.GetContent(shopItem.itemName);

            buy.interactable = mediator.Check(shopItem.price);
        }
    }
}