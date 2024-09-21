using Assets.AssetLoaders;

using Match3;

using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIGamePlay : MonoBehaviour
    {
        LevelConfig level;
        public RectTransform rootGoals;
        private Dictionary<int,UIGoalEntry> entries = new();
        public TextMeshProUGUI moveCounter;

        public void Initialize(GamePlay gamePlay, LevelConfig level)
        {
            this.level = level;
            gamePlay.OnGoalChanged += GoalChange;
            gamePlay.OnMoveHappened += MoveHappen;
            moveCounter.text = level.MaxMove.ToString();

            foreach (var goal in level.Goals)
            {
                StartCoroutine(LoaderAsset.InstantiateAsset<UIGoalEntry>("GoalEntry", rootGoals, op =>
                {
                    op.spriteGem.sprite = goal.Gem.UISprite;
                    op.countDown.text = goal.Count.ToString();
                    op.type = goal.Gem.GemType;
                    entries[op.type] = op;
                }));
               
            }
        }

        private void MoveHappen(int move)
        {
            moveCounter.text = move.ToString();
        }

        public void AddMatchEffect(Gem gem)
        {
            var elem = new Image();

            //m_Document.rootVisualElement.Add(elem);

            elem.style.position = Position.Absolute;

            elem.sprite = gem.UISprite;

            var worldPosition = gem.transform.position;
            //var pos = RuntimePanelUtils.CameraTransformWorldToPanel(m_Document.rootVisualElement.panel,
            //    worldPosition,
            //    mainCamera);

            var label = entries[gem.GemType];
            //var target = (Vector2)label.LocalToWorld(label.transform.position);

            //elem.style.left = pos.x;
            //elem.style.top = pos.y;
            elem.style.translate = new Translate(Length.Percent(-50), Length.Percent(-50));

            //m_CurrentGemAnimations.Add(new UIAnimationEntry()
            //{
            //    Time = 0.0f,
            //    WorldPosition = worldPosition,
            //    StartPosition = pos,
            //    StartToEnd = target - pos,
            //    UIElement = elem,
            //    Curve = null
            //});
        }

        private void GoalChange(int type, int count)
        {
            if(entries.TryGetValue(type, out UIGoalEntry entry)) 
            {
                entry.countDown.text = count.ToString();
            }
            else
            {
                Debug.Log($"No type {type} UIGoalEntry");
            }
        }
    }
}