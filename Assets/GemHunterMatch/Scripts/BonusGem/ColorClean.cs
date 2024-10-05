using Assets.GameMains.Scripts.AudiosSources;
using Assets.GemHunterMatch.Scripts;
using Assets.GemHunterMatch.Scripts.GenerateGridBoard;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.VFX;

namespace Match3
{
    /// <summary>
    /// Bonus that will delete all gems of a given typeGoal in the board.
    /// </summary>
    public class ColorClean : BonusGem
    {
        public VisualEffect UseEffect;
        public AudioClip TriggerSound;
        
        private Texture2D m_PositionMap;
        
        public override void Awake()
        {
            m_Usable = true;

            PoolService.instance.AddNewInstance(UseEffect, 2);
            m_PositionMap = new Texture2D(64, 1, TextureFormat.RGBAFloat, false);
        }

        public override void Use(Gem swappedGem, bool isBonus = true)
        {
            //this allow to stop recursion on some bonus (like bomb trying to explode themselve again and again)
            //if isBonus is true, this is not a BonusGem on the board so no risk of recursion we can ignore this
            if (!isBonus && m_Used)
                return;

            m_Used = true;
            
            int type = -1;
        
            //first find which typeGoal to delete. If we swapped a BonusGem, this is this BonusGem typeGoal
            if (swappedGem != null)
                type = swappedGem.GemType;

            if (type < 0)
            {//we either swapped with another bonus or we double clicked, so that bonus will clear the BonusGem with the most typeGoal
                Dictionary<int, int> typeCount = new();

                foreach (var (cell, content) in GridBoard.instance.contentCell)
                {
                    if (content.ContainingGem != null)
                    {
                        if (typeCount.ContainsKey(content.ContainingGem.GemType))
                            typeCount[content.ContainingGem.GemType] += 1;
                        else
                            typeCount[content.ContainingGem.GemType] = 1;
                    }
                }

                int highestCount = 0;
                int highestType = 0;
                foreach (var (gemType, count) in typeCount)
                {
                    if (count > highestCount)
                    {
                        highestCount = count;
                        highestType = gemType;
                    }
                }

                type = highestType;
            }

            Color[] infoColor = new Color[64];
            int currentColor = 0;

            //we create a new match in the board, set its typeGoal to force deletion (as this match came from a bonus, not a swapHandler)
            var newMatch = GridBoard.instance.matchHandler.CreateCustomMatch(currentIndex);
            newMatch.ForcedDeletion = true;
            //we grab from the cell and not use "this" because when used as a Bonus Item, the item at this index won't be the BonusGem
            HandleContent(GridBoard.instance.contentCell[currentIndex], newMatch);

            foreach (var (cell, content) in GridBoard.instance.contentCell)
            {
                
                if (content.ContainingGem?.GemType == type)
                {
                    if (content.Obstacle != null)
                    {
                        content.Obstacle.Damage(1);
                    }
                    else if(content.ContainingGem.CurrentMatch == null)
                    {
                        HandleContent(content, newMatch);
                        var pos = content.ContainingGem.transform.position;
                        infoColor[currentColor] = new Color(pos.x, pos.y, pos.z);
                        currentColor++;
                    }
                }
            }
            
            m_PositionMap.filterMode = FilterMode.Point;
            m_PositionMap.wrapMode = TextureWrapMode.Repeat;
            m_PositionMap.SetPixels(infoColor, 0);
            m_PositionMap.Apply();

            VisualEffect vfxInst = PoolService.instance.GetInstance(UseEffect);
            
            vfxInst.Stop();
            vfxInst.SetTexture(Shader.PropertyToID("PosMap"), m_PositionMap);
            vfxInst.SetInt(Shader.PropertyToID("PosCount"), currentColor);

            vfxInst.transform.position = GridBoard.instance.GetCellCenter(currentIndex);
            vfxInst.Play();
            
            AudioManager.instance.PlayEffect(TriggerSound);
        }
    }
}