using System;
using UnityEngine;

namespace Sakura
{
    public class SAListRenderFactory<T> : ClassFactory<T>, ISizeFactory where T : SAListItemRender
    {
        protected GameObject _skinPrefab;
        public Vector2 _size = new Vector2();

        public SAListRenderFactory(GameObject prefab)
        {
            if (prefab != null)
            {
                this._skinPrefab = prefab;
                _size = SAUIExtend.GetSize(_skinPrefab);
                this._skinPrefab.SetActive(false);
            }
            else
            {
                Debug.LogError("SAListRenderFactory prefab is null");
            }
        }

        public int itemHeight
        {
            get { return (int) _size.y; }
        }

        public int itemWidth
        {
            get { return (int) _size.x; }
        }

        public override object newInstance()
        {
            //            T instance = (T)base.newInstance();
            if (_skinPrefab == null)
            {
                return null;
            }

            GameObject go = GameObject.Instantiate(_skinPrefab);
            go.SetActive(true);
            SAListItemRender render = go.GetComponent<SAListItemRender>();

            return render;
        }
    }
}