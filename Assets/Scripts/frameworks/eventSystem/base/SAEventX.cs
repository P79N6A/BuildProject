using System.Collections.Generic;

namespace Sakura
{
    public class SAEventX
    {
        static private Stack<SAEventX> sEventPool = new Stack<SAEventX>();

        public const string START = "start";

        public const string LOCK = "lock";
        public const string UNLOCK = "unLock";

        public const string READY = "ready";

        public const string OPEN = "open";
        public const string CLOSE = "close";
        public const string PAUSE = "pause";
        public const string STOP = "stop";
        public const string PLAY = "play";

        public const string EXIT = "exit";
        public const string ENTER = "enter";

        public const string UPDATE = "update";
        public const string ENTER_FRAME = "enterFrame";

        public const string ADDED = "added";
        public const string ADDED_TO_STAGE = "addedToStage";

        public const string REMOVED = "removed";
        public const string REMOVED_FROM_STAGE = "removedFromStage";

        public const string TRIGGERED = "triggered";

        public const string FLATTEN = "flatten";
        public const string RESIZE = "resize";

        public const string REPAINT = "Repaint";

        public const string PROGRESS = "progress";
        public const string CHANGE = "change";
        public const string COMPLETE = "complete";
        public const string CANCEL = "cancel";

        public const string SUCCESS = "success";
        public const string FAILED = "failed";

        public const string SCROLL = "scroll";
        public const string SELECT = "select";

        public const string DESTOTY = "destory";
        public const string DISPOSE = "dispose";
        public const string DATA = "data";

        public const string ERROR = "error";

        public const string TIMEOUT = "timeout";

        public const string CONNECTION = "connection";

        public const string ITEM_CLICK = "itemClick";
        public const string CLICK = "click";

        public const string FOCUS_IN = "focus_in";
        public const string FOCUS_OUT = "focus_out";

        public const string TOUCH_BEGAN = "touchBegan";
        public const string TOUCH_END = "touchEnd";
        public const string TOUCH_MOVE = "touchMove";

        public const string FIRE = "fire";
        public const string RELOAD = "reload";
        public const string RESTART = "restart";

        public const string RENDER = "render";
        public const string PING = "ping";

        public const string RENDERABLE_CHANGE = "renderable_change";

        public const string MEDIATOR_SHOW = "mediatorShow";
        public const string MEDIATOR_HIDE = "mediatorHide";
        public const string MEDIATOR_READY = "mediatorReady";
        public const string PROXY_READY = "proxyReady";

        public const string ROOT_CREATED = "rootCreated";
        public const string SET_SKIN = "setSkin";
        public const string STATE_CHANGE = "stateChange";
        public static string CLEAR_CACHE = "clearCache";
        public static string DEPEND_READY = "dependReady";
        public static string CLEAR = "clear";


        private string _type;
        private object _data;
        private IEventDispatcher _target;
        private IEventDispatcher mCurrentTarget;

        public SAEventX(string type, object data = null)
        {
            this.type = type;
            this.data = data;
        }

        public SAEventX(string type)
        {
            this.type = type;
        }

        public IEventDispatcher target
        {
            set { _target = value; }
            get { return _target; }
        }

        public string type
        {
            set { _type = value; }
            get { return _type; }
        }

        public object data
        {
            set { _data = value; }
            get { return _data; }
        }

        public SAEventX clone()
        {
            return new SAEventX(_type, _data);
        }

        internal void setTarget(IEventDispatcher value)
        {
            this.target = value;
        }

        internal void setCurrentTarget(IEventDispatcher value)
        {
            mCurrentTarget = value;
        }

        internal SAEventX reset(string type, object data = null)
        {
            this.type = type;
            this.data = data;
            this.target = null;
            return this;
        }

        public static SAEventX FromPool(string type, object data = null)
        {
            SAEventX e;
            if (sEventPool.Count > 0)
            {
                e = sEventPool.Pop();
                e.reset(type, data);
                return e;
            }
            else
            {
                e = new SAEventX(type, data);
                sEventPool.Push(e);
                return e;
            }
        }

        public static void ToPool(SAEventX e)
        {
            if (sEventPool.Count < 100)
            {
                e.data = e.target = null;
                sEventPool.Push(e);
            }
        }
    }
}