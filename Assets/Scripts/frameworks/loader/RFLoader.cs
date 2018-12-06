using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakura
{
    public class RFLoader:EventDispatcher
    {
        public static float DEBUG_TIMEOUT = 5.0f;
        public static Dictionary<string, bool> mapping404 = new Dictionary<string, bool>();
        public static Dictionary<string,AssetBundle> assetBundleMapping=new Dictionary<string, AssetBundle>();

        public int timeout = -1;
        public uint retryCount = 0;

        public string postData;
        public string _url;

        public bool isLocalFile = true;
        public bool checkProgress = false;
        
        protected int _retriedCount = 0;
        protected object _data;
        protected LoadState _status;
        protected LoaderXDataType _parserType;

        private float startTime = 0;
        private float preTime = 0;
        private float preProgress = 0;
        private float sinceTime = 0;
        private float lastDebugTime = 0;

        public RFLoader(string url, LoaderXDataType parserType)
        {
            _url = url;
            _parserType = parserType;
        }

        public static void ADD_TO_MAPPING(string url, AssetBundle assetBundle)
        {
            if (assetBundle == null)
            {
                DebugX.LogWarning("assetBundle is null:"+url);
            }

            AssetBundle old = null;
            string key = url.ToLower();
            assetBundleMapping.TryGetValue(key, out old);
            if (old != null)
            {
                throw new Exception("assetBundle exit:"+key);
            }

            assetBundleMapping[key] = assetBundle;
        }

        public static void REMOVE_FROM_MAPPING(string url)
        {
            if (string.IsNullOrEmpty(url) == false)
            {
                AssetBundle assetBundle = null;
                string key = url.ToLower();
                if (assetBundleMapping.TryGetValue(key, out assetBundle))
                {
                    assetBundleMapping.Remove(key);
                    if (assetBundle != null)
                    {
                        assetBundle.Unload(false);
                    }
                }
            }
        }

        public virtual void load()
        {

        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return AbstractApp.coreLoaderQueue.StartCoroutine(routine);
        }

        protected void update(float progress)
        {
            if (startTime == 0)
            {
                startTime = lastDebugTime = Time.realtimeSinceStartup;
            }

            sinceTime = Time.realtimeSinceStartup - startTime;
            if (progress != preProgress && progress > 0)
            {
                preTime = Time.realtimeSinceStartup;
                preProgress = progress;
                this.simpleDispatch(SAEventX.PROGRESS, preProgress);
            }

            if ((Time.realtimeSinceStartup - lastDebugTime) > DEBUG_TIMEOUT)
            {
                lastDebugTime = Time.realtimeSinceStartup;
                DebugX.Log(_url + "time:" + sinceTime + "pro:" + progress);
            }
        }

        protected virtual void onAssetBundleHandle(AssetBundle assetBundle)
        {
            if (assetBundle != null)
            {
                ADD_TO_MAPPING(_url,assetBundle);
                _status = LoadState.COMPLETE;
                _data = assetBundle;
                this.simpleDispatch(SAEventX.COMPLETE, _data);
            }
            else
            {
                _status = LoadState.ERROR;
                _data = null;

                string message = string.Format("load:加载：{0}，数据没有assetBundle", _url);
                DebugX.LogWarning(message);
                this.simpleDispatch(SAEventX.FAILED, message);
            }
        }

        protected void selfComplete()
        {
            this.simpleDispatch(SAEventX.COMPLETE);
        }
    }
}