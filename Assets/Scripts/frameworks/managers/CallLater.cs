using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sakura
{
    public class CallLater:QueueHandle<float>
    {
        private static CallLater instance=new CallLater();

        private static Dictionary<string,Action<float>> actionMap=new Dictionary<string, Action<float>>();

        /// <summary>
        /// 添加延迟调用函数
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="delayTime">Time.deltaTime: returns different floats each update, mostly around 0.016f - 0.018f. </param>
        /// <param name="key">替换掉key相同的 已有的handler</param>
        public static void Add(Action<float> handler, float delayTime = 0.016f, string key = "")
        {
            if (delayTime <= 0)
            {
                handler(0);

                return;
            }

            if (delayTime < 0.016f)
            {
                delayTime = 0.016f;
            }

            if (string.IsNullOrEmpty(key) == false)
            {
                Action<float> oldHanler;
                if (actionMap.TryGetValue(key, out oldHanler))
                {
                    Remove(oldHanler);
                    actionMap[key] = handler;
                }
            }

            instance.add(delayTime, handler);
        }

        public static bool Has(Action<float> handler)
        {
            return instance.hasHandle(handler);
        }

        public static void Remove(Action<float> oldHanler,string key="")
        {
            instance.__removeHandle(oldHanler);
            if (string.IsNullOrEmpty(key) == false)
            {
                actionMap.Remove(key);
            }
        }

        private void add(float delayTime, Action<float> handler)
        {
            __addHandle(handler, Time.time + delayTime, true);
            if (len > 0)
            {
                TickManager.Add(render);
            }
            else if (firstNode != null)
            {
                TickManager.Add(render);
                Debug.LogError("callLater has a bug+"+len);
            }
        }

        private void render(float deltaTime)
        {
            if (len > 0)
            {
                dispatching = true;
                SignalNode<float> t = firstNode;

                List<SignalNode<float>> temp = getSignalNodeList();
                float now = Time.time;
                while (t!=null)
                {
                    if (t.active == NodeActiveState.Runing)
                    {
                        if (now > t.data)
                        {
                            Remove(t.action);
                            t.action(t.data);
                        }
                    }
                    temp.Add(t);
                    t = t.next;
                }
                dispatching = false;
                int l = temp.Count;
                for (int i = 0; i < l; i++)
                {
                    SignalNode<float> item = temp[i];
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
            else
            {
                TickManager.Remove(render);
            }
        }
    }
}