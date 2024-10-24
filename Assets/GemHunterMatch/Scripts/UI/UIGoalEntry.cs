using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIGoalEntry : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countDown;
        [SerializeField] private Image icon;
        [SerializeField] private Image executed;

        private int typeGoal;

        public void Init(GemGoal goal)
        {
            icon.sprite = goal.Gem.UISprite;
            countDown.text = goal.Count.ToString();
            typeGoal = goal.Gem.GemType;

            executed.gameObject.SetActive(false);
        }

        public void Init(Goals goal)
        {
            icon.sprite = goal.gem.UISprite;
            countDown.text = goal.count.ToString();
            typeGoal = goal.gem.GemType;

            if(goal.isExecut)
            {
                countDown.gameObject.SetActive(false);
                executed.gameObject.SetActive(true);
            }
           
        }

        public int GetTypeGoal() => typeGoal;

        public void Change(int count, bool isExecut)
        {
            if(count > 0) 
            {
                countDown.text = count.ToString();
            }
            else
            {
                countDown.gameObject.SetActive(false);
                executed.gameObject.SetActive(isExecut);
            }
        }
    }
}