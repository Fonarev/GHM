using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using Match3;

using System.Collections;

using UnityEngine;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts
{
    public class BubleCube : UnderGem
    {
        public override void Init(Vector3Int cell)
        {
            base.Init(cell);

            // we also register the cell as a normal "gem" cell so a gem is spawn under the blocker on start.
            GridBoard.RegisterCell(cell);
            GridBoard.ChangeLock(cell, false);
            GridBoard.RegisterMatchedCallback(cell, CellMatch);
        }

        public override void Clear()
        {
            GridBoard.UnregisterMatchedCallback(m_Cell, CellMatch);
            GridBoard.ChangeLock(m_Cell, false);
            Destroy(gameObject);
        }

        void CellMatch()
        {
            if (ChangeState(m_CurrentState + 1))
            {
                Clear();
            }
        }

    }
}