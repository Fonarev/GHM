using System.Runtime.InteropServices;

namespace Assets.YG.Scripts
{
    public class FullAd : YandexBase, IAd
    {
        protected override string sourceMessage => "FullAd";

        public FullAd(YandexGame yandex, bool isMessage = true) : base(yandex, isMessage){ }

        [DllImport("__Internal")] private static extern void FullAdShow();

        public void Show(int id = 0)
        {
#if !UNITY_EDITOR
            if (yandex.nowAdsShow) FullAdShow();
            Message("FullAdShow");
#else
            Message("FullAdShow");
#endif
        }

        public void Open()
        {
            yandex.NowFullAd = true;
            Message("OpenFullAd");
        }

        public void Close(string wasShown)
        {
            yandex.NowFullAd = false;

            if (wasShown == "true")
                Message("Closed FullAd");
            else
                Message("The advertisement was not shown.");
        }

        public void Error()
        {
            yandex.NowFullAd = false;
            Message("Error FullAd.");
        }
    }
}
