using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.WheelOfLuck.Scripts
{
    [RequireComponent(typeof(Button))]
    public class ButtonOpenWheel : MonoBehaviour
    {
        private WheelOfLuckService service;
        private Transform container;
        public TextMeshProUGUI timeState;
        private Button openButton => GetComponent<Button>();
        private void OnDisable()
        {
            service.OnClaimReward -= Service_OnClaimReward;
            service.OnTimeSpan -= Service_OnTimeSpan;
        }
        public void Init(WheelOfLuckService service,Transform container)
        {
            this.service = service;
            this.container = container;
            openButton.onClick.AddListener(OnClick);
            openButton.interactable = false;
            service.OnClaimReward += Service_OnClaimReward;
            service.OnTimeSpan += Service_OnTimeSpan;
            timeState.text = "00:00:00";
        }

        private void Service_OnTimeSpan(System.TimeSpan time)
        {
            timeState.text = $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        private void Service_OnClaimReward(bool claim)
        {
            openButton.interactable = claim;
        }

        private void OnClick()
        {
            service.OpenWheel(container);
        }
    }
}