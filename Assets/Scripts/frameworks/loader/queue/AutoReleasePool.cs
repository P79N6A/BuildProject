using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sakura
{
    public class AutoReleasePool
    {
        public static float TIMEOUT = 10f;
        public static bool enabled = true;

        private static AutoReleasePool instance=new AutoReleasePool();
        private Dictionary<IAutoReleaseRef, float> pool;
        private float timeCount = 0;

        public AutoReleasePool()
        {
            pool = new Dictionary<IAutoReleaseRef, float>();
        }

        public static void add(IAutoReleaseRef value)
        {
            if (!enabled)
            {
                value.__dispose();
                return;
            }
            instance._add(value);
        }

        public static void remove(IAutoReleaseRef value)
        {
            if (!enabled)
            {
                return;
            }
            instance._remove(value);
        }

        private void _add(IAutoReleaseRef value)
        {
            if (pool.ContainsKey(value))
            {
                pool[value] = Time.realtimeSinceStartup;
                return;
            }
            pool.Add(value,Time.realtimeSinceStartup);
            TickManager.Add(tick);
        }

        private void _remove(IAutoReleaseRef value)
        {
            if (pool.ContainsKey(value) == false)
            {
                return;
            }
            pool.Remove(value);
        }

        private void tick(float deltaTime)
        {
            if ((timeCount += deltaTime) < 5)
            {
                return;
            }

            timeCount = 0;
            forceTime(Time.realtimeSinceStartup);
        }

        private void forceTime(float now)
        {
            List<IAutoReleaseRef> clearList=new List<IAutoReleaseRef>();
            foreach (IAutoReleaseRef res in pool.Keys)
            {
                if (now - pool[res] > TIMEOUT)
                {
                    clearList.Add(res);
                }
            }

            int len = clearList.Count;
            if (len > 0)
            {
                foreach (IAutoReleaseRef res in clearList)
                {
                    res.__dispose();
                    pool.Remove(res);
                }
            }

            if (pool.Count == 0)
            {
                TickManager.Remove(tick);
            }
        }
    }
}