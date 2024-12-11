using Assets.AssetLoaders;
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Bank;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.GenerateGridBoard;
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
        public event Action<int, int,bool> OnGoalChanged;
        public event Action<bool> OnAllGoalFinished;
        public event Action<int,int> OnAddScore;
        public event Action<int> OnMoveHappened;
        public event Action<int> OnMoveTriger;
        public event Action<int,int> OnUsedBonusItem;
        public event Action<Gem> OnMachtedGem;
        public event Action<UnderGem> OnMachted;

        public VisualSetting visualSettings;
        public BonusGemBonusItem[] bonusList;
        public Dictionary<int, BonusGemBonusItem> bonusItems = new();
        public bool IsPlaying { get; private set; }
        public int GoalLeft { get; private set; }
        public int RemainingMove { get; private set; }

        public List<Goals> Goals = new();
        private int Score;
        private LevelConfig level;
        private GridBoard gridBoard;
      
        private Wallet wallet;
        private bool isPlaying;
        bool isConditions;
        private GameObject objectSVX;
        public void Initialize(Wallet wallet)
        {
            this.wallet = wallet;
            CoroutineHandler.StartRoutine(Load());
            foreach (var item in level.GemGoals)
            {
                var goal = new Goals();
                goal.gem = item.Gem;
                goal.count = item.Count;
                Goals.Add(goal);
            }
            foreach (var item in level.ObstaclesGoals)
            {
                var goal = new Goals();
                goal.obstacle = item.Obstacle;
                goal.count = item.Count;
                Goals.Add(goal);
            }
            foreach (var item in level.UnderGemGoals)
            {
                var goal = new Goals();
                goal.underGem = item.UnderGem;
                goal.count = item.Count;
                Goals.Add(goal);
            }
            RemainingMove = level.MaxMove;
            GoalLeft = Goals.Count;
           
        }

        private IEnumerator Load()
        {
            level = LevelDatabase.GetLevel(GlobalMediator.instance.SelectLevel);
           
            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GridBoard>(level.gridBoardReference, null, op =>
            {
                gridBoard = op;
                gridBoard.Initialize(this); 
            }));
            yield return CoroutineHandler.StartRoutine(LoaderAsset.LoadList<BonusGemBonusItem>("bonusItem", op => { bonusItems[op.UsedBonusGem.GemType] = op; }));
          
           
            IsPlaying = true;
        }

        public void AddScore(int score)
        {
            var oldScore = Score;
            Score += score;
            OnAddScore.Invoke(oldScore, Score);
        }

        public void AddMoves(int moves)
        {
            RemainingMove = moves;
            Play();
        }

        public void Moved()
        {
            var prev = RemainingMove;

            RemainingMove = Mathf.Max(0, RemainingMove - 1);
            OnMoveHappened?.Invoke(RemainingMove);

            if (prev > level.LowMoveTrigger && RemainingMove <= level.LowMoveTrigger)
            {
                OnMoveTriger.Invoke(RemainingMove);
            }

            if (RemainingMove <= 0)
            {
                OnNoMoveLeft();
            }
        }

        private void OnNoMoveLeft()
        {
             
            Finish(isConditions);
        }

        public bool Matched(Gem gem)
        {
            foreach (var goal in Goals)
            {
                if (goal.GetCurrentType() == gem.GemType && gem != null)
                {
                    if (goal.count == 0)
                        return false;

                    OnMachtedGem.Invoke(gem);

                    goal.count -= 1;
                    OnGoalChanged?.Invoke(gem.GemType, goal.count, goal.isExecut);
                    //Debug.Log($"{gem.GemType}, {goal.count}");

                    if (goal.count == 0)
                    {
                        goal.isExecut = true;
                        OnGoalChanged?.Invoke(gem.GemType, goal.count, goal.isExecut);
                        GoalLeft -= 1;
                        if (GoalLeft == 0)
                        {
                            isConditions = true;
                            Finish(isConditions);
                            Debug.Log($"Finished");
                        }
                    }

                    return true;
                }
                
            }

            return false;
        }
        public bool Matched(UnderGem underGem)
        {
            foreach (var goal in Goals)
            {
                
                if (goal.GetCurrentType() == underGem.GemType && underGem != null)
                {
                    if (goal.count == 0)
                        return false;

                    OnMachted.Invoke( underGem);

                    goal.count -= 1;
                    OnGoalChanged?.Invoke(underGem.GemType, goal.count, goal.isExecut);
                    //Debug.Log($"{gem.GemType}, {goal.count}");

                    if (goal.count == 0)
                    {
                        goal.isExecut = true;
                        OnGoalChanged?.Invoke(underGem.GemType, goal.count, goal.isExecut);
                        GoalLeft -= 1;

                    }

                    if (GoalLeft == 0)
                    {
                        isConditions = true;
                        Finish(isConditions);
                        Debug.Log($"Finished");
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
                //yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset(visualSettings.LoseEffect, transform,op=>
                //{
                //    AudioManager.instance.PlayEffect("chime");
                //    objectSVX = op;
                //}));

                AudioManager.instance.PlayEffect("chime");

                while (gridBoard.BoardChanged)
                {
                    yield return new WaitForSeconds(2);
                    yield return gridBoard.BoardChanged;
                }

                OnAllGoalFinished.Invoke(isConditions);
                //objectSVX.SetActive(false);
                //Addressables.ReleaseInstance(objectSVX);
                SetCompletedLevel();
               
            }
            else
            {
                while (gridBoard.BoardChanged)
                {
                    yield return new WaitForSeconds(1);
                    yield return gridBoard.BoardChanged;
                }

                if (!this.isConditions)
                {
                    AudioManager.instance.PlayEffect("jingle_chime");
                    OnAllGoalFinished.Invoke(isConditions);
                }
                
            }
            //yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset(visualSettings.WinEffect, transform));
        }

        private void SetCompletedLevel()
        {
            YandexGame.Instance.progressData.levels[level.level].isCompleted = true;
            int oldScore = YandexGame.Instance.progressData.Score;
            YandexGame.Instance.progressData.Score += Score;
            if (oldScore< YandexGame.Instance.progressData.Score)
            {
                YandexGame.Instance.NewLeaderboardScores("Score", YandexGame.Instance.progressData.Score);
            }
           
            int nextLevel = level.level;
            nextLevel++;
            var currentLocotion = YandexGame.Instance.progressData.locations[1];

            if (currentLocotion.endindexLevel < nextLevel)
            {
                currentLocotion.completed = true;
                currentLocotion.isSelected = false;
                YandexGame.Instance.progressData.locations[2] = new Location()
                {
                    number = 2,
                    openLevels = 21,
                    startLevel = 21,
                    maxLevels = 20,
                    isLock = true,
                    isSelected = true
                };
            }

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

        internal void Matched(Obstacle obstacle)
        {
            throw new NotImplementedException();
        }
    }
}