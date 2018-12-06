using System;
using UnityEngine;

namespace Sakura
{
    [HideInInspector]
    public class MonoDispatcher : MonoBehaviour, IEventDispatcher
    {
        [NonSerialized]
        public bool isDebug = false;

        protected object _data;

        private EventDispatcher eventDispatcher;
        private bool _isDisposed = false;
        private bool _isDispoing = false;

        public bool addEventListener(string type, Action<SAEventX> listener, int priority = 0)
        {
            if (eventDispatcher == null)
            {
                eventDispatcher=new EventDispatcher(this);
            }

            return eventDispatcher.addEventListener(type, listener, priority);
        }

        public bool dispatchEvent(SAEventX e)
        {
            if (eventDispatcher == null)
            {
                return false;
            }

            return eventDispatcher.dispatchEvent(e);
        }

        public bool hasEventListener(string type)
        {
            if (eventDispatcher == null)
            {
                return false;
            }

            return eventDispatcher.hasEventListener(type);
        }

        public bool removeEventListener(string type, Action<SAEventX> listener)
        {
            if (eventDispatcher == null)
            {
                return false;
            }

            return eventDispatcher.removeEventListener(type, listener);
        }

        public bool simpleDispatch(string type, object data = null)
        {
            if (eventDispatcher == null)
            {
                return false;
            }

            return eventDispatcher.simpleDispatch(type, data);
        }

        public bool isDisposed
        {
            get { return _isDisposed; }
        }

        public bool isDisposing
        {
            get { return _isDispoing; }
        }

        public void OnDestroy()
        {
            _isDispoing = true;
            doDestroy();
            _isDisposed = true;
        }
        /******************************************************************************/

        protected virtual void OnEnable()
        {
            if (eventDispatcher == null)
            {
                return;
            }

            this.simpleDispatch(SAEventX.ADDED_TO_STAGE);
        }

        protected virtual void OnDisable()
        {
            if (eventDispatcher == null)
            {
                return;
            }

            this.simpleDispatch(SAEventX.REMOVED_FROM_STAGE);
        }

        protected virtual void doDestroy()
        {
            this.simpleDispatch(SAEventX.DESTOTY);
            if (eventDispatcher != null)
            {
                eventDispatcher.Dispose();
                eventDispatcher = null;
            }
        }
    }

}
