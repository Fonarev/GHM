using Assets.AssetLoaders;
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.EntryPoints;
using Assets.GameMains.Scripts.Expansion;
using Assets.GemHunterMatch.Scripts.UI;

using Match3;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    public class GamePlay : MonoBehaviour
    { public class Goals
    {
            public Gem gem;
            public int count;
    }
        public event Action<int, int> OnGoalChanged;
        public event Action OnAllGoalFinished;
        public event Action<int> OnMoveHappened;

        public static GamePlay instance;
        public bool IsPlaying { get; private set; }
        public int GoalLeft { get; private set; }
        public int RemainingMove { get; private set; }

        private List<Goals> gemGoals = new();
        private LevelConfig level;
        public GridBoard gridBoard;
        public UIGamePlay ui;

        private void Awake()
        {
            instance = this;
        }

        public void Initialize()
        {
            CoroutineHandler.StartRoutine(Load());
            RemainingMove = level.MaxMove;
            foreach (var item in level.Goals)
            {
                var goal = new Goals();
                goal.gem = item.Gem;
                goal.count = item.Count;
                gemGoals.Add(goal);
            }
           
            GoalLeft = gemGoals.Count;
            ui.Initialize(this, level);
        }

        private IEnumerator Load()
        {
            level = LevelDatabase.GetLevel(GlobalMediator.instance.SelectLevel);

            yield return CoroutineHandler.StartRoutine(LoaderAsset.InstantiateAsset<GridBoard>(level.gridBoardReference, null, op => { gridBoard = op; }));

            gridBoard.Initialize(this, level);
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

        public void ChangeCoins(int v)
        {
            AudioManager.instance.PlayEffect("coin");
        }
    }
}