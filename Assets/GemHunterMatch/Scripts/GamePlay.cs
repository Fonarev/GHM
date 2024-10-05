using Assets.AssetLoaders;
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;
using Assets.YG.Scripts;

using Match3;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    public class GamePlay : MonoBehaviour
    {
        public class Goals
        {
            public Gem gem;
            public int count;
        }

        public event Action<int, int> OnGoalChanged;
        public event Action OnAllGoalFinished;
        public event Action<int> OnMoveHappened;
        public event Action<int,int> OnUsedBonusItem;

        public static GamePlay instance;
        public Dictionary<int, BonusGemBonusItem> bonusItems = new();
        public bool IsPlaying { get; private set; }
        public int GoalLeft { get; private set; }
        public int RemainingMove { get; private set; }

        private List<Goals> gemGoals = new();
        private LevelConfig level;
        public GridBoard gridBoard;
        public UIGamePlay ui;
        private Wallet wallet;

        private void Awake()
        {
            instance = this;
        }

        public void Initialize(Wallet wallet)
        {
            this.wallet = wallet;

            CoroutineHandler.StartRoutine(Load());

            foreach (var item in level.Goals)
            {
                var goal = new Goals();
                goal.gem = item.Gem;
                goal.count = item.Count;
                gemGoals.Add(goal);
            }

            RemainingMove = level.MaxMove;
            GoalLeft = gemGoals.Count;
           
        }

        private IEnumerator Load()
        {
            level = LevelDatabase.GetLevel(GlobalMediator.instance.SelectLevel);

            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GridBoard>(level.gridBoardReference, null, op => { gridBoard = op; }));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.LoadList<BonusGemBonusItem>("bonusItem", op => { bonusItems[op.UsedBonusGem.GemType] = op; }));

            gridBoard.Initialize(this, level);
            ui.Initialize(this, wallet, level);
        }

        public void Moved()
        {
            var prev = RemainingMove;

            RemainingMove = Mathf.Max(0, RemainingMove - 1);
            OnMoveHappened?.Invoke(RemainingMove);

            if (prev > level.LowMoveTrigger && RemainingMove <= level.LowMoveTrigger)
            {
                //UIHandler.Instance.TriggerCharacterAnimation(UIHandler.CharacterAnimation.LowMove);
            }

            if (RemainingMove <= 0)
            {
                OnNoMoveLeft();
            }
        }

        private void OnNoMoveLeft()
        {
            Finish();
        }

        public bool Matched(Gem gem)
        {
            foreach (var goal in gemGoals)
            {
                if (goal.gem.GemType == gem.GemType)
                {
                    if (goal.count == 0)
                        return false;

                    ui.AddMatchEffect(gem);

                    goal.count -= 1;
                    OnGoalChanged?.Invoke(gem.GemType, goal.count);
                    Debug.Log($"{gem.GemType}, {goal.count}");

                    if (goal.count == 0)
                    {
                        GoalLeft -= 1;
                        if (GoalLeft == 0)
                        {
                            OnAllGoalFinished?.Invoke();
                            Finish();
                            Debug.Log($"Finished");
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public void Finish()
        {
           IsPlaying = false;
        }

        public void ChangeCoins(int amount)
        {
            wallet.Add(amount);
            AudioManager.instance.PlayEffect("coin");
        }

        public void ActivateBonusItem(BonusGemBonusItem item)
        {
            if (item != null)
            {
                if (YandexGame.Instance.progressData.bonusGemItem[item.UsedBonusGem.GemType] > 0)
                    gridBoard.activatedBonus = item;
                else
                    Debug.Log("Create Handle!!!!   Item < 0");
            }
            else
            {
                gridBoard.activatedBonus = item;
            }
            
        }

        public void UseBonusItem(BonusGemBonusItem activatedBonus, Vector3Int clickedCell)
        {
            activatedBonus.Use(clickedCell);
            if (YandexGame.Instance.progressData.bonusGemItem.TryGetValue(activatedBonus.UsedBonusGem.GemType, out var amount))
            {
                amount -= 1;
                YandexGame.Instance.progressData.bonusGemItem[activatedBonus.UsedBonusGem.GemType] -= 1;
                OnUsedBonusItem.Invoke(activatedBonus.UsedBonusGem.GemType,amount);
            }
        }

    }
}