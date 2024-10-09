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
using UnityEngine.AddressableAssets;

namespace Assets.GemHunterMatch.Scripts
{
    public class GamePlay : MonoBehaviour
    {
        public event Action<int, int> OnGoalChanged;
        public event Action<bool> OnAllGoalFinished;
        public event Action<int> OnMoveHappened;
        public event Action<int,int> OnUsedBonusItem;

        public static GamePlay Instance => instance;

        public VisualSetting visualSettings;
        public Dictionary<int, BonusGemBonusItem> bonusItems = new();
        public bool IsPlaying { get; private set; }
        public int GoalLeft { get; private set; }
        public int RemainingMove { get; private set; }

        private List<Goals> gemGoals = new();
        private LevelConfig level;
        public GridBoard gridBoard;
        public UIGamePlay ui;
        private Wallet wallet;
        private bool isPlaying;
        private static GamePlay instance;
       
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

            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GridBoard>(level.gridBoardReference, null, op => 
            {
                gridBoard = op;
                CoroutineHandler.StartRoutine(gridBoard.Initialize(this, level)); 
            }));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.LoadList<BonusGemBonusItem>("bonusItem", op => { bonusItems[op.UsedBonusGem.GemType] = op; }));
          
            ui.Initialize(this, wallet, level);
            IsPlaying = true;
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
            Finish(false);
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
                            Finish(true);
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

        public void Finish(bool isConditions)
        {
            IsPlaying = false;
          
            StartCoroutine(ShowVisualFinish(isConditions));
        }

        private IEnumerator ShowVisualFinish(bool isConditions)
        {
            yield return new WaitForSeconds(1);

            if (isConditions)
            {
                yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset(visualSettings.LoseEffect, transform));
                SetCompletedLevel();
                AudioManager.instance.PlayEffect("chime");

                while (gridBoard.boardChanged)
                {
                    yield return new WaitForSeconds(2);
                    yield return gridBoard.boardChanged;
                }
            }
            else
            {
                AudioManager.instance.PlayEffect("jingle_chime");
            }

            OnAllGoalFinished.Invoke(isConditions);
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset(visualSettings.WinEffect, transform));
        }

        private void SetCompletedLevel()
        {
            YandexGame.Instance.progressData.levels[level.level].isCompleted = true;

            int nextLevel = level.level;
            nextLevel++;
            if (!YandexGame.Instance.progressData.levels.ContainsKey(nextLevel))
            {
                YandexGame.Instance.progressData.levels[nextLevel] = new LevelData(){ level = nextLevel, isOpened = true};
            }

            YandexGame.Instance.Save();
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