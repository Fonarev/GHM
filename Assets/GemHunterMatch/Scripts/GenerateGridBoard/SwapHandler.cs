using Assets.GameMains.Scripts.AudiosSources;

using Match3;

using System.Collections.Generic;

using UnityEngine;


namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class SwapHandler
    {
        private float swapSpeed;
        private bool swipeQueued;
        private Vector3Int startSwipe;
        private Vector3Int endSwipe;
        private (Vector3Int, Vector3Int) swappingCells;
        private SwapStage SwapStage = SwapStage.None;
        private Dictionary<Vector3Int, BoardCell> contentCell;
       
        private readonly GridBoard gridBoard;
        private readonly MatchHandler matchHandler;

        public SwapHandler(GridBoard gridBoard, MatchHandler matchHandler, float swapSpeed = 10)
        {
            this.gridBoard = gridBoard;
            this.matchHandler = matchHandler;
            this.swapSpeed = swapSpeed;
        }

        public void UpData()
        {
            if (SwapStage != SwapStage.None)
            {
                TickSwap();
                return;
            }

            if (swipeQueued)
                SwapeQueued();
        }

        public void SetSwap(Vector3Int startPosCell, Vector3Int endCell)
        {
            startSwipe = startPosCell;
            endSwipe = endCell;
            swipeQueued = true;
        }

        private void TickSwap()
        {
            contentCell = gridBoard.contentCell;

            Gem gemToStart = contentCell[swappingCells.Item1].IncomingGem;
            Gem gemToEnd = contentCell[swappingCells.Item2].IncomingGem;

            Vector3 startPosition = gridBoard.grid.GetCellCenterWorld(swappingCells.Item1);
            Vector3 endPosition = gridBoard.grid.GetCellCenterWorld(swappingCells.Item2);

            gemToStart.transform.position =
                Vector3.MoveTowards(gemToStart.transform.position, startPosition, Time.deltaTime * swapSpeed);
            gemToEnd.transform.position =
                Vector3.MoveTowards(gemToEnd.transform.position, endPosition, Time.deltaTime * swapSpeed);

            if (gemToStart.transform.position == startPosition)
            {
                //swapHandler if finished
                if (SwapStage == SwapStage.Forward)
                {
                    contentCell[swappingCells.Item1].ContainingGem = contentCell[swappingCells.Item1].IncomingGem;
                    contentCell[swappingCells.Item2].ContainingGem = contentCell[swappingCells.Item2].IncomingGem;

                    contentCell[swappingCells.Item1].ContainingGem.MoveTo(swappingCells.Item1);
                    contentCell[swappingCells.Item2].ContainingGem.MoveTo(swappingCells.Item2);

                    bool firstCheck = Check(swappingCells.Item1,swappingCells.Item2);
                    bool secondCheck = Check(swappingCells.Item2, swappingCells.Item1);

                    if (firstCheck || secondCheck)
                    {
                        contentCell[swappingCells.Item1].IncomingGem = null;
                        contentCell[swappingCells.Item2].IncomingGem = null;

                        SwapStage = SwapStage.None;

                        // as swapHandler was successful, we count down 1 move from the level
                        GamePlay.instance.Moved();
                    }
                    else
                    {
                        //if there is no match, we revert the swapHandler
                        (contentCell[swappingCells.Item1].IncomingGem, contentCell[swappingCells.Item2].IncomingGem) = (
                            contentCell[swappingCells.Item2].IncomingGem, contentCell[swappingCells.Item1].IncomingGem);
                        (swappingCells.Item1, swappingCells.Item2) = (swappingCells.Item2, swappingCells.Item1);
                        SwapStage = SwapStage.Return;
                    }
                }
                else
                {
                    contentCell[swappingCells.Item1].ContainingGem = contentCell[swappingCells.Item1].IncomingGem;
                    contentCell[swappingCells.Item2].ContainingGem = contentCell[swappingCells.Item2].IncomingGem;

                    contentCell[swappingCells.Item1].ContainingGem.MoveTo(swappingCells.Item1);
                    contentCell[swappingCells.Item2].ContainingGem.MoveTo(swappingCells.Item2);

                    contentCell[swappingCells.Item1].IncomingGem = null;
                    contentCell[swappingCells.Item2].IncomingGem = null;

                    SwapStage = SwapStage.None;
                }
            }
        }

        private void SwapeQueued()
        {
            contentCell = gridBoard.contentCell;
            contentCell[startSwipe].IncomingGem = contentCell[endSwipe].ContainingGem;
            contentCell[endSwipe].IncomingGem = contentCell[startSwipe].ContainingGem;

            contentCell[startSwipe].ContainingGem = null;
            contentCell[endSwipe].ContainingGem = null;

            SwapStage = SwapStage.Forward;
            swappingCells = (startSwipe, endSwipe);

            AudioManager.instance.PlayEffect("swipe");

            swipeQueued = false;
            gridBoard.incrementHintTimer = false;
        }

        private bool Check(Vector3Int item1, Vector3Int item2)
        {
            if (contentCell[item1].ContainingGem.Usable)
            {
                contentCell[item1].ContainingGem.Use(contentCell[item2].ContainingGem);
                return true;
            }

            return matchHandler.DoCheck(item1);
        }

       
    }
}