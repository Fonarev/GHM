using System.Runtime.InteropServices;

namespace Assets.YG.Scripts
{
    public class Languages : YandexBase
    {
        protected override string sourceMessage => "Languages";

        public Languages(YandexGame yandex, bool isMessage = true) : base(yandex, isMessage){ }
    
        [DllImport("__Internal")] private static extern string GetLanguage();

        public string GetLangs()=> GetLanguage();
      
    }
}