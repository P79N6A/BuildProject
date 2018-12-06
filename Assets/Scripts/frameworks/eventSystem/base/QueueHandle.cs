using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakura
{
    public class QueueHandle<T>
    {
        private static Stack<SignalNode<T>> nodePool = new Stack<SignalNode<T>>();
        private static Stack<List<SignalNode<T>>> signalNodeListPool = new Stack<List<SignalNode<T>>>();

        private static int MAX = 1000;

        internal SignalNode<T> firstNode;
        internal SignalNode<T> lastNode;
        internal bool dispatching = false;

        protected Dictionary<Action<T>, SignalNode<T>> mapping;
        protected int len = 0;

        public QueueHandle()
        {

        }

        public int length
        {
            get { return len; }
        }

        public void dispatch(T e)
        {
            if (len > 0)
            {
                dispatching = true;
                SignalNode<T> t = firstNode;

                List<SignalNode<T>> temp = getSignalNodeList();

                while (t != null)
                {
                    if (t.active == NodeActiveState.Runing)
                    {
                        t.action(e);
                    }
                    temp.Add(t);
                    t = t.next;
                }

                dispatching = false;

                int l = temp.Count;
                for (int i = 0; i < l; i++)
                {
                    SignalNode<T> item = temp[i];

                    if (item.active == NodeActiveState.TodoDelete)
                    {
                        _remove(item, item.action);
                    }
                    else if (item.active == NodeActiveState.TodoAdd)
                    {
                        item.active = NodeActiveState.Runing;
                    }
                }

                recycle(temp);
            }
        }

        public bool __addHandle(Action<T> value, T data, bool forceData = false)
        {
            if (mapping == null)
            {
                mapping=new Dictionary<Action<T>, SignalNode<T>>();
            }

            SignalNode<T> t = null;
            if (mapping.TryGetValue(value, out t))
            {
                if (t.active == NodeActiveState.TodoDelete)
                {
                    if (dispatching)
                    {
                        t.active = NodeActiveState.TodoAdd;
                    }
                    else
                    {
                        t.active = NodeActiveState.Runing;
                    }
                    t.data = data;
                    return true;
                }
                if (forceData)
                {
                    t.data = data;
                }
                return false;
            }

            t = getSignalNode();
            t.action = value;
            t.data = data;
            mapping.Add(value,t);

            if (dispatching)
            {
                t.active = NodeActiveState.TodoAdd;
            }
            else
            {
                firstNode = lastNode = t;
            }

            len++;
            return true;
        }

        public bool __removeHandle(Action<T> value)
        {
            if (lastNode == null || mapping == null)
            {
                return false;
            }

            SignalNode<T> t = null;
            if (mapping.TryGetValue(value, out t) == false)
            {
                return false;
            }

            if (dispatching)
            {
                t.active = NodeActiveState.TodoDelete;
                return true;
            }

            return _remove(t, value);
        }

        public bool hasHandle(Action<T> value)
        {
            if (mapping == null)
            {
                return false;
            }

            return mapping.ContainsKey(value);
        }

        public void _clear()
        {
            if (firstNode == null)
            {
                return;
            }

            SignalNode<T> t = firstNode;
            SignalNode<T> n;

            while (t != null)
            {
                t.action = null;
                if (nodePool.Count > MAX)
                {
                    break;
                }

                n = t.next;
                t.action = null;
                t.pre = t.next = null;
                nodePool.Push(t);

                t = n;
            }

            mapping = null;
            firstNode = lastNode = null;
            len = 0;
        }

        /***************************************************************************/

        protected static List<SignalNode<T>> getSignalNodeList()
        {
            if (signalNodeListPool.Count > 0)
            {
                List<SignalNode<T>> temp = signalNodeListPool.Pop();
                temp.Clear();
                return temp;
            }
            return new List<SignalNode<T>>();
        }

        protected static void recycle(List<SignalNode<T>> node)
        {
            if (signalNodeListPool.Count < 300)
            {
                signalNodeListPool.Push(node);
            }
        }

        protected SignalNode<T> getSignalNode()
        {
            SignalNode<T> t;
            if (nodePool.Count > 0)
            {
                t = nodePool.Pop();
                t.active = NodeActiveState.Runing;
            }
            else
            {
                t = new SignalNode<T>();
            }

            return t;
        }

        protected bool _remove(SignalNode<T> t, Action<T> value)
        {
            if (t == null)
            {
                Debug.Log("queueHandle error nil");
            }

            SignalNode<T> pre = t.pre;
            SignalNode<T> next = t.next;
            if (pre != null)
            {
                pre.next = next;
            }
            else
            {
                firstNode = next;
            }

            if (next != null)
            {
                next.pre = pre;
            }
            else
            {
                lastNode = pre;
            }

            t.active = NodeActiveState.TodoDelete;
            mapping.Remove(value);

            if (nodePool.Count < MAX)
            {
                t.action = null;
                t.pre = t.next = null;
                nodePool.Push(t);
            }

            len--;

            if (len < 0)
            {
                Debug.LogError("QueueHandle lenError:" + len);
            }

            return true;
        }
        /***************************************************************************/




    }
}