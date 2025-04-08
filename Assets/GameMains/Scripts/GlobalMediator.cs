using Assets.GameMains.Scripts.AudiosSources;
using Assets.YG.Scripts;

using Match3;

using System;

using UnityEngine;

namespace Assets.GameMains.Scripts
{
    public class GlobalMediator 
    {
        public event Action<bool> OnApplicationFocused;
        public event Action<int> OnCoinsChanged;
        public Action<bool> OnNowAdsShow;
        public static event Action<int> OnSelectedLevel;
        public event Action OnExitMenu;
        public event Action<int, int> OnAddBonus;
        public event Action<BonusGemBonusItem> OnOpenedShop;
        public bool IsEnableBackgroundMusic { get => YandexGame.Instance.progressData.music; set { YandexGame.Instance.progressData.music = value; } }
        public bool IsEnableAudioEffect { get => YandexGame.Instance.progressData.effectAudio; set { YandexGame.Instance.progressData.effectAudio = value; } }
        public bool StateNowAdsShow { get => stateNowAdsShow; set { stateNowAdsShow = value; OnNowAdsShow?.Invoke(value); } }
        public static int SelectLevel
        {
            get => selectLevel;
            private set
            {
                selectLevel = value;
                OnSelectedLevel.Invoke(selectLevel);
                Debug.Log($"Level {selectLevel}");
            }
        }
        public int SelectLocation
        {
            get => selectLocation;
            private set
            {
                selectLocation = value;
                Debug.Log($"Location {selectLocation}");
            }
        }
        public int Coins { get => YandexGame.Instance.progressData.coins; set { YandexGame.Instance.progressData.coins = value; OnCoinsChanged?.Invoke(value); YandexGame.Instance.Save(); } }

        private static int selectLevel;
        private int selectLocation;
        private bool stateNowAdsShow;
        public void ApplicationFocus(bool isPause) => OnApplicationFocused?.Invoke(isPause);
        public void SelectedLevel(int location, int level)
        {
            SelectLevel = level;
            SelectLocation = location;
        }

        public void ExitMenu() => OnExitMenu?.Invoke();

        public void AddBonus(int gemType, int amount)=> OnAddBonus?.Invoke(gemType,amount);
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

        internal void OpenShop(BonusGemBonusItem bonus)
        {
            OnOpenedShop?.Invoke(bonus);
        }
    }
}