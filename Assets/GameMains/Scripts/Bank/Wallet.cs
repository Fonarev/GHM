using System;

namespace Assets.GameMains.Scripts.Bank
{
    public class Wallet 
    {
        public event Action<int> OnValueChanged;

        public int Coins
        {
            get => coins;

            private set
            {
                int oldValue = Coins; 
                coins = value;

                if(oldValue != Coins) OnValueChanged.Invoke(coins); 
            }
        }

        private int coins;

        public void Initialize(int amount)
        {
            Coins = amount;
        }

        public void Add(int amount)
        {
            Coins += amount;
        }

        public void Spend(int amount)
        {
            Coins -= amount;
        }

        public bool Check(int amount)
        {
            return amount <= Coins ? true : false;
        }
    }
}