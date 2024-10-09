using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;

using Match3;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIGamePlay : MonoBehaviour
    {
        [SerializeField] private UIPopupEntryHandler popupHandler;
        [SerializeField] private AssetReference popupLevelGoals;
        [SerializeField] private AssetReference popupWin;
        private LevelConfig level;
        public RectTransform rootGoals;
        public RectTransform containerPopup;
        private Dictionary<int,UIGoalEntry> goals = new();
        public TextMeshProUGUI moveCounter;
        public TextMeshProUGUI cons;
        public UIBonusGroup bonusGroup;
        
        public void Initialize(GamePlay gamePlay, Wallet wallet, LevelConfig level)
        {
            cons.text = wallet.Coins.ToString();
            wallet.OnValueChanged += value => cons.text = value.ToString();
            this.level = level;
            gamePlay.OnGoalChanged += GoalChange;
            gamePlay.OnMoveHappened += MoveHappen;
            gamePlay.OnAllGoalFinished += Finished;
            moveCounter.text = level.MaxMove.ToString();
            StartCoroutine(LoaderAsset.InstantiateAsset<UIPopupLevelGoals>(popupLevelGoals, containerPopup, op => op.Init(level)));
            foreach (var goal in level.Goals)
            {
                StartCoroutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", rootGoals, op =>
                {
                    op.Init(goal);
                    goals[op.GetTypeGoal()] = op;
                }));
               
            }
            bonusGroup.Init(gamePlay);
        }

        private void Finished(bool isCondition)
        {
            StartCoroutine(LoaderAsset.InstantiateAsset<UIPopupWin>(popupWin, containerPopup, op => op.Init(isCondition)));
        }

        private void MoveHappen(int move)
        {
            moveCounter.text = move.ToString();
        }

        public void AddMatchEffect(Gem gem)
        {
            popupHandler.Show(gem.UISprite, gem.transform.position, goals[gem.GemType].transform.position);
        }

        private void GoalChange(int type, int count)
        {
            if(goals.TryGetValue(type, out UIGoalEntry entry)) 
            {
                entry.Change(count);
            }
            else
            {
                Debug.Log($"No type {type} UIGoalEntry");
            }
        }
    }
}