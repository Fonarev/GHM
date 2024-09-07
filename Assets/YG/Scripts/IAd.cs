namespace Assets.YG.Scripts
{
    public interface IAd
    {
       public void Close(string wasShown);
       public void Error();
       public void Open();
       public void Show(int id = 0);
    }
}