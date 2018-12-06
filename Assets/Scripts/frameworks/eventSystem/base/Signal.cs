using System;
using System.Collections.Generic;

namespace Sakura
{
    public class Signal:QueueHandle<SAEventX>
    {
        public bool add(Action<SAEventX> value, int priority = 0)
        {
            if (mapping == null)
            {
                mapping=new Dictionary<Action<SAEventX>, SignalNode<SAEventX>>();
            }

            SignalNode<SAEventX> t = null;
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

                    return true;
                }
                return false;
            }

            SignalNode<SAEventX> newNode = getSignalNode();

            newNode.action = value;
            newNode.priority = priority;
            mapping[value] = newNode;

            if (dispatching)
            {
                newNode.active = NodeActiveState.TodoAdd;
            }

            if (firstNode == null)
            {
                len = 1;
                lastNode = firstNode = newNode;
                return true;
            }

            SignalNode<SAEventX> findNode = null;
            if (priority > lastNode.priority)
            {
                t = firstNode;
                SignalNode<SAEventX> pre;
                while (t!=null)
                {
                    if (priority > t.priority)
                    {
                        pre = t.pre;
                        newNode.next = t;
                        t.pre = newNode;

                        if (pre != null)
                        {
                            pre.next = newNode;
                            newNode.pre = pre;
                        }
                        else
                        {
                            firstNode = newNode;
                        }

                        findNode = t;
                        break;
                    }

                    t = t.next;
                }
            }

            if (findNode == null)
            {
                lastNode.next = newNode;
                newNode.pre = lastNode;
                lastNode = newNode;
            }

            len++;
            return true;
        }

        public bool remove(Action<SAEventX> value)
        {
            return __removeHandle(value);
        }
    }
}