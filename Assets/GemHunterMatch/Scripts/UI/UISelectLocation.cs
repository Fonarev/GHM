using Assets.YG.Scripts;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLocation : MonoBehaviour
    {
        [SerializeField] private RectTransform rootSpawn;
        [SerializeField] private UILocationEntry[] locations;

        private const int offset = 1;

        public void Init()
        {
           for (int i = 0; i < locations.Length; i++) 
           {
                if (YandexGame.Instance.progressData.locations.TryGetValue(i + offset, out var location))
                {
                    locations[i].Init(location);
                }
           }
        }
    }
}