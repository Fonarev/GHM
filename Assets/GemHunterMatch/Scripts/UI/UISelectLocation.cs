using Assets.GemHunterMatch.Scripts.Loaders;

using UnityEngine;

namespace Assets.GemHunterMatch.Scripts.UI
{
    public class UISelectLocation : MonoBehaviour
    {
        [SerializeField] private RectTransform rootSpawn;
        [SerializeField] private UILocationEntry[] locations;
       
        private LocalAssetLoader loader;
        public void Init()
        {
           for (int i = 0; i < locations.Length; i++) 
           {
             locations[i].Init(i + 1);
           }
        }
    }
}