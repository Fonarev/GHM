using Assets.GameMains.Scripts.AudiosSources;
using Assets.GemHunterMatch.Scripts;
using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using UnityEngine;

namespace Match3
{
    /// <summary>
    /// Bonus gems that will move across time horizontally/vertically and delete all gems in that direction
    /// </summary>
    public class LineRocket : BonusGem
    {
        public GameObject VisualPrefab;
        public bool Vertical;

        public AudioClip TriggerSound;
        
        public override void Awake()
        {
            m_Usable = true;
        }

        public override void Use(Gem swappedGem, bool isBonus = true)
        {
            //this allow to stop recursion on some bonus (like bomb trying to explode themselve again and again)
            //if isBonus is true, this is not a BonusGem on the board so no risk of recursion we can ignore this
            if (!isBonus && m_Used)
                return;

            m_Used = true;
            
            var dir = Vertical ? Vector3Int.up : Vector3Int.right;

            AudioManager.instance.PlayEffect(TriggerSound);
            
            //delete itself first.
            var newMatch = GridBoard.instance.matchHandler.CreateCustomMatch(currentIndex);
            HandleContent(GridBoard.instance.contentCell[currentIndex], newMatch);

            //if there is a cell on a side, we add a new board action that will go in that direction.
            if (GridBoard.instance.contentCell.ContainsKey(currentIndex + dir))
            {
                GridBoard.instance.AddNewBoardAction(new RocketAction(currentIndex, dir, VisualPrefab, 0));
            }

            if (GridBoard.instance.contentCell.ContainsKey(currentIndex - dir))
            {
                GridBoard.instance.AddNewBoardAction(new RocketAction(currentIndex, -dir, VisualPrefab,
                    Vertical ? 2 : 1));
            }
        }
    }
    
    /// <summary>
    /// RocketAction is a board action that will delete BonusGem along a direction at a given speed until it can no longer go
    /// forward
    /// </summary>
    class RocketAction : IBoardAction
    {
        protected Vector3Int m_CurrentCell;
        protected Vector3Int m_Direction;

        protected GameObject m_Visual;

        private const float MoveSpeed = 10.0f;
        
        public RocketAction(Vector3Int startCell, Vector3Int direction, GameObject visualPrefab, int flip)
        {
            m_CurrentCell = startCell;
            m_Direction = direction;
            
            GridBoard.instance.LockMovement();

            m_Visual = GameObject.Instantiate(visualPrefab, 
                GridBoard.instance.GetCellCenter(m_CurrentCell), 
                Quaternion.identity);

            switch (flip)
            {
                case 1:
                    m_Visual.transform.localScale = new Vector3(-1, 1, 1);
                    break;
                case 2:
                    m_Visual.transform.localScale = new Vector3(1, -1, 1);
                    break;
            }
        }
        
        //Called by the board on all its board actions.
        public bool Tick()
        {
            m_Visual.transform.position += (Vector3)(m_Direction) * (Time.deltaTime * MoveSpeed);

            Vector3 cell = GridBoard.instance.WorldToCell(m_Visual.transform.position);

            while (m_CurrentCell != cell)
            {
                m_CurrentCell += m_Direction;

                if (GridBoard.instance.contentCell.TryGetValue(m_CurrentCell, out var content) && content.ContainingGem != null)
                {
                    if (content.Obstacle != null)
                    {
                        content.Obstacle.Damage(1);
                    }
                    else if (content.ContainingGem.Usable)
                    {
                        content.ContainingGem.Use(null);
                    }
                    else if (!content.ContainingGem.Damage(1))
                    {
                        GridBoard.instance.DestroyGem(m_CurrentCell, true);
                    }
                }

                if (!GridBoard.instance.contentCell.ContainsKey(m_CurrentCell + m_Direction))
                {
                    GameObject.Destroy(m_Visual);
                    //if we don't have a cell after that one, we reached the end, return false to finish that BoardAction
                    GridBoard.instance.UnlockMovement();
                    return false;
                }
            }
            
            return true;
        }
    }
}
