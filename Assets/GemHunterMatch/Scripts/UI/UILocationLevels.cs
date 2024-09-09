using UnityEngine;
using UnityEngine.UI;

namespace Assets.GemHunterMatch.Scripts.UI
{
   
    public class UILocationLevels : MonoBehaviour
    {
        [SerializeField] private UISelectLevels selectLevels;
        [SerializeField] private Button selectLocation;
        public void Init()
        {
            selectLocation.onClick.AddListener(OnSelectedLocal);
        }

        private void OnSelectedLocal()
        {
            selectLevels.gameObject.SetActive(!selectLevels.gameObject.activeSelf);
        }
    }
}