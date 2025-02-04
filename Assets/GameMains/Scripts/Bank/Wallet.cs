using Assets.GameMains.Scripts.AudiosSources;
using Assets.YG.Scripts;

using System;

namespace Assets.GameMains.Scripts.Bank
{
    public class Wallet 
    {
        public event Action<int> OnValueChanged;

        public int Coins
        {
            get => YandexGame.Instance.progressData.coins;

            private set
            {
                int oldValue = coins;
                coins = value;
                YandexGame.Instance.progressData.coins = coins;
                if (oldValue != coins) OnValueChanged ?.Invoke(coins); 
            }
        }

        private int coins;

        public void Initialize()
        {
            //Coins = amount;
        }

        public void Add(int amount)
        {
            AudioManager.instance.PlayEffect("coin");
            Coins += amount;
        }

        public void Spend(int amount)
        {
            AudioManager.instance.PlayEffect("coin");
            Coins -= amount;
        }

        public bool Check(int amount)
        {
            return amount <= Coins ? true : false;
        }
    }
}