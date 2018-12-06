using System;
using UnityEngine;

namespace Sakura
{
    public class SkinFactory<T> : ClassFactory<T>, ISizeFactory where T : SASkinBase
    {
        protected GameObject _skinPrefab;
        public Vector2 _size=new Vector2();

        public SkinFactory(GameObject prefab)
        {
            if (prefab != null)
            {
                this._skinPrefab = prefab;
                _size = SAUIExtend.GetSize(_skinPrefab);
                this._skinPrefab.SetActive(false);
            }
            else
            {
                Debug.LogError("SkinFactory prefab is null");
            }
        }

        public override object newInstance()
        {
            T instance = (T) base.newInstance();
            if (_skinPrefab != null)
            {
                GameObject go = GameObject.Instantiate(_skinPrefab);
                go.SetActive(true);
                instance.skin = go;
            }

            return instance;
        }

        public virtual int itemHeight
        {
            get { return (int) _size.y; }
        }

        public virtual int itemWidth
        {
            get { return (int) _size.x; }
        }
    }
}