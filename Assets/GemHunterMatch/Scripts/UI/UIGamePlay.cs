using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.UI;

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
        private GamePlay gamePlay;
        private Wallet wallet;
        private LevelConfig level;
        public RectTransform rootGoals;
        public RectTransform containerPopup;
        private Dictionary<int,UIGoalEntry> goals = new();
        public TextMeshProUGUI moveCounter;
        public TextMeshProUGUI cons;
        public UIBonusGroup bonusGroup;

        private void OnDisable()
        {
            wallet.OnValueChanged -= ValueChange;
            gamePlay.OnGoalChanged -= GoalChange;
            gamePlay.OnMoveHappened -= MoveHappen;
            gamePlay.OnMoveTriger -= GamePlay_OnMoveTriger;
            gamePlay.OnAllGoalFinished -= Finished;
            gamePlay.OnMachted -= MatchEffect;
        }

        public void Initialize(GamePlay gamePlay, Wallet wallet, LevelConfig level)
        {
            this.gamePlay = gamePlay;
            this.wallet = wallet;
            this.level = level;
            cons.text = wallet.Coins.ToString();
            wallet.OnValueChanged += ValueChange;
            
            gamePlay.OnGoalChanged += GoalChange;
            gamePlay.OnMoveHappened += MoveHappen;
            gamePlay.OnMoveTriger += GamePlay_OnMoveTriger;
            gamePlay.OnAllGoalFinished += Finished;
            gamePlay.OnMachted += MatchEffect;

            moveCounter.text = level.MaxMove.ToString();

            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIPopupLevelGoals>(popupLevelGoals, containerPopup, op => op.Init(level)));

            foreach (var goal in level.Goals)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", rootGoals, op =>
                {
                    op.Init(goal);
                    goals[op.GetTypeGoal()] = op;
                }));
            }
            bonusGroup.Init(gamePlay);
        }

        private void GamePlay_OnMoveTriger(int moves)
        {
            StartCoroutine(LoaderAsset.InstantiateAsset("PopupWarning",containerPopup));
        }

        private void ValueChange(int value)
        {
            cons.text = value.ToString();
        }
       

        private void Finished(bool isCondition)
        {
            if (isCondition)
                StartCoroutine(LoaderAsset.InstantiateAsset<UIPopupWin>(popupWin, containerPopup, op => op.Init(isCondition)));
            else
                StartCoroutine(LoaderAsset.InstantiateAsset<PopupDefeat>("PopupDefeat", containerPopup, op => { op.Init(gamePlay,wallet); op.Show(gamePlay.gemGoals); })); 
        }

        private void MoveHappen(int move)
        {
            moveCounter.text = move.ToString();
        }

        public void MatchEffect(Gem gem)
        {
            popupHandler.Show(gem.UISprite, gem.transform.position, goals[gem.GemType].transform.position);
        }

        private void GoalChange(int type, int count, bool isExecut)
        {
            if(goals.TryGetValue(type, out UIGoalEntry entry)) 
            {
                entry.Change(count,isExecut);
            }
            else
            {
                Debug.Log($"No type {type} UIGoalEntry");
            }
        }
    }
}