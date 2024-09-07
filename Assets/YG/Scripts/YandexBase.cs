namespace Assets.YG.Scripts
{
    public abstract class YandexBase
    {
        protected readonly YandexGame yandex;
        protected bool isMessage;
        protected abstract string sourceMessage { get; }

        public YandexBase(YandexGame yandex, bool isMessage = true)
        {
            this.yandex = yandex;
            this.isMessage = isMessage;
        }

        protected void Message(string message)
        {
            if(isMessage) yandex.Message(sourceMessage +" - "+ message);
        }
      
    }
}