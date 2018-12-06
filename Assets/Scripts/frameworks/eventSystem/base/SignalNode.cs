using System;

namespace Sakura
{
    public class SignalNode<T>
    {
        public SignalNode<T> next;
        public SignalNode<T> pre;
        public Action<T> action;

        public T data;

        internal NodeActiveState active = NodeActiveState.Runing;

        public int priority = 0;
    }

    public enum NodeActiveState
    {
        Runing,
        TodoAdd,
        TodoDelete
    }
}