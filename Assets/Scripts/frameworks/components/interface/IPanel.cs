namespace Sakura
{
    public interface IPanel /*: ISkinable*/
    {
        void show();

        bool isShow
        {
            get;
        }

        void hide(SAEventX e = null);

        //    bool activeInHierarchy { get; }
    }

}
