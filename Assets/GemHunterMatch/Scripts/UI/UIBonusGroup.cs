using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;
using Assets.YG.Scripts;

using Match3;

using System.Collections.Generic;

using UnityEngine;

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
            if (gamePlay != null) gamePlay.OnUsedBonusItem -= UsedBonusItem;
            foreach (var entry in bonusItems)
            {
                entry.Value.Button.onClick.RemoveAllListeners();
            }
        }

        public void Init(GamePlay gamePlay)
        {
            this.gamePlay = gamePlay;
           
            foreach (var bonus in gamePlay.bonusList)
            {
                if (bonus.UsedBonusGem.GemType != -2)
                {
                    CreateEntry(bonus);
                }
            }
            GlobalMediator.instance.OnAddBonus += UsedBonusItem;
            gamePlay.OnUsedBonusItem += UsedBonusItem;
        }

        private void CreateEntry(BonusGemBonusItem bonus)
        {
            int amountData = YandexGame.Instance.progressData.GetBonusGemAmount(bonus.UsedBonusGem.GemType);
            UIItemEntry entry = Instantiate(item, transform);
            entry.Init(bonus, amountData);
            bonusItems[bonus.UsedBonusGem.GemType] = entry;

            entry.Button.onClick.AddListener(() =>
            {
                int currentType = bonus.UsedBonusGem.GemType;

                if (selectedType != currentType)
                {
                   if(selectedType != 0) 
                      bonusItems[selectedType].SwitchView(false);

                    selectedType = bonus.UsedBonusGem.GemType;
                    entry.SwitchView(true);
                    gamePlay.ActivateBonusItem(bonus);
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