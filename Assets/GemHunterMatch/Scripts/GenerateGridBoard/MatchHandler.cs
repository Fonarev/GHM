using Assets.GameMains.Scripts.AudiosSources;

using Match3;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Random = UnityEngine.Random;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class MatchHandler
    {
        public List<Match> tickingMatch { get; set; } = new();
        public List<Vector3Int> tickingCells { get; set; } = new();
        public List<Vector3Int> newTickingCells { get; set; } = new();
        public List<Vector3Int> cellToMatchCheck { get; set; } = new();
        public List<Vector3Int> emptyCells { get; set; } = new();
        public List<PossibleSwap> possibleSwaps { get; set; } = new();

        private int pickedSwap;
     
        private readonly GamePlay gamePlay;
        private readonly GridBoard gridBoard;
        private readonly Placements generateGem;

        public MatchHandler(GamePlay gamePlay, GridBoard gridBoard, Placements generateGem)
        {
            this.gamePlay = gamePlay;
            this.gridBoard = gridBoard;
            this.generateGem = generateGem;
        }

        public void UpData()
        {
            if (cellToMatchCheck.Count > 0)
            {
                DoMatchCheck();

                gridBoard.IncrementHintTimer = false;
                //gridBoard.BoardChanged = true;
            }
            if (tickingMatch.Count > 0)
            {
                MatchTicking();
               
                gridBoard.IncrementHintTimer = false;
                //gridBoard.BoardChanged = true;
            }
            if (emptyCells.Count > 0)
            {
                EmptyCheck();

                gridBoard.IncrementHintTimer = false;
                gridBoard.BoardChanged = true;
               
            }
            if (newTickingCells.Count > 0)
            {
                tickingCells.AddRange(newTickingCells);
                newTickingCells.Clear();
                gridBoard.IncrementHintTimer = false;
            }
        }

        public PossibleSwap GetMatch()
        {
            if (possibleSwaps.Count > 0) 
                return possibleSwaps[pickedSwap];

            return null;
        }

        public List<Vector3Int> SortTickingCellsList()
        {
            tickingCells.Sort((a, b) =>
            {
                int yCmp = a.y.CompareTo(b.y);
                if (yCmp == 0)
                {
                    return a.x.CompareTo(b.x);
                }

                return yCmp;
            });

            return tickingCells;
        }

        public void FindAllPossibleMatch()
        {
            possibleSwaps.Clear();

            for (int y = generateGem.Bounds.yMin; y <= generateGem.Bounds.yMax; ++y)
            {
                for (int x = generateGem.Bounds.xMin; x <= generateGem.Bounds.xMax; ++x)
                {
                    var idx = new Vector3Int(x, y, 0);
                    if (gridBoard.contentCell.TryGetValue(idx, out var cell) && cell.CanBeMoved)
                    {
                        var topIdx = idx + Vector3Int.up;
                        var rightIdx = idx + Vector3Int.right;

                        CreatePossibleSwap(idx, topIdx,true);
                        CreatePossibleSwap(idx, rightIdx,false);
                    }
                }
            }

            pickedSwap = Random.Range(0, possibleSwaps.Count);
        }

        public bool DoCheck(Vector3Int startCell, bool createMatch = true)
        {
            if (!gridBoard.contentCell.TryGetValue(startCell, out var centerGem) || centerGem.ContainingGem == null)
                return false;

            if (centerGem.ContainingGem.CurrentMatch != null)
                return false;

            Vector3Int[] offsets = new[]
            {
                Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left
            };

            //First find all the connected BonusGem of the same typeGoal
            List<Vector3Int> gemList = new List<Vector3Int>();
            List<Vector3Int> checkedCells = new();

            Queue<Vector3Int> toCheck = new();
            toCheck.Enqueue(startCell);

            while (toCheck.Count > 0)
            {
                var current = toCheck.Dequeue();

                gemList.Add(current);
                checkedCells.Add(current);

                foreach (var dir in offsets)
                {
                    var nextCell = current + dir;

                    if (checkedCells.Contains(nextCell))
                        continue;

                    if (gridBoard.contentCell.TryGetValue(current + dir, out var content)
                        && content.CanMatch()
                        && content.ContainingGem.CurrentMatch == null
                        && content.ContainingGem.GemType == centerGem.ContainingGem.GemType)
                    {
                        toCheck.Enqueue(nextCell);
                    }
                }
            }

            List<Vector3Int> temporaryShapeMatch = new();
            MatchShape matchedShape = null;
            List<BonusGem> matchedBonusGem = new();

            foreach (var bonusGem in gridBoard.bonusSettings.Bonuses)
            {
                foreach (var shape in bonusGem.Shapes)
                {
                    if (shape.FitIn(gemList, ref temporaryShapeMatch))
                    {
                        if (matchedShape == null || matchedShape.Cells.Count < shape.Cells.Count)
                        {
                            matchedShape = shape;
                            matchedBonusGem.Clear();
                            matchedBonusGem.Add(bonusGem);
                        }
                        else if (matchedShape.Cells.Count == shape.Cells.Count)
                        {
                            matchedBonusGem.Add(bonusGem);
                        }
                    }
                }
            }

            //-- now we build a list of all line of 3+ gems
            List<Vector3Int> lineList = CreateLineList(offsets, gemList);

            //no lines and no bonus match, so there is no match in that.
            if (lineList.Count == 0 && temporaryShapeMatch.Count == 0)
                return false;
           
            TryMatch(startCell, createMatch, temporaryShapeMatch, matchedBonusGem, lineList);
          
            return true;
        }

        public void DoMatchCheck()
        {
            foreach (var cell in cellToMatchCheck)
            {
                DoCheck(cell);
            }

            cellToMatchCheck.Clear();
        }

        private void EmptyCheck()
        {
            if (gridBoard.FreezeMoveLock > 0)
                return;
            var contentCell = gridBoard.contentCell;
            //go over empty cells
            for (int i = 0; i < emptyCells.Count; ++i)
            {
                var emptyCell = emptyCells[i];

                if (!contentCell[emptyCell].IsEmpty())
                {
                    emptyCells.RemoveAt(i);
                    i--;
                    continue;
                }

                var aboveCellIdx = emptyCell + Vector3Int.up;
                bool aboveCellExist = contentCell.TryGetValue(aboveCellIdx, out var aboveCell);

                //if we have a BonusGem above an empty cell, make that BonusGem fall
                if (aboveCellExist && aboveCell.ContainingGem != null && aboveCell.CanFall)
                {
                    var incomingGem = aboveCell.ContainingGem;
                    contentCell[emptyCell].IncomingGem = incomingGem;
                    aboveCell.ContainingGem = null;

                    incomingGem.StartMoveTimer();
                    incomingGem.SpeedMultiplier = 1.0f;

                    //add that empty cell to be ticked so the BonusGem goes down into it
                    newTickingCells.Add(emptyCell);

                    //the above cell is now empty and this cell is not empty anymore
                    emptyCells.Add(aboveCellIdx);
                    emptyCells.Remove(emptyCell);
                }
                else if ((!aboveCellExist || aboveCell.BlockFall) &&
                         contentCell.TryGetValue(aboveCellIdx + Vector3Int.right, out var aboveRightCell) &&
                         aboveRightCell.ContainingGem != null && aboveRightCell.CanFall)
                {
                    var incomingGem = aboveRightCell.ContainingGem;
                    contentCell[emptyCell].IncomingGem = incomingGem;
                    aboveRightCell.ContainingGem = null;

                    incomingGem.StartMoveTimer();
                    incomingGem.SpeedMultiplier = 1.41421356237f;

                    //add that empty cell to be ticked so the BonusGem goes down into it
                    newTickingCells.Add(emptyCell);

                    //the above cell is now empty and this cell is not empty anymore
                    emptyCells.Add(aboveCellIdx + Vector3Int.right);
                    emptyCells.Remove(emptyCell);
                }
                else if ((!aboveCellExist || aboveCell.BlockFall) &&
                         contentCell.TryGetValue(aboveCellIdx + Vector3Int.left, out var aboveLeftCell) &&
                         aboveLeftCell.ContainingGem != null && aboveLeftCell.CanFall)
                {
                    var incomingGem = aboveLeftCell.ContainingGem;
                    contentCell[emptyCell].IncomingGem = incomingGem;
                    aboveLeftCell.ContainingGem = null;

                    incomingGem.StartMoveTimer();
                    incomingGem.SpeedMultiplier = 1.41421356237f;

                    //add that empty cell to be ticked so the BonusGem goes down into it
                    newTickingCells.Add(emptyCell);

                    //the above cell is now empty and this cell is not empty anymore
                    emptyCells.Add(aboveCellIdx + Vector3Int.left);
                    emptyCells.Remove(emptyCell);
                }
                else if (gridBoard.spawnerPoints.Contains(aboveCellIdx))
                {
                    //spawn a new BonusGem
                    gridBoard.ActivateSpawnerAt(emptyCell);
                }
            }

            //empty cell are only handled once, sow e clear the list everytime it been checked.
            //emptyCells.Clear();
        }

        private void CreatePossibleSwap(Vector3Int idx, Vector3Int directions, bool UpAndDown)
        {
            if (gridBoard.contentCell.TryGetValue(directions, out var topCell) && topCell.CanBeMoved)
            {
                //swapHandler the cell
                (gridBoard.contentCell[idx].ContainingGem, gridBoard.contentCell[directions].ContainingGem) = (
                    gridBoard.contentCell[directions].ContainingGem, gridBoard.contentCell[idx].ContainingGem);

                if (DoCheck(directions, false))
                {
                    var direct = UpAndDown == true ? Vector3Int.up : Vector3Int.right;

                    possibleSwaps.Add(new PossibleSwap()
                    {
                        StartPosition = idx,
                        Direction = direct
                    });
                }
               
                if (DoCheck(idx, false))
                {
                    var direct = UpAndDown == true ? Vector3Int.down : Vector3Int.left;

                    possibleSwaps.Add(new PossibleSwap()
                    {
                        StartPosition = directions,
                        Direction = direct
                    });
                }
              
                //swapHandler back
                (gridBoard.contentCell[idx].ContainingGem, gridBoard.contentCell[directions].ContainingGem) = (
                    gridBoard.contentCell[directions].ContainingGem, gridBoard.contentCell[idx].ContainingGem);
            }
        }

        private List<Vector3Int> CreateLineList(Vector3Int[] offsets, List<Vector3Int> gemList)
        {
            List<Vector3Int> lineList = new();

            foreach (var idx in gemList)
            {
                //for each dir (up/down/left/right) if there is no BonusGem in that dir, that mean this could be the start of
                //a matching line, so we check in the opposite direction till we have no more BonusGem
                foreach (var dir in offsets)
                {
                    if (!gemList.Contains(idx + dir))
                    {
                        var currentList = new List<Vector3Int>() { idx };
                        var next = idx - dir;

                        while (gemList.Contains(next))
                        {
                            currentList.Add(next);
                            next -= dir;
                        }

                        if (currentList.Count >= 3)
                        {
                            lineList = currentList;
                        }
                    }
                }
            }

            return lineList;
        }

        private void TryMatch(Vector3Int startCell, bool createMatch, List<Vector3Int> temporaryShapeMatch, List<BonusGem> matchedBonusGem, List<Vector3Int> lineList)
        {
            if (createMatch)
            {
                Match finalMatch = CreateCustomMatch(startCell);
               
                finalMatch.SpawnedBonus = matchedBonusGem.Count == 0 ? null : matchedBonusGem[Random.Range(0, matchedBonusGem.Count)];

                foreach (var cell in lineList)
                {
                    if (gridBoard.matchedCallback.TryGetValue(cell, out var clbk))
                        clbk.Invoke();

                    if (gridBoard.contentCell[cell].CanDelete())
                        finalMatch.AddGem(gridBoard.contentCell[cell].ContainingGem);
                }

                foreach (var cell in temporaryShapeMatch)
                {
                    if (gridBoard.matchedCallback.TryGetValue(cell, out var clbk))
                        clbk.Invoke();

                    if (gridBoard.contentCell[cell].CanDelete())
                        finalMatch.AddGem(gridBoard.contentCell[cell].ContainingGem);
                }
              
                AudioManager.instance.PlayEffect("math_" + 1.ToString());

                //UIHandler.instance.TriggerCharacterAnimation(UIHandler.CharacterAnimation.Match);
            }
        }

        public Match CreateCustomMatch(Vector3Int newCell)
        {
            var newMatch = new Match()
            {
                DeletionTimer = 0.0f,
                MatchingGem = new(),
                OriginPoint = newCell,
                SpawnedBonus = null
            };
           
            tickingMatch.Add(newMatch);
  
            return newMatch;
        }

        private void MatchTicking()
        {
            for (int i = 0; i < tickingMatch.Count; ++i)
            {
                Match match = tickingMatch[i];

                Debug.Assert(match.MatchingGem.Count == match.MatchingGem.Distinct().Count(),
                    "There is duplicate gems in the matching lists");

                const float deletionSpeed = 1.0f / 0.3f;
                match.DeletionTimer += Time.deltaTime * deletionSpeed;
               
                TickMathHandlerGem(match);

                if (match.MatchingGem.Count == 0)
                {
                    gamePlay.AddScore(match.Score);
                    tickingMatch.RemoveAt(i);
                    i--;
                }
            }
        }
        //creait new class tickMath
        private void TickMathHandlerGem(Match match)
        {
            for (int j = 0; j < match.MatchingGem.Count; j++)
            {
                var gemIdx = match.MatchingGem[j];
                var gem = gridBoard.contentCell[gemIdx].ContainingGem;

                if (gem == null)
                {
                    match.MatchingGem.RemoveAt(j);
                    j--;
                    continue;
                }

                StopBouncing(gemIdx, gem);

                //forced deletion doesn't wait for end of timer
                var underGem = gridBoard.contentCell[gemIdx].UnderGem;

                if (match.ForcedDeletion || match.DeletionTimer > 1.0f)
                {
                    Object.Destroy(gridBoard.contentCell[gemIdx].ContainingGem.gameObject);
                    gridBoard.contentCell[gemIdx].ContainingGem = null;

                    if (match.ForcedDeletion && gridBoard.contentCell[gemIdx].Obstacle != null)
                    {
                        gridBoard.contentCell[gemIdx].Obstacle.Clear();
                    }

                    //if (underGem != null)
                    //{
                    //    gridBoard.contentCell[gemIdx].UnderGem.Clear();
                    //    gamePlay.Matched(gridBoard.contentCell[gemIdx].UnderGem);
                    //}

                    //callback are only called when this was a match from swipe and not from bonus or other source 
                    if (!match.ForcedDeletion && gridBoard.cellsCallbacks.TryGetValue(gemIdx, out var clbk))
                    {
                        clbk.Invoke();
                    }

                    match.MatchingGem.RemoveAt(j);
                    j--;

                    match.DeletedCount += 1;
                    //we only spawn coins for non bonus match
                    TryConditionsCoins(match, gem);

                    if (match.SpawnedBonus != null && match.OriginPoint == gemIdx)
                    {
                        gridBoard.NewGemAt(match.OriginPoint, match.SpawnedBonus);
                    }
                    else
                    {
                        emptyCells.Add(gemIdx);
                    }

                    if (gem.CurrentState != Gem.State.Disappearing)
                    {
                        gamePlay.Matched(gem);

                        if (underGem != null)
                        {
                            gridBoard.contentCell[gemIdx].UnderGem.Clear();
                            gamePlay.Matched(underGem);
                        }

                        if (gridBoard.contentCell[gemIdx].Obstacle != null)
                        {
                            gridBoard.contentCell[gemIdx].Obstacle.Clear();
                            gamePlay.Matched(gridBoard.contentCell[gemIdx].Obstacle);
                        }

                        foreach (var matchEffect in gem.effectMatch)
                        {
                            gridBoard.PoolEffect.PlayInstance(matchEffect.type, gridBoard.Grid.GetCellCenterWorld(gem.CurrentIndex));
                        }

                        gem.gameObject.SetActive(false);

                        gem.Destroyed();
                    }
                }
                else if (gem.CurrentState != Gem.State.Disappearing)
                {
                    gamePlay.Matched(gem);

                    if (underGem != null)
                    {
                        gridBoard.contentCell[gemIdx].UnderGem.Clear();
                        gamePlay.Matched(underGem);
                    }

                    foreach (var matchEffect in gem.effectMatch)
                    {
                        gridBoard.PoolEffect.PlayInstance(matchEffect.type, gridBoard.Grid.GetCellCenterWorld(gem.CurrentIndex));
                    }

                    gem.gameObject.SetActive(false);

                    gem.Destroyed();
                }
            }
        }

        private void TryConditionsCoins(Match match, Gem gem)
        {
            if (match.DeletedCount >= 4 && !match.ForcedDeletion)
            {
                gamePlay.AddCoins(1);
                gridBoard.PoolEffect.PlayInstance(EffectType.CoinUp, gem.transform.position);
            }
        }

        private void StopBouncing(Vector3Int gemIdx, Gem gem)
        {
            if (gem.CurrentState == Gem.State.Bouncing)
            {
                //we stop it bouncing as it is getting destroyed
                //We check both current and new ticking cells, as it could be the first frame where it started
                //bouncing so it will be in the new ticking cells NOT in the ticking cell list yet.
                if (tickingCells.Contains(gemIdx)) tickingCells.Remove(gemIdx);
                if (newTickingCells.Contains(gemIdx)) newTickingCells.Remove(gemIdx);

                gem.transform.position = gridBoard.Grid.GetCellCenterWorld(gemIdx);
                gem.transform.localScale = Vector3.one;
                gem.StopBouncing();
            }
        }
    }
}