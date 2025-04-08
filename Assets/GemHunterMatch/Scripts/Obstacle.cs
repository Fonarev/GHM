using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using UnityEngine;
using UnityEngine.VFX;

namespace Match3
{
    //Base class for everything filling a gem space and that get notified when a match is made adjacent to it
    public abstract class Obstacle : MonoBehaviour
    {
        [System.Serializable]
        public class LockStateData
        {
            public Sprite Sprite;
            public VisualEffect UndoneVFX;
        }
        
        public LockStateData[] LockState;

        protected SpriteRenderer m_SpriteRenderer;
        protected int m_CurrentState = 0;
        protected Vector3Int m_Cell;
        public Sprite UISprite;
        private bool m_Done = false;
        public int GemType;

        public virtual void Init(Vector3Int cell)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = LockState[0].Sprite;
            m_CurrentState = 0;

            m_Cell = cell;

            GridBoard.AddObstacle(cell, this);
            
            foreach (var state in LockState)
            {
                //GridBoard.instance.PoolEffect.Create.AddNewInstance(state.UndoneVFX, 4);
            }
        }

        public virtual void Clear()
        {
        
        }

        public void Damage(int amount)
        {
            if (ChangeState(m_CurrentState + amount))
            {
                Clear();
            }
        }

        protected bool ChangeState(int newState)
        {
            //if done we return false as we don't want to re-delete it
            if (m_Done)
                return false;
        
            m_CurrentState = newState;
            //play the undone effect of the state before this one
            if(m_CurrentState-1 >= 0)
                //GameManager.instance.PoolSystem.PlayInstanceAt(BlockState[m_CurrentState - 1].UndoneVFX, transform.position);
            
            if (m_CurrentState < LockState.Length)
            {
                m_SpriteRenderer.sprite = LockState[m_CurrentState].Sprite;
                return false;
            }

            m_Done = true;
            return true;
        }
    }
}