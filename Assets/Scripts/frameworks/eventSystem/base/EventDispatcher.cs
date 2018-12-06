using System;
using System.Collections.Generic;
using Assets.Script.Sakura.framework.pool;
using UnityEngine.Experimental.UIElements;

namespace Sakura
{
    public class EventDispatcher : IEventDispatcher, IDisposable
    {
        internal Dictionary<string, Signal> mEventListeners = null;
        internal IEventDispatcher mTarget;

        public EventDispatcher(IEventDispatcher target = null)
        {
            if (target == null)
            {
                target = this;
            }

            this.mTarget = target;
        }

        public bool addEventListener(string type, Action<SAEventX> listener, int priority = 0)
        {
            if (listener == null)
            {
                return false;
            }

            if (mEventListeners == null)
            {
                mEventListeners=new Dictionary<string, Signal>();
            }

            Signal signal = null;
            if (mEventListeners.TryGetValue(type, out signal)==false)
            {
                signal=new Signal();
                mEventListeners.Add(type,signal);
            }

            return signal.add(listener, priority);
        }

        public bool dispatchEvent(SAEventX e)
        {
            if (e == null)
            {
                return false;
            }

            if (mEventListeners == null || mEventListeners.ContainsKey(e.type) == false)
            {
                return false;
            }

            //不太明白下面几行的用意
            IEventDispatcher previousTarget = e.target;
            e.setTarget(mTarget);

            bool b = invokeEvent(e);

            if (previousTarget != null)
            {
                e.setTarget(previousTarget);
            }

            return b;
        }

        public bool hasEventListener(string type)
        {
            if (mEventListeners == null)
            {
                return false;
            }

            Signal signal;
            if (mEventListeners.TryGetValue(type, out signal))
            {
                return signal != null && signal.firstNode != null;
            }

            return false;
        }

        public bool removeEventListener(string type, Action<SAEventX> listener)
        {
            if (mEventListeners != null)
            {
                Signal signal = null;
                if (mEventListeners.TryGetValue(type, out signal) == false)
                {
                    return false;
                }
            }

            return false;
        }

        public bool simpleDispatch(string type, object data = null)
        {
            if (hasEventListener(type) == false)
            {
                return false;
            }
            SAEventX e=SAEventX.FromPool(type,data);
            bool b = dispatchEvent(e);
            SAEventX.ToPool(e);
            return b;
        }

        public virtual void Dispose()
        {
            _clear();
        }

        private void _clear()
        {
            if (mEventListeners == null)
            {
                return;
            }

            foreach (Signal signal in mEventListeners.Values)
            {
                signal._clear();
            }

            mEventListeners = null;
        }

        public bool invokeEvent(SAEventX e)
        {
            if (mEventListeners == null)
            {
                return false;
            }

            Signal signal;
            if (mEventListeners.TryGetValue(e.type, out signal) == false)
            {
                return false;
            }

            SignalNode<SAEventX> t = signal.firstNode;
            if (t == null)
            {
                return false;
            }

            List<Action<SAEventX>> temp = SimpleListPool<Action<SAEventX>>.Get();

            int i = 0;
            while (t!=null)
            {
                temp.Add(t.action);
                t = t.next;
                i++;
            }

            e.setCurrentTarget(e.target);

            Action<SAEventX> listener;
            for (int j = 0; j < i; j++)
            {
                listener = temp[j];
                listener(e);
//                return true;
            }

            SimpleListPool<Action<SAEventX>>.Release(temp);
            return true;
        }
    }

}
