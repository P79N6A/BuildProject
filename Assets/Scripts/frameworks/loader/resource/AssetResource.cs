using System;
using System.Collections.Generic;
using Sakura;
using UnityEngine;

namespace Sakura
{
    public class AssetResource : EventDispatcher, IAutoReleaseRef
    {
        
        public static bool THROW_ERROR = true;
        public static ILoaderFactory loaderFactory;

        /// <summary>
        /// 强制超时时间（默认-1无超时策略）
        /// </summary>
        public int timeout = -1;

        /// <summary>
        /// 想要提交服务器的数据
        /// </summary>
        public string postData;

        public LoaderXDataType parserType;
        /// <summary>
        /// 是否强制从远程加载
        /// </summary>
        public bool isForceRemote = false;

        /// <summary>
        /// 是否默认初始化资源
        /// </summary>
        public bool isAutoMainAsset = false;

        public int maxPoolSize = 10;

        /// <summary>
        /// 资源状态
        /// </summary>
        protected AssetState _status = AssetState.NONE;
        protected static Dictionary<Hash128, Hash128Link> allManifestMapping = new Dictionary<Hash128, Hash128Link>();
        protected Stack<PoolItem> pool;
        protected string _url;
        protected object _data;

        private int _reference = 0;

        public virtual void load(uint retryCount = 0, bool progress = false, int priority = 0)
        {
            if (_status == AssetState.LOADING)
            {
                return;
            }

            if (isLoaded)
            {
                if (_status == AssetState.FAILD)
                {
                    if (WebRequestLoader.mapping404[_url])
                    {
                        this.dispatchEvent(new SAEventX(SAEventX.FAILED, "404"));
                        return;
                    }

                    _status = AssetState.LOADING;
                    
                }
            }
        }

        public bool isLoaded
        {
            get { return (_status == AssetState.READY || _status == AssetState.FAILD); }
        }

        public virtual bool recycleToPool(PoolItem poolItem)
        {
            if (pool == null)
            {
                pool=new Stack<PoolItem>();
            }

            GameObject go = poolItem.gameObject;
            if (pool.Count < maxPoolSize)
            {
                go.SetActive(false);
                pool.Push(poolItem);
                return true;
            }
            GameObject.Destroy(go);
            return false;
        }

        public string url
        {
            get { return _url; }
        }

        public int retainCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int release()
        {
            if (--_reference < 1)
            {
                _reference = 0;
                AutoReleasePool.add(this);
            }
            return _reference;
        }

        public int retain()
        {
            if (++_reference == 1)
            {
                AutoReleasePool.remove(this);
            }
            return _reference;
        }

        public void __dispose()
        {
            throw new NotImplementedException();
        }

        public virtual object data
        {
            get { return _data; }
        }
    }
}