using Assets.GameMains.Scripts.Bank;

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

            priceT.text ="x " + price.ToString();
            button.onClick.AddListener(OnClick);
            button.interactable = wallet.Check(price);
            this.window = window;
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
            
            }
        }
    }
}