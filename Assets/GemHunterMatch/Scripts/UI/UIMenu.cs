using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] private UILocationLevels locationLevels;

        public void Initialize()
        {
            locationLevels.Init();
        }
    }
}