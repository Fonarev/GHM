using Assets.GameMains.Scripts.Bank;

using TMPro;

using UnityEngine;
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
      
        private Button button => GetComponent<Button>();
        private TextMeshProUGUI priceT => GetComponentInChildren<TextMeshProUGUI>();
        internal void Init(BayType type, GamePlay gamePlay, Wallet wallet)
        {
            this.type = type;
            this.gamePlay = gamePlay;
            this.wallet = wallet;

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