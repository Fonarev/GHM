using Assets.GemHunterMatch.Scripts.UI;

using System.Threading.Tasks;

namespace Assets.GemHunterMatch.Scripts.Loaders
{
    public class LoaderLevelEntryProvider : LocalAssetLoader
    {
        public Task<LevelButtonSelectUI> Load()
        {
            return Load<LevelButtonSelectUI>("LevelEntry");
        }

        public void Reset()
        {
            UnLoad(typeof(LevelButtonSelectUI));
        }
        public void ResetAll()
        {
            UnLoadAll();
        }

    }
}