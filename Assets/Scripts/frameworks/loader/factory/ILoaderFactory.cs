namespace Sakura
{
    public interface ILoaderFactory
    {
        RFLoader getLoader(AssetResource resource);
    }
}