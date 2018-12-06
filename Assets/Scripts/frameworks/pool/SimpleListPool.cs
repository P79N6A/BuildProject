using System.Collections;
using System.Collections.Generic;

namespace Assets.Script.Sakura.framework.pool
{
    public static class SimpleListPool<T>
    {
        private static Stack<List<T>> Pools=new Stack<List<T>>();

        public static List<T> Get()
        {
            if (Pools.Count > 0)
            {
                return Pools.Pop();
            }
            return new List<T>();
        }

        public static void Release(List<T> value)
        {
            if (Pools.Count < 100)
            {
                value.Clear();
                Pools.Push(value);
            }
        }
    }
}