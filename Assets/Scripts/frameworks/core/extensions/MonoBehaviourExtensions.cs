using System;
using UnityEngine;

namespace Sakura
{
    public static class MonoBehaviourExtensions
    {
        public static bool addEventListener(this Component self, string type, Action<SAEventX> listener,
            int priority = 0)
        {
            MonoDispatcher dispatcher = self.gameObject.GetOrAddComponent<MonoDispatcher>();
            return dispatcher.addEventListener(type, listener, priority);
        }

        public static bool hasEventListener(this Component self, string type)
        {
            MonoDispatcher dispatcher = self.GetComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.hasEventListener(type);
        }

        public static bool removeEventListener(this Component self, string type, Action<SAEventX> listener)
        {
#if UNITY_EDITOR
            if (self == null) return false;
#endif
            MonoDispatcher dispatcher = self.GetComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.removeEventListener(type, listener);
        }

        public static bool dispatchEvent(this Component self, SAEventX e)
        {
            MonoDispatcher dispatcher = self.GetComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.dispatchEvent(e);
        }

        public static bool simpleDispatch(this Component self, string type, object data = null)
        {
            MonoDispatcher dispatcher = self.GetComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.simpleDispatch(type, data);
        }

        public static void SetActive(this Component self, bool value)
        {
            if (self != null && self.gameObject != null && self.gameObject.activeSelf != value)
            {
                self.gameObject.SetActive(value);
            }
        }
    }
}