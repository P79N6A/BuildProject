namespace Sakura
{
    public interface IDataRenderer : IDataRenderer<object>
    {

    }

    public interface IDataRenderer<T>
    {
        T data
        {
            get;
            set;
        }
    }

}
