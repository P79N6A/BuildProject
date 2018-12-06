using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    public class SASkinBase :EventDispatcher, ISkinable, IDataRenderer, IFindComponent
    {
        protected GameObject _skin;
        protected object _data;
        protected string _name;
        protected bool _isActive;
        protected bool _enabled = true;

        //----------------------------------------Public---------------------------------------//

        public virtual void Refresh()
        {
            doData();
        }



        public virtual void SetActive(bool v)
        {
            _isActive = v;
            if (_skin != null && _skin.activeSelf != v)
            {
                _skin.SetActive(v);
            }
        }

        public bool enabled
        {
            set
            {
                _enabled = value;
                doEnabled();
            }
            get { return _enabled; }
        }


        public virtual object data
        {
            get { return _data; }
            set
            {
                _data = value;
                doData();
            }
        }

        public GameObject skin
        {
            get { return _skin; }
            set
            {
                if (_skin != null)
                {
                    unbindComponents();
                }

                _skin = value;

                if (_skin != null)
                {
                    prebindComponents();
                    bindComponents();
                    postbindComponents();
                }
            }
        }

        public string name
        {
            get
            {
                if (!string.IsNullOrEmpty(_name) && _skin != null)
                {
                    _name = _skin.name;
                }
                return _name;
            }
            set { _name = _skin.name; }
        }

        public bool isActive
        {
            get
            {
                if (_skin != null)
                {
                    return _skin.activeSelf;
                }
                return false;
            }
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
                go = _skin;
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
                go = _skin;
            }
            return SAUIExtend.GetComponent<T>(go, path);
        }

        //----------------------------------------Protected---------------------------------------//

        protected virtual void doData()
        {

        }

        protected virtual void postbindComponents()
        {

        }

        protected virtual void bindComponents()
        {
        }

        protected virtual void prebindComponents()
        {
        }

        protected virtual void unbindComponents()
        {
        }


        protected virtual void doEnabled()
        {
        }

        protected virtual void updateView()
        {

        }
    }

}
