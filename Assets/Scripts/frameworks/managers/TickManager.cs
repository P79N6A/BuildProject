using System;
using System.Net.NetworkInformation;

namespace Sakura
{
    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate,
        LaterCameraUpdate,
        DrawGizmos
    }

    public class TickManager
    {
        private static QueueHandle<float> updateQueue;
        private static QueueHandle<float> fixedUpdateQueue;
        private static QueueHandle<float> lateUpdateQueue;
        private static QueueHandle<float> laterCameraUpdateQueue;
        private static QueueHandle<float> drawGizmosQueue;
#if UNITY_EDITOR
        private static float _preTime = 0f;
#endif
        private static float _time = 0f;
        private static float _deltaTime = 0f;

        static TickManager()
        {
            updateQueue = new QueueHandle<float>();
            fixedUpdateQueue=new QueueHandle<float>();
            lateUpdateQueue=new QueueHandle<float>();
            laterCameraUpdateQueue=new QueueHandle<float>();
            drawGizmosQueue=new QueueHandle<float>();

#if UNITY_EDITOR
            _preTime = (float) UnityEditor.EditorApplication.timeSinceStartup;
            UnityEditor.EditorApplication.update -= editorUpdate;
            UnityEditor.EditorApplication.update += editorUpdate;
#endif
        }

        public static bool Add(Action<float> handle, UpdateType type = UpdateType.Update)
        {
            if (handle == null)
            {
                return false;
            }

            switch (type)
            {
                case UpdateType.Update:
                    return updateQueue.__addHandle(handle, 0);
                case UpdateType.FixedUpdate:
                    return fixedUpdateQueue.__addHandle(handle, 0);
                case UpdateType.LateUpdate:
                    return lateUpdateQueue.__addHandle(handle, 0);
                case UpdateType.LaterCameraUpdate:
                    return laterCameraUpdateQueue.__addHandle(handle, 0);
                case UpdateType.DrawGizmos:
                    return drawGizmosQueue.__addHandle(handle, 0);
                default:
                    return updateQueue.__addHandle(handle, 0);
            }
        }

        public static bool Remove(Action<float> handle)
        {
            bool b = updateQueue.__removeHandle(handle);
            if (b == false)
            {
                b = fixedUpdateQueue.__removeHandle(handle);
                if (b == false)
                {
                    b = lateUpdateQueue.__removeHandle(handle);
                    if (b == false)
                    {
                        b = laterCameraUpdateQueue.__removeHandle(handle);
                        if (b == false)
                        {
                            b = drawGizmosQueue.__removeHandle(handle);
                        }
                    }
                }
            }

            return b;
        }

#if UNITY_EDITOR
        private static void editorUpdate()
        {
            _time = (float) UnityEditor.EditorApplication.timeSinceStartup;
            _deltaTime = _time - _preTime;
            _preTime = _time;
        }
#endif
    }
}