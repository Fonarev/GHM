using Assets.GemHunterMatch.Scripts;

using UnityEngine;

namespace Match3
{
    public class TieBlocker : Obstacle
    {
        public override void Init(Vector3Int cell)
        {
            base.Init(cell);
        
            // we also register the cell as a normal "BonusGem" cell so a BonusGem is spawn under the blocker on start.
            GridBoard.RegisterCell(cell);
            GridBoard.ChangeLock(cell, true);
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
            if(ChangeState(m_CurrentState + 1))
            {
                Clear();
            }
        }
    }
}