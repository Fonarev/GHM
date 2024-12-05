using Match3;

using System;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts
{
    public class Goals 
    {
        public Gem gem;
        public Obstacle obstacle;
        public UnderGem underGem;
        public int count;
        public bool isExecut;
        public Sprite GetSpriteGoal()
        {
            if (gem != null)
            {
                return gem.UISprite;
            }
            else
            {
                if (obstacle != null)
                {
                    return obstacle.UISprite;
                }
                else
                {
                    if (underGem != null)
                        return underGem.UISprite;
                }
            }

            return null;
        }

        public int GetCurrentType()
        {

            if (obstacle != null) return obstacle.GemType;

            if (underGem != null) return underGem.GemType;


            return gem.GemType;
        }
    }
   
}