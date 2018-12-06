using System;
using UnityEngine;

namespace Sakura
{
    //一个为了UIActionBase而妥协的类（记得要用接口来实现多继承关系）
    //对应SASkinBase
    public class SAMonoDispatcher : UIActionBase, IEventDispatcher,IDataRenderer
    {
        private EventDispatcher eventDispatcher;
        protected object _data;

        /****************************************Public***************************************/

        public object data
        {
            get { return _data; }
            set
            {
                _data = value;
                doData();
            }
        }

        public static SAEventTrigger Get(GameObject go)
        {
            SAEventTrigger listener = go.GetComponent<SAEventTrigger>();
            if (listener == null)
            {
                listener = go.AddComponent<SAEventTrigger>();
            }

            return listener;
        }

        public bool addEventListener(string type, Action<SAEventX> listener, int priority = 0)
        {
            if (eventDispatcher == null)
            {
                eventDispatcher = new EventDispatcher(this);
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

        /****************************************Protected***************************************/

        protected virtual void doData()
        {

        }

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

        protected virtual void OnDestroy()
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