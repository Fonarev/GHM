using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using System.Collections;

using UnityEngine;
using UnityEngine.VFX;

namespace Assets.GemHunterMatch.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnderGem : MonoBehaviour
    {
        [System.Serializable]
        public class BlockStateData
        {
            public Sprite Sprite;
            public VisualEffect UndoneVFX;
        }
        
        public BlockStateData[] BlockState;

        protected SpriteRenderer m_SpriteRenderer;
        protected int m_CurrentState = 0;
        protected Vector3Int m_Cell;

        private bool m_Done = false;
        public Sprite UISprite;
        public int GemType;

        public virtual void Init(Vector3Int cell)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_SpriteRenderer.sprite = BlockState[0].Sprite;
            m_CurrentState = 0;

            m_Cell = cell;

            GridBoard.AddUnderGem(cell, this);

            foreach (var state in BlockState)
            {
                //GridBoard.Instance.PoolEffect.Create.AddNewInstance(state.UndoneVFX, 4);
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
            if (m_CurrentState - 1 >= 0)
                //GameManager.Instance.PoolSystem.PlayInstanceAt(BlockState[m_CurrentState - 1].UndoneVFX, transform.position);

                if (m_CurrentState < BlockState.Length)
                {
                    m_SpriteRenderer.sprite = BlockState[m_CurrentState].Sprite;
                    return false;
                }

            m_Done = true;
            return true;
        }

    }
}