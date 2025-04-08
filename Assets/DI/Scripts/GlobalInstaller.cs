using Assets.DailyRewards.Scripts;
using Assets.GameMains.Scripts;

namespace Assets.DI.Scripts
{
    public class GlobalInstaller : Installer
    {
        public override void Installize()
        {
            container.Reg<GlobalMediator>();
            container.Reg<DailyRewardsService>().Perform();
            container.Reg<LoaderScenes>().Perform();
        }
    }
}