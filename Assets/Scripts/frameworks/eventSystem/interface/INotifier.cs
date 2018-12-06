namespace Sakura
{
    public interface INotifier
    {
        bool simpleDispatch(string type, object data = null);
    }

}
