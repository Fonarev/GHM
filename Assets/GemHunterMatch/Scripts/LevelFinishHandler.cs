using Assets.GameMains.Scripts;
using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using Match3;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    public class LevelFinishHandler 
    {
        private readonly GridBoard gridBoard;
        private readonly GamePlay gamePlay;

        private SeterBonusInGridBoard seterBonus;
        private List<BoardCell> bonusCells;

        public LevelFinishHandler(GridBoard gridBoard,GamePlay gamePlay)
        {
            this.gridBoard = gridBoard;
            this.gamePlay = gamePlay;

            seterBonus = new(gridBoard,gamePlay);
            bonusCells = new();
        }
       
        public IEnumerator ToFinish()
        {
            yield return HandlerCoroutine.StartRoutine(WaitForBoardChanged());

            yield return HandlerCoroutine.StartRoutine(UseAllBonusGem());

            yield return HandlerCoroutine.StartRoutine(SetBonusGems());

            yield return HandlerCoroutine.StartRoutine(UseAllBonusGem());

            yield return HandlerCoroutine.StartRoutine(WaitForBoardChanged());
        }
       
        public IEnumerator WaitForBoardChanged()
        {
            while (gridBoard.BoardChanged)
            {
                yield return gridBoard.BoardChanged;
            }

            yield return new WaitForSeconds(0.1f);
        }

        private IEnumerator SetBonusGems()
        {
            int move = gamePlay.RemainingMove;

            while (gamePlay.RemainingMove > 0)
            {
               

                for (int i = 0; i < move; i++)
                {
                    gamePlay.SubtractMove();

                    seterBonus.RandomBonus().RandomCell().Instan();
                    yield return new WaitForSeconds(0.1f);
                    //yield return HandlerCoroutine.StartRoutine(UseAllBonusGem());
                }

                yield return HandlerCoroutine.StartRoutine(UseAllBonusGem());
                //yield return HandlerCoroutine.StartRoutine(WaitForBoardChanged());

                yield return gamePlay.RemainingMove;
            }
        }

        private IEnumerator FindAllBonusGem()
        {
            bonusCells.Clear();

            foreach (var cell in gridBoard.contentCell.Values)
            {
                if (cell.ContainingGem != null && cell.ContainingGem.GemType < 0)
                    bonusCells.Add(cell);
            }

            yield return null;
        }

        private IEnumerator UseAllBonusGem()
        {
            yield return HandlerCoroutine.StartRoutine(FindAllBonusGem());

            while (bonusCells.Count > 0)
            {
                var bonus = bonusCells[0].ContainingGem;

                if (bonus != null)
                    bonus.Use(null, true);
                else
                    yield return HandlerCoroutine.StartRoutine(FindAllBonusGem());

                yield return new WaitForSeconds(0.1f);

                yield return bonusCells.Count;
            }
        }
    }
}