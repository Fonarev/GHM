using Assets.GameMains.Scripts;
using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using Match3;

using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Assets.GemHunterMatch.Scripts
{
    public class GridBoard : MonoBehaviour
    {
        public Gem[] existingGems;
        public BonusSetting bonusSettings;
        public BonusGemBonusItem activatedBonus;

        public static GridBoard Instance => instance;
       
        public List<Vector3Int> spawnerPoints { get; private set; } = new();
        public Dictionary<Vector3Int, BoardCell> contentCell = new();

        public Dictionary<Vector3Int, Action> cellsCallbacks = new();
        public Dictionary<Vector3Int, Action> matchedCallback = new();

        public bool IncrementHintTimer { get; set; }
        public bool BoardChanged { get; set; }
        public int FreezeMoveLock { get; private set; }
        public PoolVFX PoolVFX { get; set; }
        public MatchHandler MatchHandler { get; set; }
        public Grid Grid => GetComponent<Grid>();

        private MoveController moveController;
        private InputHandler inputHandler;
        private SwapHandler swapHandler;
        private HintShowMatches hint;
        private Placements placement;
        private VFXController effectController;
        private GamePlay gamePlay;

        private bool isInit;
        private List<IBoardAction> boardActions = new();
        private static GridBoard instance;

        private void Awake()
        {
            if (Instance == null) instance = this;
        }

        private void Update()
        {
            if (!isInit) return;

            HandleBonusAction();

            if (gamePlay.IsPlaying)
                inputHandler.UpData();

            PoolVFX.UpDate();

            IncrementHintTimer = activatedBonus == null;

            swapHandler.UpData();

            if (MatchHandler.tickingCells.Count > 0) moveController.MoveGems();

            MatchHandler.UpData();

            hint.Show(IncrementHintTimer);

        }

        public void Initialize(GamePlay gamePlay)
        {
            this.gamePlay = gamePlay;

            PoolVFX = new(transform);

            placement = new(Instance);
            placement.FillBoardGems();

            effectController = new(gamePlay.visualSettings);
            effectController.Instatiate(gamePlay.transform);

            MatchHandler = new(gamePlay, this, placement);
            MatchHandler.FindAllPossibleMatch();

            moveController = new(gamePlay, this, MatchHandler);

            swapHandler = new(gamePlay, this, MatchHandler);

            inputHandler = new(this, gamePlay, swapHandler, effectController, Camera.main);

            hint = new(MatchHandler, this, gamePlay.visualSettings);
            hint.Instatiate(gamePlay.transform);

            isInit = true;
        }

        public static void RegisterCell(Vector3Int cellPosition, Gem startingGem = null)
        {
            CheckInstance();

            if (!Instance.contentCell.ContainsKey(cellPosition))
                Instance.contentCell.Add(cellPosition, new BoardCell());

            if (startingGem != null)
                Instance.NewGemAt(cellPosition, startingGem);
        }
        public static void RegisterSpawnerPoint(Vector3Int cell)
        {
            CheckInstance();
            Instance.spawnerPoints.Add(cell);
        }
        public static void AddObstacle(Vector3Int cell, Obstacle obstacle)
        {
            RegisterCell(cell);

            obstacle.transform.position = Instance.Grid.GetCellCenterWorld(cell);
            Instance.contentCell[cell].Obstacle = obstacle;
        }
        public static void ChangeLock(Vector3Int cellPosition, bool lockState)
        {
            CheckInstance();
            Instance.contentCell[cellPosition].Locked = lockState;
        }
        public static void RegisterDeletedCallback(Vector3Int cellPosition, Action callback)
        {
            CheckInstance();
            if (!Instance.cellsCallbacks.ContainsKey(cellPosition))
            {
                Instance.cellsCallbacks[cellPosition] = callback;
            }
            else
            {
                Instance.cellsCallbacks[cellPosition] += callback;
            }
        }
        public static void RegisterMatchedCallback(Vector3Int cellPosition, Action callback)
        {
            if (!Instance.matchedCallback.ContainsKey(cellPosition))
            {
                Instance.matchedCallback[cellPosition] = callback;
            }
            else
            {
                Instance.matchedCallback[cellPosition] += callback;
            }
        }
        public static void UnregisterMatchedCallback(Vector3Int cellPosition, Action callback)
        {
            if (!Instance.matchedCallback.ContainsKey(cellPosition))
                return;

            Instance.matchedCallback[cellPosition] -= callback;
            if (Instance.matchedCallback[cellPosition] == null)
                Instance.matchedCallback.Remove(cellPosition);
        }
        private static void CheckInstance()
        {

            if (Instance == null)
                instance = GameObject.Find(LevelDatabase.GetLevel(GlobalMediator.instance.SelectLevel).gridBoardReference + "(Clone)").GetComponent<GridBoard>();
        }

        public void UnregisterDeletedCallback(Vector3Int cellPosition, Action callback)
        {
            if (!Instance.cellsCallbacks.ContainsKey(cellPosition))
                return;

            Instance.cellsCallbacks[cellPosition] -= callback;
            if (Instance.cellsCallbacks[cellPosition] == null)
                Instance.cellsCallbacks.Remove(cellPosition);
        }
        public Gem NewGemAt(Vector3Int cell, Gem gemPrefab)
        {
            if (gemPrefab == null)
                gemPrefab = Instance.existingGems[Random.Range(0, Instance.existingGems.Length)];

            if (gemPrefab.effectMatchPrefabs.Length != 0)
            {
                foreach (var matchEffectPrefab in gemPrefab.effectMatchPrefabs)
                {
                    //GameManager.Instance.PoolSystem.AddNewInstance(matchEffectPrefab, 16);
                }
            }

            //New Gem may be called after the board was init (as startup doesn't seem to be reliably called BEFORE init)
            if (Instance.contentCell[cell].ContainingGem != null)
            {
                Destroy(Instance.contentCell[cell].ContainingGem.gameObject);
            }

            var gem = Instantiate(gemPrefab, Instance.Grid.GetCellCenterWorld(cell), Quaternion.identity);
            Instance.contentCell[cell].ContainingGem = gem;
            gem.Init(cell);

            return gem;
        }
        public void ActivateSpawnerAt(Vector3Int cell)
        {
            var gem = Instantiate(existingGems[Random.Range(0,existingGems.Length)], Grid.GetCellCenterWorld(cell + Vector3Int.up), Quaternion.identity);
            contentCell[cell].IncomingGem = gem;

            gem.StartMoveTimer();
            gem.SpeedMultiplier = 1.0f;
            MatchHandler.newTickingCells.Add(cell);

            if (MatchHandler.emptyCells.Contains(cell)) MatchHandler.emptyCells.Remove(cell);
        }

        public Vector3 GetCellCenter(Vector3Int cell) => Instance.Grid.GetCellCenterWorld(cell);
        public Vector3Int WorldToCell(Vector3 pos) => Instance. Grid.WorldToCell(pos);
        public void LockMovement() => FreezeMoveLock += 1;
        public void UnlockMovement() => FreezeMoveLock -= 1;
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

            MatchHandler.tickingMatch.Add(match);
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

    }
}