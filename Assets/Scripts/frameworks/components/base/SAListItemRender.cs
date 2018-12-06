using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    /// <summary>
    /// 继承自MonoBehaviour
    /// </summary>
    public class SAListItemRender :MonoDispatcher,IListItemRender,IFindComponent
    {
        protected string _name;
        protected bool _isSelected = false;
        protected int _index;

        public virtual void refresh()
        {
            doData();
        }

        public GameObject skin
        {
            get
            {
                return this.gameObject;
            }
        }

        public new string name
        {
            get
            {
                if (skin != null)
                {
                    _name = skin.name;
                }
                else
                {
                    _name = "_XXX_";
                }
                return _name;
            }
            set { _name = value; }
        }

        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                doSelected(value);
            }
        }

        public int index
        {
            get { return _index; }
            set { _index = value; }
        }

        public Action<string, IListItemRender, object> itemEventHandle { set; get; }

        public object data
        {
            get { return _data; }
            set
            {
                _data = value;
                doData();
            }
        }

        /// <summary>
        /// 复位render，因为render是循环使用的
        /// </summary>
        public virtual void resetRender()
        {

        }

        public virtual void Dispose()
        {

        }

        public Text getText(string name, GameObject parent = null)
        {
            return getComponent<Text>(name, parent);
        }

        public RawImage getRawImage(string name, GameObject parent = null)
        {
            return getComponent<RawImage>(name, parent);
        }

        public Button getButton(string name, GameObject parent = null)
        {
            return getComponent<Button>(name, parent);
        }

        public Image getImage(string name, GameObject parent = null)
        {
            return getComponent<Image>(name, parent);
        }

        public GameObject getGameObject(string name, GameObject go = null)
        {
            if (go == null)
            {
                go = skin;
            }
            Transform transform = go.transform.Find(name);
            if (transform != null)
            {
                return transform.gameObject;
            }

            return null;
        }

        public T getComponent<T>(string path = "", GameObject go = null) where T : Component
        {
            if (go == null)
            {
                go = skin;
            }
            return SAUIExtend.GetComponent<T>(go, path);
        }

        /******************************************Protected******************************************/

        protected virtual void doSelected(bool value)
        {

        }

        protected virtual void doData()
        {

        }

    }
}