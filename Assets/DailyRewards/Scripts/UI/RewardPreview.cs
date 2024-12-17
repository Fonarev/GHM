using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.DailyRewards.Scripts.UI
{
    public class RewardPreview : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountRevard;
        [SerializeField] private TextMeshProUGUI numberDaily;

        private Image CurrentTarget => GetComponent<Image>();

        public void Init(int amount, int number, bool target)
        {
            amountRevard.text = amount.ToString();
            numberDaily.text = "Daily " + number.ToString();

            CurrentTarget.color = target ? Color.green : Color.white;
        }
    }
}