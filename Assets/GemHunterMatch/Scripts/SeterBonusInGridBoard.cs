using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using Match3;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    public class SeterBonusInGridBoard
    {
        private readonly GridBoard gridBoard;
        private readonly GamePlay gamePlay;

        private Dictionary<Vector3Int,BonusGem> bonuses;
        private BonusGem bonus;
        private Vector3Int cellKey;

        public SeterBonusInGridBoard(GridBoard gridBoard, GamePlay gamePlay)
        {
            this.gridBoard = gridBoard;
            this.gamePlay = gamePlay;
            bonuses = new();
        }
        public SeterBonusInGridBoard UseBonus()
        {
            gridBoard.contentCell[cellKey].ContainingGem.Use(null, true);
            bonuses.Remove(cellKey);
            return this;
        }
        public SeterBonusInGridBoard UseAllBonus()
        {
            foreach (var bonus in bonuses)
            {
                bonus.Value.Use(null, true);
            }
            bonuses.Clear();
            return this;
        }

        public SeterBonusInGridBoard RandomBonus()
        {
            int randomIndex = Random.Range(0, gamePlay.bonusFiniches.Length);
            bonus = gamePlay.bonusFiniches[randomIndex];
            return this;
        }

        public SeterBonusInGridBoard RandomCell()
        {
            Vector3Int[] listKeys = gridBoard.contentCell.Keys.ToArray();

            int randomIndex = Random.Range(0, gridBoard.contentCell.Count);
            cellKey = listKeys[randomIndex];

            if (gridBoard.contentCell[cellKey].ContainingGem == null)
                RandomCell();

                return this;
        }

        public SeterBonusInGridBoard Instan()
        {
            if (gridBoard.contentCell[cellKey].ContainingGem != null)
                Object.Destroy(gridBoard.contentCell[cellKey].ContainingGem.gameObject);

            BonusGem gem = Object.Instantiate(bonus, gridBoard.GetCellCenter(cellKey), Quaternion.identity);
            gem.Init(cellKey);

            gridBoard.contentCell[cellKey].ContainingGem = gem;
            bonuses[cellKey] = gem;
            return this;
        }
    }
}