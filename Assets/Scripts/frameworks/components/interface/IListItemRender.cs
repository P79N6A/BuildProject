using System;

namespace Sakura
{
    public interface IListItemRender : IDataRenderer, IEventDispatcher, IDisposable
    {
        bool isSelected { get; set; }

        int index { get; set; }

        void refresh();

        Action<string, IListItemRender, object> itemEventHandle { get; set; }
    }
}