using Assets.GameMains.Scripts.AudiosSources;
using Assets.GemHunterMatch.Scripts;

using UnityEngine;

namespace Match3
{
    public class SmallBomb : BonusGem
    {
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
            
            var newMatch = GridBoard.instance.matchHandler.CreateCustomMatch(currentIndex);
            newMatch.ForcedDeletion = true;
            HandleContent(GridBoard.instance.contentCell[currentIndex], newMatch);

            AudioManager.instance.PlayEffect(TriggerSound);

            Vector3Int[] spaces = new[]
            {
                currentIndex + Vector3Int.left,
                currentIndex + Vector3Int.right,
                currentIndex + Vector3Int.up,
                currentIndex + Vector3Int.down
            };
        
            foreach (var idx in spaces)
            {
                if (GridBoard.instance.contentCell.TryGetValue(idx, out var content) &&
                    content.ContainingGem != null)
                {
                    HandleContent(content, newMatch);
                }
            }
        }
    }
}