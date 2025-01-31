using Assets.AssetLoaders;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.UI;

using Match3;

using System.Collections;
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
        [SerializeField] private TextMeshProUGUI levelNumber;
        private GamePlay gamePlay;
        private Wallet wallet;
        private LevelConfig level;
        public RectTransform rootGoals;
        [SerializeField] private RectTransform containerPopups;
        private Dictionary<int,UIGoalEntry> goals = new();
        public TextMeshProUGUI moveCounter;
        public TextMeshProUGUI coins;
        public TextMeshProUGUI score;
        public UIBonusGroup bonusGroup;

        private void OnDisable()
        {
            wallet.OnValueChanged -= ValueChange;
            gamePlay.OnGoalChanged -= GoalChange;
            gamePlay.OnMoveHappened -= MoveHappen;
            gamePlay.OnMoveTriger -= GamePlay_OnMoveTriger;
            gamePlay.OnAllGoalFinished -= Finished;
            gamePlay.OnShowMessages -= ShowMessages;
            gamePlay.OnMachtedGem -= MatchEffect;
            gamePlay.OnMachted -= OnMachted;
            gamePlay.OnAddScore -= OnAddScore;
        }
        public void Update()
        {
            
        }
        private void OnAddScore(int oldScore, int newScore)
        {
            //score.text = "Score: " + newScore.ToString();
            StartCoroutine(ScrollScore(oldScore, newScore));
        }
       private IEnumerator ScrollScore(int oldScore,int newScore)
       {
            var scroll = newScore - oldScore;

            while (scroll > 0)
            {
                var amount = oldScore += 1;
                score.text = "Score: " + amount.ToString();

                yield return scroll--;
            }
       }
        public void Initialize(GamePlay gamePlay, Wallet wallet, LevelConfig level)
        {
            this.gamePlay = gamePlay;
            this.wallet = wallet;
            this.level = level;

            levelNumber.text = "level " + level.level.ToString();
            moveCounter.text = level.MaxMove.ToString();
            coins.text = wallet.Coins.ToString();

            wallet.OnValueChanged += ValueChange;
            
            gamePlay.OnGoalChanged += GoalChange;
            gamePlay.OnMoveHappened += MoveHappen;
            gamePlay.OnMoveTriger += GamePlay_OnMoveTriger;
            gamePlay.OnAllGoalFinished += Finished;
            gamePlay.OnShowMessages += ShowMessages;
            gamePlay.OnMachtedGem += MatchEffect;
            gamePlay.OnMachted += OnMachted;
            gamePlay.OnAddScore += OnAddScore;
            score.text ="Score: "+ 0.ToString();
            bonusGroup.Init(gamePlay);

            foreach (var goal in gamePlay.Goals)
            {
                CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", rootGoals, op =>
                {
                    op.Init(goal);
                    goals[op.GetTypeGoal()] = op;
                }));
            }

            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<UIPopupLevelGoals>(popupLevelGoals, containerPopups, op => op.Init(gamePlay)));
        }

        private void ShowMessages(string message)
        {
            CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<MessagesPopup>("MessagesPopup", containerPopups, op =>
            {
                op.Init(message);
            }));
        }

        private void OnMachted(UnderGem underGem)
        {
            popupHandler.Show(underGem.UISprite, underGem.transform.position, goals[underGem.GemType].transform.position,false);
        }

        private void GamePlay_OnMoveTriger(int moves)
        {
            StartCoroutine(LoaderAsset.InstantiateAsset("PopupWarning",containerPopups));
        }

        private void ValueChange(int value)
        {
            coins.text = value.ToString();
        }
       

        private void Finished(bool isCondition)
        {
            if (isCondition)
                StartCoroutine(LoaderAsset.InstantiateAsset<UIPopupWin>(popupWin, containerPopups, op => op.Init(isCondition)));
            else
                StartCoroutine(LoaderAsset.InstantiateAsset<PopupDefeat>("PopupDefeat", containerPopups, op => { op.Init(gamePlay,wallet); op.Show(gamePlay.Goals); })); 
        }

        private void MoveHappen(int move)
        {
            moveCounter.text = move.ToString();
        }

        public void MatchEffect(Gem gem)
        {
            if (gem != null)
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