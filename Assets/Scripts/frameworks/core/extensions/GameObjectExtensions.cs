using System;
using System.Collections;
using UnityEngine;

namespace Sakura
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            if (component == null)
            {
                component = self.AddComponent<T>();
            }

            return component;
        }

        public static bool addEventListener(this GameObject self, string type, Action<SAEventX> listener,
            int priority = 0)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            return dispatcher.addEventListener(type, listener, priority);
        }

        public static bool hasEventListener(this GameObject self, string type)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.hasEventListener(type);
        }

        public static bool removeEventListener(this GameObject self, string type, Action<SAEventX> listener)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.removeEventListener(type, listener);
        }

        public static bool dispatchEvent(this GameObject self, SAEventX e)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.dispatchEvent(e);
        }

        public static bool simpleDispatch(this GameObject self, string type, object data = null)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                return false;
            }

            return dispatcher.simpleDispatch(type, data);
        }

        public static Coroutine StartCoroutine(this GameObject self, IEnumerator routine)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                dispatcher = self.AddComponent<MonoDispatcher>();
            }

            return dispatcher.StartCoroutine(routine);
        }

        public static void stopCoroutine(this GameObject self, Coroutine routine)
        {
            MonoDispatcher dispatcher = self.GetOrAddComponent<MonoDispatcher>();
            if (dispatcher == null)
            {
                dispatcher = self.AddComponent<MonoDispatcher>();
            }
            dispatcher.StopCoroutine(routine);
        }

        public static Vector2 GetUISize(this GameObject go)
        {
            RectTransform rectTransform = go.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                return rectTransform.sizeDelta;
            }

            return go.transform.localScale;
        }
    }
}