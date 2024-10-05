using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using Match3;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIBonusGroup : MonoBehaviour
    {
        public UIItemEntry item;
       
        private int selectedType;
        private Dictionary<int, UIItemEntry> bonusItems = new();
        private GamePlay gamePlay;

        private void OnDisable()
        {
            gamePlay.OnUsedBonusItem -= UsedBonusItem;
        }

        public void Init(GamePlay gamePlay)
        {
            this.gamePlay = gamePlay;

            foreach (var bonus in gamePlay.bonusItems)
            {
                if (bonus.Value.UsedBonusGem.GemType != -2)
                {
                    CreateEntry(bonus);
                }
            }

            gamePlay.OnUsedBonusItem += UsedBonusItem;
        }

        private void CreateEntry(KeyValuePair<int, BonusGemBonusItem> bonus)
        {
            int amountData = YandexGame.Instance.progressData.bonusGemItem[bonus.Value.UsedBonusGem.GemType];
            UIItemEntry entry = Instantiate(item, transform);
            entry.Init(bonus.Value.DisplaySprite, amountData);
            bonusItems[bonus.Key] = entry;

            entry.Button.onClick.AddListener(() =>
            {
                int currentType = bonus.Value.UsedBonusGem.GemType;

                if (selectedType != currentType)
                {
                   if(selectedType != 0) 
                      bonusItems[selectedType].SwitchView(false);

                    selectedType = bonus.Value.UsedBonusGem.GemType;
                    entry.SwitchView(true);
                    gamePlay.ActivateBonusItem(bonus.Value);
                }
                else
                {
                    gamePlay.ActivateBonusItem(null);
                    entry.SwitchView(false);
                    selectedType = 0;
                }

                AudioManager.instance.PlayEffect(EffectClip.click);

            });
        }

        private void UsedBonusItem(int type, int amount)
        {
           if(bonusItems.TryGetValue(type, out var bonus)) 
           {
                bonus.ChangedAmount(amount);
           }
        }
    }
}