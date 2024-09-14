using Assets.GameMains.Scripts.EntryPoints;
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
        public VisualSetting visualSetting;
       

        public static GridBoard instance;
        public Grid grid => GetComponent<Grid>();

        public List<Vector3Int> spawnerPoints = new();
        public Dictionary<Vector3Int, BoardCell> contentCell = new();

        public Dictionary<Vector3Int, Action> cellsCallbacks = new();
        public Dictionary<Vector3Int, Action> matchedCallback = new();

        private static GenerateGem generateGem;
        private VFXController effectController;
        private MatchHandler matchHandler;
        private MoveController moveController;
        private InputHandler inputHandler;
        private SwapHandler swapHandler;
       
        private GameEntryPoint gameEntry;
        private GameObject hintIndicator;
        private float sinceLastHint;
        internal BonusItem m_ActivatedBonus;
        public bool incrementHintTimer { get; set; }

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        public void Initialize(GameEntryPoint gameEntry)
        {
            this.gameEntry = gameEntry;

            if (generateGem == null) 
                generateGem = new(this);

            generateGem.FillBoardGems();

            effectController = new(gemHoldPrefab, holdTrailPrefab);
            effectController.InstatiateVFX(transform);

            matchHandler = new(this, generateGem);
            matchHandler.FindAllPossibleMatch();
            moveController = new(this, matchHandler);
            swapHandler = new(this,matchHandler);
            inputHandler = new(this, swapHandler, effectController, Camera.main);

            hintIndicator = Instantiate(visualSetting.HintPrefab);
            hintIndicator.SetActive(false);
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
            var gem = Instantiate(existingGems[Random.Range(0, existingGems.Length)], grid.GetCellCenterWorld(cell + Vector3Int.up), Quaternion.identity);
            contentCell[cell].IncomingGem = gem;

            gem.StartMoveTimer();
            gem.SpeedMultiplier = 1.0f;
            matchHandler.newTickingCells.Add(cell);

            if (matchHandler.emptyCells.Contains(cell)) matchHandler.emptyCells.Remove(cell);
        }

        private void Update()
        {
            if (inputHandler == null) return;
            incrementHintTimer = m_ActivatedBonus == null;
            inputHandler.UpData();

            if (swapHandler != null) swapHandler.UpData();

            if (matchHandler.tickingCells.Count > 0) moveController.MoveGems();

            if (matchHandler != null) matchHandler.UpData();

            if (incrementHintTimer)
            {
                //nothing can happen anymore, if we were in the last stretch trigger the end
                //if (m_FinalStretch)
                //{
                //    //this stop the end to be called in a loop. Input is still disabled to user cannot interact with board
                //    m_FinalStretch = false;
                //    UIHandler.Instance.ShowEnd();
                //    return;
                //}

                //Nothing happened this frame, but the board was changed since last possible match check, so need to refresh
                if (matchHandler.boardChanged)
                {
                    matchHandler.FindAllPossibleMatch();
                    matchHandler.boardChanged = false;
                }

                var match = matchHandler.GetMatch();
                if (match == null) return;
                ShowHint(match);
            }
            else
            {
                hintIndicator.SetActive(false);
                sinceLastHint = 0.0f;
            }
        }

        private void ShowHint(PossibleSwap match)
        {
            if (hintIndicator.activeSelf)
            {
                var startPos = grid.GetCellCenterWorld(match.StartPosition);
                var endPos = grid.GetCellCenterWorld(match.StartPosition + match.Direction);

                var current = hintIndicator.transform.position;
                current = Vector3.MoveTowards(current, endPos, 1.0f * Time.deltaTime);

                hintIndicator.transform.position = current == endPos ? startPos : current;
            }
            else
            {
                sinceLastHint += Time.deltaTime;
                if (sinceLastHint >= inactivityBeforeHint)
                {
                    hintIndicator.transform.position = grid.GetCellCenterWorld(match.StartPosition);
                    hintIndicator.SetActive(true);
                }
            }
        }
    }
}