using Assets.GemHunterMatch.Scripts.UI;

using System.Threading.Tasks;

namespace Assets.GemHunterMatch.Scripts.Loaders
{
    public class LoaderLevelEntryProvider : LocalAssetLoader
    {
        public Task<UILevelEntry> Load()
        {
            return Load<UILevelEntry>("LevelEntry");
        }

        public void Reset()
        {
            UnLoad();
        }

    }
}