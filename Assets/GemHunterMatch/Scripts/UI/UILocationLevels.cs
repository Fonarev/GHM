using Assets.GameMains.Scripts.AudiosSources;
using Assets.GameMains.Scripts.Expansion;

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
            selectLevels.Init();
            selectLocation.onClick.AddListener(OnSelectedLocal);
        }

        private void OnSelectedLocal()
        {
            AudioManager.instance.PlayEffect(EffectClip.click);
            selectLevels.gameObject.SetActive(!selectLevels.gameObject.activeSelf);
        }
    }
}