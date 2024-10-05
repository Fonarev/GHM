
using Assets.GameMains.Scripts;
using Assets.GameMains.Scripts.Bank;

namespace Assets.DI.Scripts
{
    public class GlobalInstaller : Installer
    {
        public override void Installize()
        {
            container.Reg<Wallet>().Perform();
            container.Reg<LoaderScenes>().Perform();
        }
    }
}