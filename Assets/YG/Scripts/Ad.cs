namespace Assets.YG.Scripts
{
    public abstract class Ad 
    {
        protected readonly YandexGame yandex;

        public Ad(YandexGame YG) => yandex = YG;

        public abstract void Show(int id = 0);
        public abstract void Open();
        public abstract void Close(string wasShown);
        public abstract void Error();
    }
}