
using Assets.GameMains.Scripts;

namespace Assets.DI.Scripts
{
    public class GlobalInstaller : Installer
    {
        public override void Installize()
        {
            container.Reg<LoaderScenes>().Perform();
        }
    }
}