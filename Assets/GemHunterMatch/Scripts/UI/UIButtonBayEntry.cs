using Assets.GameMains.Scripts.Bank;
using Assets.YG.Scripts;

using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIButtonBayEntry : MonoBehaviour
    {
        [SerializeField] private BayType type;
        [SerializeField] private int price;
        [SerializeField] private int moves;
        private GamePlay gamePlay;
        private Wallet wallet;
        private GameObject window;

        private Button button => GetComponent<Button>();
        private TextMeshProUGUI priceT => GetComponentInChildren<TextMeshProUGUI>();
        public void Init(BayType type, GamePlay gamePlay, Wallet wallet,GameObject window)
        {
            this.type = type;
            this.gamePlay = gamePlay;
            this.wallet = wallet;
            this.window = window;

            switch (type)
            {
                case BayType.Moves:
                    priceT.text = "x " + price.ToString();
                    button.interactable = wallet.Check(price);
                    break;

                case BayType.RewardAds:
                   
                    break;
            }
         
            button.onClick.AddListener(OnClick);
            
        }

        private void OnClick()
        {
            switch(type)
            {
                case BayType.Moves:
                    if (wallet.Check(price))
                    {
                        wallet.Spend(price);
                        gamePlay.AddMoves(moves);
                        
                        Addressables.ReleaseInstance(window);
                    }
                    else
                    {
                        Debug.Log("No coins and plise add coins prise");
                    }
                    break;

                case BayType.RewardAds:
                    YandexGame.Instance.RewardShow(0);
                    Addressables.ReleaseInstance(window);
                    break;
            }
        }
    }
}