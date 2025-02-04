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

namespace Assets.GemHunterMatch.Scripts
{
    public class GamePlay : MonoBehaviour
    {
        public event Action<int, int,bool> OnGoalChanged;
        public event Action<string> OnShowMessages;
        public event Action<bool> OnAllGoalFinished;
        public event Action<int,int> OnAddScore;
        public event Action<int> OnMoveHappened;
        public event Action<int> OnMoveTriger;
        public event Action<int,int> OnUsedBonusItem;
        public event Action<Gem> OnMachtedGem;
        public event Action<UnderGem> OnMachted;

        public VisualSetting visualSettings;
        public BonusGemBonusItem[] bonusList;
        public BonusGem[] bonusFiniches;
        public Dictionary<int, BonusGemBonusItem> bonusItems = new();
        public bool IsPlaying { get; private set; }
        public int GoalLeft { get; private set; }
        public int RemainingMove
        {
            get => remainingMove;

            private set
            {
                int oldMove = remainingMove;

                if (remainingMove > 0)
                    remainingMove = value;

                if (oldMove != remainingMove)
                    OnMoveHappened?.Invoke(RemainingMove);
            }
        }

        public List<Goals> Goals = new();
        public int Score{ get; private set; }
        private LevelConfig level;
        private GridBoard gridBoard;
      
        private Wallet wallet;
        private bool isConditions;
        private LevelFinishHandler levelFinishHandler;
        private int remainingMove;

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
            remainingMove = level.MaxMove;
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
            ComputeCamera();
            levelFinishHandler = new(gridBoard, this);
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
            remainingMove = moves;
            OnMoveHappened?.Invoke(RemainingMove);
            Play();
        }
        public int SubtractMove()
        {
            RemainingMove = Mathf.Max(0, RemainingMove - 1);

            return RemainingMove;
        }
        public void Moved()
        {
            var prev = RemainingMove;

            SubtractMove();

            if (prev > level.LowMoveTrigger && RemainingMove <= level.LowMoveTrigger)
            {
                OnMoveTriger.Invoke(RemainingMove);
            }

            if (RemainingMove <= 0)
            {
                Finish(isConditions);
            }
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
            this.isConditions = isConditions;
            IsPlaying = false;
            StartCoroutine(ShowVisualFinish());
        }
      
        private IEnumerator ShowVisualFinish()
        {
            yield return HandlerCoroutine.StartRoutine(levelFinishHandler.WaitForBoardChanged());

            if (isConditions)
            {
                OnShowMessages.Invoke("victory");
                AudioManager.instance.PlayEffect("chime");
 
                yield return HandlerCoroutine.StartRoutine(levelFinishHandler.ToFinish());
                yield return HandlerCoroutine.StartRoutine(SetCompletedLevel());
                yield return new WaitForSeconds(0.1f);

                OnAllGoalFinished.Invoke(isConditions);
            }
            else
            {
                OnShowMessages.Invoke("fail");
                AudioManager.instance.PlayEffect("jingle_chime");

                yield return new WaitForSeconds(0.5f);

                OnAllGoalFinished.Invoke(isConditions);
            }
        }

        private IEnumerator SetCompletedLevel()
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
            yield return null;
        }

        public void AddCoins(int amount)
        {
            wallet.Add(amount);
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

        public void ComputeCamera()
        {
            //setup the camera so it look at the center of the play area, and change its ortho setting so it perfectly frame
            var bounds = gridBoard.Bounds;
            Vector3 center = gridBoard.Grid.CellToLocalInterpolated(bounds.center) + new Vector3(0.5f, 0.5f, 0.0f);
            center = gridBoard.transform.TransformPoint(center);

            //we offset of 1 up as the top bar is thicker, so this center it better between the top & bottom bar
            Camera.main.transform.position = center + Vector3.back * 10.0f + Vector3.up * 0.75f;

            float halfSize = 0.0f;

            if (Screen.height > Screen.width)
            {
                float screenRatio = Screen.height / (float)Screen.width;
                halfSize = ((bounds.size.x + 1) * 0.5f + visualSettings.BorderMargin) * screenRatio;
            }
            else
            {
                //On Wide screen, we fit vertically
                halfSize = (bounds.size.y + 3) * 0.5f + visualSettings.BorderMargin;
            }

            halfSize += visualSettings.BorderMargin;

            Camera.main.orthographicSize = halfSize;
        }
        //public void UpdateVolumes()
        //{
        //    if (MusicSourceActive.volume < 1.0f)
        //    {
        //        MusicSourceActive.volume = Mathf.MoveTowards(MusicSourceActive.volume, 1.0f, Time.deltaTime * 0.5f);
        //        MusicSourceBackground.volume = Mathf.MoveTowards(MusicSourceBackground.volume, 0.0f, Time.deltaTime * 0.5f);
        //    }
        //    Settings.SoundSettings.Mixer.SetFloat("MainVolume", Mathf.Log10(Mathf.Max(0.0001f, m_SoundData.MainVolume)) * 30.0f);
        //    Settings.SoundSettings.Mixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(0.0001f, m_SoundData.SFXVolume)) * 30.0f);
        //    Settings.SoundSettings.Mixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(0.0001f, m_SoundData.MusicVolume)) * 30.0f);
        //}
    }
}