using Match3;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class MoveController 
    {
        private readonly GridBoard gridBoard;
        private readonly MatchHandler matchHandler;

        public MoveController(GridBoard gridBoard, MatchHandler matchHandler)
        {
            this.gridBoard = gridBoard;
            this.matchHandler = matchHandler;
        }
       
        public void MoveGems()
        {
            //sort bottom left to top right, so we minimize timing issue (a BonusGem on top try to fall into a cell that is 
            //not yet empty but will be empty once the bottom BonusGem move away)
            var tickingCells = matchHandler.SortTickingCellsList();

            for (int i = 0; i < tickingCells.Count; i++)
            {
                var cellIdx = tickingCells[i];

                var currentCell = gridBoard.contentCell[cellIdx];
                var targetPosition = gridBoard.grid.GetCellCenterWorld(cellIdx);

                if (currentCell.IncomingGem != null && currentCell.ContainingGem != null)
                {
                    Debug.LogError(
                        $"A ticking cell at {cellIdx} have incoming gems {currentCell.IncomingGem} containing gem {currentCell.ContainingGem}");
                    continue;
                }

                //update either position or state.
                if (currentCell.IncomingGem?.CurrentState == Gem.State.Falling)
                {
                    var gem = currentCell.IncomingGem;
                    gem.TickMoveTimer(Time.deltaTime);

                    var maxDistance = gridBoard.visualSettings.FallAccelerationCurve.Evaluate(gem.FallTime) *
                                      Time.deltaTime * gridBoard.visualSettings.FallSpeed * gem.SpeedMultiplier;

                    gem.transform.position = Vector3.MoveTowards(gem.transform.position, targetPosition,
                        maxDistance);

                    if (gem.transform.position == targetPosition)
                    {
                        matchHandler.tickingCells.RemoveAt(i);
                        i--;

                        currentCell.IncomingGem = null;
                        currentCell.ContainingGem = gem;
                        gem.MoveTo(cellIdx);

                        //reached target position, now check if continue falling or finished its fall.
                        if (matchHandler.emptyCells.Contains(cellIdx + Vector3Int.down) && gridBoard.contentCell.TryGetValue(cellIdx + Vector3Int.down, out var belowCell))
                        {
                            //incoming BonusGem goes to the below cell
                            currentCell.ContainingGem = null;
                            belowCell.IncomingGem = gem;

                            gem.SpeedMultiplier = 1.0f;

                            var target = cellIdx + Vector3Int.down;
                            matchHandler.newTickingCells.Add(target);

                            matchHandler.emptyCells.Remove(target);
                            matchHandler.emptyCells.Add(cellIdx);

                            //if we continue falling, this is now an empty space, if there is a BonusGem above it will fall by itself
                            //but if this is a spawner above, we need to spawn a new BonusGem
                            if (gridBoard.spawnerPoints.Contains(cellIdx + Vector3Int.up))
                            {
                               gridBoard.ActivateSpawnerAt(cellIdx);
                            }
                        }
                        else if ((!gridBoard.contentCell.TryGetValue(cellIdx + Vector3Int.left, out var leftCell) ||
                                  leftCell.BlockFall) &&
                                 matchHandler.emptyCells.Contains(cellIdx + Vector3Int.down + Vector3Int.left) &&
                                 gridBoard.contentCell.TryGetValue(cellIdx + Vector3Int.down + Vector3Int.left, out var belowLeftCell))
                        {
                            //the cell to the left is either non existing or locked, and below that is an empty space, we can fall diagonally
                            currentCell.ContainingGem = null;
                            belowLeftCell.IncomingGem = gem;

                            gem.SpeedMultiplier = 1.41421356237f;

                            var target = cellIdx + Vector3Int.down + Vector3Int.left;
                            matchHandler.newTickingCells.Add(target);

                            //if the empty cell was part of the empty cell list, we need to remove it it's not empty anymore
                            matchHandler.emptyCells.Remove(target);
                            matchHandler.emptyCells.Add(cellIdx);

                            //if we continue falling, this is now an empty space, if there is a BonusGem above it will fall by itself
                            //but if this is a spawner above, we need to spawn a new BonusGem
                            if (gridBoard.spawnerPoints.Contains(cellIdx + Vector3Int.up))
                            {
                                gridBoard.ActivateSpawnerAt(cellIdx);
                            }
                        }
                        else if ((!gridBoard.contentCell.TryGetValue(cellIdx + Vector3Int.right, out var rightCell) ||
                                  rightCell.BlockFall) &&
                                 matchHandler.emptyCells.Contains(cellIdx + Vector3Int.down + Vector3Int.right) &&
                                 gridBoard.contentCell.TryGetValue(cellIdx + Vector3Int.down + Vector3Int.right, out var belowRightCell))
                        {
                            //we couldn't fall directly below, so we check diagonally
                            //incoming BonusGem goes to the below cell
                            currentCell.ContainingGem = null;
                            belowRightCell.IncomingGem = gem;

                            gem.SpeedMultiplier = 1.41421356237f;

                            var target = cellIdx + Vector3Int.down + Vector3Int.right;
                            matchHandler.newTickingCells.Add(target);

                            //if the empty cell was part of the empty cell list, we need to remove it it's not empty anymore
                            matchHandler.emptyCells.Remove(target);
                            matchHandler.emptyCells.Add(cellIdx);

                            //if we continue falling, this is now an empty space, if there is a BonusGem above it will fall by itself
                            //but if this is a spawner above, we need to spawn a new BonusGem
                            if (gridBoard.spawnerPoints.Contains(cellIdx + Vector3Int.up))
                            {
                                gridBoard.ActivateSpawnerAt(cellIdx);
                            }
                        }
                        else
                        {
                            //re add but this time we will bounce and not fall.
                            matchHandler.newTickingCells.Add(cellIdx);
                            gem.StopFalling();
                        }
                    }
                }
                else if (currentCell.ContainingGem?.CurrentState == Gem.State.Bouncing)
                {
                    var gem = currentCell.ContainingGem;
                    gem.TickMoveTimer(Time.deltaTime);
                    Vector3 center = gridBoard.grid.GetCellCenterWorld(cellIdx);

                    float maxTime = gridBoard.visualSettings.BounceCurve
                        .keys[gridBoard.visualSettings.BounceCurve.length - 1].time;

                    if (gem.FallTime >= maxTime)
                    {
                        gem.transform.position = center;
                        gem.transform.localScale = Vector3.one;
                        gem.StopBouncing();

                        matchHandler.tickingCells.RemoveAt(i);
                        i--;
                        matchHandler.cellToMatchCheck.Add(cellIdx);
                    }
                    else
                    {
                        gem.transform.position =
                            center + Vector3.up * gridBoard.visualSettings.BounceCurve.Evaluate(gem.FallTime);
                        gem.transform.localScale =
                            new Vector3(1, gridBoard.visualSettings.SquishCurve.Evaluate(gem.FallTime), 1);
                    }
                }
                else if (currentCell.ContainingGem?.CurrentState == Gem.State.Still)
                {
                    //a ticking cells should only be falling or bouncing, if neither of those, remove it 
                    matchHandler.tickingCells.RemoveAt(i);
                    i--;
                }

            }
           
        }

    }
}