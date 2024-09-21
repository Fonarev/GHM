using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using Match3;

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;

using Random = UnityEngine.Random;

namespace Assets.GemHunterMatch.Scripts
{
    public class GridBoard : MonoBehaviour
    {
        public Gem[] existingGems;
        public float inactivityBeforeHint = 8.0f;
        public VisualEffect gemHoldPrefab;
        public VisualEffect holdTrailPrefab;
        public VisualSetting visualSettings;
        public BonusSetting bonusSettings;
        public static GridBoard instance;
        public Grid grid => GetComponent<Grid>();

        public List<Vector3Int> spawnerPoints { get; private set; } = new();
        public Dictionary<Vector3Int, BoardCell> contentCell = new();

        public Dictionary<Vector3Int, Action> cellsCallbacks = new();
        public Dictionary<Vector3Int, Action> matchedCallback = new();

        private static GenerateGem generateGem;
        private VFXController effectController;
        public MatchHandler matchHandler;
        private MoveController moveController;
        private InputHandler inputHandler;
        private SwapHandler swapHandler;
        private HintShowMatches hint;

        private GamePlay gameEntry;
        public LevelConfig levelConfig => _levelConfig;
        public BonusItem activatedBonus;
        public bool incrementHintTimer{ get; set; }
        private bool isInit;
        public int freezeMoveLock { get; private set; }
        private List<IBoardAction> boardActions = new();
        private LevelConfig _levelConfig;

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        public void Initialize(GamePlay gamePlay, LevelConfig levelConfig)
        { 
            this.gameEntry = gamePlay;
            _levelConfig = levelConfig;

            if (generateGem == null) 
                generateGem = new(this);

            generateGem.FillBoardGems();

            effectController = new(gemHoldPrefab, holdTrailPrefab);
            effectController.Instatiate(transform);

            matchHandler = new(this, generateGem);
            matchHandler.FindAllPossibleMatch();
            moveController = new(this, matchHandler);
            swapHandler = new(this,matchHandler);
            inputHandler = new(this, swapHandler, effectController, Camera.main);
            hint = new(matchHandler, grid, inactivityBeforeHint);
            hint.Instatiate(visualSettings.HintPrefab);
            isInit = true;
        }

        public static void RegisterCell(Vector3Int cellPosition, Gem startingGem = null)
        {
            CheckInstance();

            if (!instance.contentCell.ContainsKey(cellPosition))
                instance.contentCell.Add(cellPosition, new BoardCell());

            if (startingGem != null)
            {
               if(generateGem == null)
                  generateGem = new(instance);

                generateGem.NewGemAt(cellPosition, startingGem);
            }
        }
        public static void RegisterSpawnerPoint(Vector3Int cell)
        {
            CheckInstance();
            instance.spawnerPoints.Add(cell);
        }
        public static void AddObstacle(Vector3Int cell, Obstacle obstacle)
        {
            RegisterCell(cell);

            obstacle.transform.position = instance.grid.GetCellCenterWorld(cell);
            instance.contentCell[cell].Obstacle = obstacle;
        }
        public static void ChangeLock(Vector3Int cellPosition, bool lockState)
        {
            CheckInstance();

            instance.contentCell[cellPosition].Locked = lockState;
        }
        public static void RegisterDeletedCallback(Vector3Int cellPosition, System.Action callback)
        {
            CheckInstance();
            if (!instance.cellsCallbacks.ContainsKey(cellPosition))
            {
                instance.cellsCallbacks[cellPosition] = callback;
            }
            else
            {
                instance.cellsCallbacks[cellPosition] += callback;
            }
        }
        public static void UnregisterDeletedCallback(Vector3Int cellPosition, System.Action callback)
        {
            if (!instance.cellsCallbacks.ContainsKey(cellPosition))
                return;

            instance.cellsCallbacks[cellPosition] -= callback;
            if (instance.cellsCallbacks[cellPosition] == null)
                instance.cellsCallbacks.Remove(cellPosition);
        }
        public static void RegisterMatchedCallback(Vector3Int cellPosition, System.Action callback)
        {
            if (!instance.matchedCallback.ContainsKey(cellPosition))
            {
                instance.matchedCallback[cellPosition] = callback;
            }
            else
            {
                instance.matchedCallback[cellPosition] += callback;
            }
        }
        public static void UnregisterMatchedCallback(Vector3Int cellPosition, System.Action callback)
        {
            if (!instance.matchedCallback.ContainsKey(cellPosition))
                return;

            instance.matchedCallback[cellPosition] -= callback;
            if (instance.matchedCallback[cellPosition] == null)
                instance.matchedCallback.Remove(cellPosition);
        }
        private static void CheckInstance()
        {
            if (instance == null)
                instance = GameObject.Find("Grid(Clone)").GetComponent<GridBoard>();
        }

        public void ActivateSpawnerAt(Vector3Int cell)
        {
            var gem = Instantiate(existingGems[Random.Range(0,existingGems.Length)], grid.GetCellCenterWorld(cell + Vector3Int.up), Quaternion.identity);
            contentCell[cell].IncomingGem = gem;

            gem.StartMoveTimer();
            gem.SpeedMultiplier = 1.0f;
            matchHandler.newTickingCells.Add(cell);

            if (matchHandler.emptyCells.Contains(cell)) matchHandler.emptyCells.Remove(cell);
        }

        public Vector3 GetCellCenter(Vector3Int cell) => grid.GetCellCenterWorld(cell);
        public Vector3Int WorldToCell(Vector3 pos) => grid.WorldToCell(pos);
        public void LockMovement() => freezeMoveLock += 1;
        public void UnlockMovement() => freezeMoveLock -= 1;
        public void DestroyGem(Vector3Int cell, bool forcedDeletion = false)
        {
            if (contentCell[cell].ContainingGem?.CurrentMatch != null)
                return;

            var match = new Match()
            {
                DeletionTimer = 0.0f,
                MatchingGem = new List<Vector3Int> { cell },
                OriginPoint = cell,
                SpawnedBonus = null,
                ForcedDeletion = forcedDeletion
            };

            if (contentCell[cell].ContainingGem != null)
            {
                contentCell[cell].ContainingGem.CurrentMatch = match;
            }

            matchHandler.tickingMatch.Add(match);
        }
        public void AddNewBoardAction(IBoardAction action) => boardActions.Add(action);
        private void HandleBonusAction()
        {
            for (int i = 0; i < boardActions.Count; ++i)
            {
                if (!boardActions[i].Tick())
                {
                    boardActions.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Update()
        {
            if (!isInit) return;

            HandleBonusAction();

            inputHandler.UpData();

            incrementHintTimer = activatedBonus == null;

            swapHandler.UpData();

            if (matchHandler.tickingCells.Count > 0) moveController.MoveGems();

            matchHandler.UpData();

            hint.Show(incrementHintTimer);
          
        }

    }
}