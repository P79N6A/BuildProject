using System;
using UnityEngine.EventSystems;

namespace Sakura
{
    public class SAEventTrigger : EventTrigger, IEventDispatcher
    {
        private EventDispatcher eventDispatcher;
        protected bool isDown = false;
        public bool mouseEnterEnabled = false;
        public object data;

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.simpleDispatch(SAMouseEvent.CLICK, eventData);
            base.OnPointerClick(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
            if (mouseEnterEnabled)
            {
//                TickManager.Add(tick);
            }

            this.simpleDispatch(SAMouseEvent.MOUSE_DOWN, eventData);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
            if (mouseEnterEnabled)
            {
//                TickManager.Add(tick);
            }

            this.simpleDispatch(SAMouseEvent.MOUSE_UP, eventData);
            base.OnPointerUp(eventData);
        }

        public bool addEventListener(string type, Action<SAEventX> listener, int priority = 0)
        {
            if (eventDispatcher == null)
            {
                eventDispatcher = new EventDispatcher(this);
            }
            return eventDispatcher.addEventListener(type, listener, priority);
        }

        public bool dispatchEvent(SAEventX e)
        {
            if (eventDispatcher == null)
            {
                eventDispatcher=new EventDispatcher(this);
            }

            return eventDispatcher.dispatchEvent(e);
        }

        public bool hasEventListener(string type)
        {
            if (eventDispatcher == null)
            {
                return false;
            }
            return eventDispatcher.hasEventListener(type);
        }

        public bool removeEventListener(string type, Action<SAEventX> listener)
        {
            if (eventDispatcher == null)
            {
                return false;
            }

            return eventDispatcher.removeEventListener(type, listener);
        }

        public bool simpleDispatch(string type, object data = null)
        {
            if (eventDispatcher == null)
            {
                return false;
            }

            return eventDispatcher.simpleDispatch(type, data);
        }

        protected virtual void OnDestroy()
        {
            if (eventDispatcher != null)
            {
                eventDispatcher.Dispose();
                eventDispatcher = null;
            }
        }
    }
}