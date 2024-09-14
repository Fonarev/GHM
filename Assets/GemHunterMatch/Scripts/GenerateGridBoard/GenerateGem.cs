using Match3;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.GenerateGridBoard
{
    public class GenerateGem
    {
        public BoundsInt Bounds => bounds;
        private BoundsInt bounds;
        private Gem[] existingGems;
        private List<Vector3Int> listOfCells;
        private Dictionary<int, Gem> gemLookup = new();
        private readonly GridBoard gridBoard;
        public static GenerateGem instance;
        public GenerateGem(GridBoard gridBoard)
        {
            this.gridBoard = gridBoard;
            existingGems = gridBoard.existingGems;
            listOfCells = this.gridBoard.contentCell.Keys.ToList();

            foreach (var gem in this.gridBoard.existingGems)
            {
                gemLookup.Add(gem.GemType, gem);
            }
            instance = this;
        }

        //generate a gem in every cell, making sure we don't have any match 
        public void FillBoardGems()
        {
            bounds = new BoundsInt();

            bounds.xMin = listOfCells[0].x;
            bounds.xMax = bounds.xMin;

            bounds.yMin = listOfCells[0].y;
            bounds.yMax = bounds.yMin;

            foreach (var content in listOfCells)
            {
                if (content.x > bounds.xMax)
                    bounds.xMax = content.x;
                else if (content.x < bounds.xMin)
                    bounds.xMin = content.x;

                if (content.y > bounds.yMax)
                    bounds.yMax = content.y;
                else if (content.y < bounds.yMin)
                    bounds.yMin = content.y;
            }

            CheckAllGems();
        }

        public Gem NewGemAt(Vector3Int cell, Gem gemPrefab)
        {
            if (gemPrefab == null)
                gemPrefab = existingGems[Random.Range(0, existingGems.Length)];

            if (gemPrefab.MatchEffectPrefabs.Length != 0)
            {
                foreach (var matchEffectPrefab in gemPrefab.MatchEffectPrefabs)
                {
                    //GameManager.Instance.PoolSystem.AddNewInstance(matchEffectPrefab, 16);
                }
            }

            //New Gem may be called after the board was init (as startup doesn't seem to be reliably called BEFORE init)
            if (gridBoard.contentCell[cell].ContainingGem != null)
            {
                Object.Destroy(GridBoard.instance.contentCell[cell].ContainingGem.gameObject);
            }

            var gem = Object.Instantiate(gemPrefab, gridBoard.grid.GetCellCenterWorld(cell), Quaternion.identity);
            gridBoard.contentCell[cell].ContainingGem = gem;
            gem.Init(cell);

            return gem;
        }

        private void CheckAllGems()
        {
            for (int y = bounds.yMin; y <= bounds.yMax; ++y)
            {
                for (int x = bounds.xMin; x <= bounds.xMax; ++x)
                {
                    Vector3Int idx = new Vector3Int(x, y, 0);

                    if (!gridBoard.contentCell.TryGetValue(idx, out var current) || current.ContainingGem != null)
                        continue;

                    List<int> availableGems = gemLookup.Keys.ToList();

                    int leftGemType = -1;
                    int bottomGemType = -1;
                    int rightGemType = -1;
                    int topGemType = -1;

                    leftGemType = CheckLeftGems(idx, availableGems, leftGemType);

                    bottomGemType = CheckBelowGems(idx, availableGems, leftGemType, bottomGemType);

                    //as we fill left to right and bottom to top, we could only test left and bottom, but as we can have
                    //manually placed gems, we still need to test in the other 2 direction to make sure

                    rightGemType = CheckRightGems(idx, availableGems, leftGemType, bottomGemType, rightGemType);

                    topGemType = CheckUpGems(idx, availableGems, leftGemType, bottomGemType, rightGemType, topGemType);

                    int chosenGem = availableGems[Random.Range(0, availableGems.Count)];
                    NewGemAt(idx, gemLookup[chosenGem]);
                }
            }
        }

        private int CheckLeftGems(Vector3Int idx, List<int> availableGems, int leftGemType)
        {
            if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(-1, 0, 0), out var leftContent) && leftContent.ContainingGem != null)
            {
                leftGemType = leftContent.ContainingGem.GemType;

                if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(-2, 0, 0), out var leftLeftContent) &&
                    leftLeftContent.ContainingGem != null && leftGemType == leftLeftContent.ContainingGem.GemType)
                {
                    //we have two gem of a given type on the left, so we can't ue that type anymore
                    availableGems.Remove(leftGemType);
                }
            }

            return leftGemType;
        }

        private int CheckBelowGems(Vector3Int idx, List<int> availableGems, int leftGemType, int bottomGemType)
        {
            if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(0, -1, 0), out var bottomContent) && bottomContent.ContainingGem != null)
            {
                bottomGemType = bottomContent.ContainingGem.GemType;

                if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(0, -2, 0), out var bottomBottomContent) &&
                    bottomBottomContent.ContainingGem != null && bottomGemType == bottomBottomContent.ContainingGem.GemType)
                {
                    //we have two gem of a given type on the bottom, so we can't ue that type anymore
                    availableGems.Remove(bottomGemType);
                }

                if (leftGemType != -1 && leftGemType == bottomGemType)
                {
                    //if the left and bottom gem are the same type, we need to check if the bottom left gem is ALSO
                    //of the same type, as placing that type here would create a square, which is a valid match
                    if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(-1, -1, 0), out var bottomLeftContent) &&
                        bottomLeftContent.ContainingGem != null && bottomGemType == leftGemType)
                    {
                        //we already have a corner of gem on left, bottom left and bottom position, so remove that type
                        availableGems.Remove(leftGemType);
                    }
                }
            }

            return bottomGemType;
        }
        
        private int CheckRightGems(Vector3Int idx, List<int> availableGems, int leftGemType, int bottomGemType, int rightGemType)
        {
            if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(1, 0, 0), out var rightContent) && rightContent.ContainingGem != null)
            {
                rightGemType = rightContent.ContainingGem.GemType;

                //we have the same type on left and right, so placing that type here would create a 3 line
                if (rightGemType != -1 && leftGemType == rightGemType)
                {
                    availableGems.Remove(rightGemType);
                }

                if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(2, 0, 0), out var rightRightContent) &&
                    rightRightContent.ContainingGem != null && rightGemType == rightRightContent.ContainingGem.GemType)
                {
                    //we have two gem of a given type on the right, so we can't ue that type anymore
                    availableGems.Remove(rightGemType);
                }

                //right and bottom gem are the same, check the bottom right to avoid creating a square
                if (rightGemType != -1 && rightGemType == bottomGemType)
                {
                    if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(1, -1, 0), out var bottomRightContent) &&
                        bottomRightContent.ContainingGem != null && bottomRightContent.ContainingGem.GemType == rightGemType)
                    {
                        availableGems.Remove(rightGemType);
                    }
                }
            }

            return rightGemType;
        }

        private int CheckUpGems(Vector3Int idx, List<int> availableGems, int leftGemType, int bottomGemType, int rightGemType, int topGemType)
        {
            if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(0, 1, 0), out var topContent) && topContent.ContainingGem != null)
            {
                topGemType = topContent.ContainingGem.GemType;

                //we have the same type on top and bottom, so placing that type here would create a 3 line
                if (topGemType != -1 && topGemType == bottomGemType)
                {
                    availableGems.Remove(topGemType);
                }

                if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(0, 1, 0), out var topTopContent) &&
                    topTopContent.ContainingGem != null && topGemType == topTopContent.ContainingGem.GemType)
                {
                    //we have two gem of a given type on the top, so we can't ue that type anymore
                    availableGems.Remove(topGemType);
                }

                //right and top gem are the same, check the top right to avoid creating a square
                if (topGemType != -1 && topGemType == rightGemType)
                {
                    if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(1, 1, 0), out var topRightContent) &&
                        topRightContent.ContainingGem != null && topRightContent.ContainingGem.GemType == topGemType)
                    {
                        availableGems.Remove(topGemType);
                    }
                }

                //left and top gem are the same, check the top left to avoid creating a square
                if (topGemType != -1 && topGemType == leftGemType)
                {
                    if (gridBoard.contentCell.TryGetValue(idx + new Vector3Int(-1, 1, 0), out var topLeftContent) &&
                        topLeftContent.ContainingGem != null && topLeftContent.ContainingGem.GemType == topGemType)
                    {
                        availableGems.Remove(topGemType);
                    }
                }
            }

            return topGemType;
        }

    }
}