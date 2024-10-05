using Match3;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class InputHandler 
    {
        public bool PressedThisFrame { get; private set; }
        public bool ReleasedThisFrame { get; private set; }
       
        public Vector3 WorldPos { get => worldPos; private set { worldPos = value; worldPos.z = 0; } }
        public Vector3 WorldStart { get; private set; }
        public Vector3Int StartPosCell { get=> startPosCell; private set { startPosCell = value; startPosCell.z = 0; } }

        private Vector2 clickPos;
        private Vector2 startClickPos;
        private Vector3 worldPos;
        private Vector3Int startPosCell;
       
        private Camera mainCam;
        private bool isHoldingTouch;
        private float lastClickTime = 0.0f;
       
        private readonly VFXController effectController;
        private readonly GridBoard gridBoard;
        private readonly GamePlay gamePlay;
        private readonly SwapHandler swapHandler;

        public InputHandler(GridBoard gridBoard, GamePlay gamePlay, SwapHandler swapHandler, VFXController effectController, Camera mainCam)
        {
            this.gridBoard = gridBoard;
            this.gamePlay = gamePlay;
            this.swapHandler = swapHandler;
            this.effectController = effectController;
            this.mainCam = mainCam;
            lastClickTime = Time.time;
        }

        public void UpData()
        {
            PressedThisFrame = InputService.instance.ClickAction.WasPressedThisFrame();
            ReleasedThisFrame = InputService.instance.ClickAction.WasReleasedThisFrame();
         
            clickPos = InputService.instance.ClickPosition.ReadValue<Vector2>();
            WorldPos = mainCam.ScreenToWorldPoint(clickPos);
            CheckInput();
        }

        private void CheckInput()
        {
            effectController.SetPos(WorldPos);

            if (PressedThisFrame)
            {
                Vector3Int startCell = GetStartPosCell(gridBoard.grid);

                if (gridBoard.activatedBonus != null)
                {
                    Vector3Int clickedCell = gridBoard.grid.WorldToCell(WorldPos);

                    if (gridBoard.contentCell.TryGetValue(clickedCell, out var content) && content.ContainingGem != null)
                    {
                        gamePlay.UseBonusItem(gridBoard.activatedBonus, clickedCell);
                        gridBoard.activatedBonus = null;
                        return;
                    }
                }

                if (gridBoard.contentCell.ContainsKey(startCell))
                {
                    effectController.ShowVFX(gridBoard.grid.GetCellCenterWorld(startCell), WorldPos);
                }
            }
            else if (ReleasedThisFrame)
            {
                isHoldingTouch = false;

                effectController.HideVFX();

                float clickDelta = Time.time - lastClickTime;
                lastClickTime = Time.time;

                //if last than .3 second since last click, this is a double click, activate the BonusGem if that is a BonusGem.
                if (clickDelta < 0.3f)
                {
                    if (gridBoard.contentCell.TryGetValue(StartPosCell, out var content)
                        && content.ContainingGem != null
                        && content.ContainingGem.Usable
                        && content.ContainingGem.CurrentMatch == null)
                    {
                        content.ContainingGem.Use(null);
                        return;
                    }
                }

                Vector3 endWorldPos = GetEndPos();

                //we compute the swipe in world position as then a swipe of 1 is the distance between 2 cell
                Vector3 swipe = endWorldPos - WorldStart;

                if (swipe.sqrMagnitude < 0.5f * 0.5f)
                    return;

                //the starting cell isn't a valid cell, so we exit
                if (!gridBoard.contentCell.TryGetValue(StartPosCell, out var startCellContent) || !startCellContent.CanBeMoved)
                    return;

                Vector3Int endCell = DirectionsSwipe(StartPosCell, swipe);

                //the ending cell isn't a valid cell, exit
                if (!gridBoard.contentCell.TryGetValue(endCell, out var endCellContent) || !endCellContent.CanBeMoved)
                    return;

                swapHandler.SetSwap(StartPosCell, endCell);
            }
        }

        private Vector3Int DirectionsSwipe(Vector3Int startCell, Vector3 swipe)
        {
            var endCell = startCell;

            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
            {
                endCell = swipe.x < 0 ? endCell += Vector3Int.left : endCell += Vector3Int.right;
            }
            else
            {
                endCell = swipe.y > 0 ? endCell += Vector3Int.up : endCell += Vector3Int.down;
            }

            return endCell;
        }

        public Vector3Int GetStartPosCell(Grid grid)
        {
            startClickPos = clickPos;
            WorldStart = mainCam.ScreenToWorldPoint(startClickPos);
            StartPosCell = grid.WorldToCell(WorldStart);

            return StartPosCell;
        }

        public Vector3 GetEndPos()=> mainCam.ScreenToWorldPoint(clickPos);
     
    }
}