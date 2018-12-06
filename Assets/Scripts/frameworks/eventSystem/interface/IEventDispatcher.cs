using System;

namespace Sakura
{
    public interface IEventDispatcher
    {
        bool addEventListener(string type, Action<SAEventX> listener, int priority = 0);
        bool hasEventListener(string type);
        bool removeEventListener(string type, Action<SAEventX> listener);
        bool dispatchEvent(SAEventX e);
        bool simpleDispatch(string type, object data = null);
    }

}
