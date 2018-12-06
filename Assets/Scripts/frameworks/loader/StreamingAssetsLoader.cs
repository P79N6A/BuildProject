﻿using System;
using System.Collections;
using UnityEngine;

namespace Sakura
{
    public class StreamingAssetsLoader : RFLoader
    {
        private string fullLocalURL;

        public StreamingAssetsLoader(string fullLocalURL,string url, LoaderXDataType parserType) : base(url, parserType)
        {
            this.fullLocalURL = fullLocalURL;
        }

        public override void load()
        {
            if (_data != null)
            {
                this.simpleDispatch(SAEventX.COMPLETE, _data);
                return;
            }

            if (_status == LoadState.LOADING)
            {
                DebugX.LogWarning("StreamingLoading:"+fullLocalURL);
                return;
            }

            _status = LoadState.LOADING;
            StartCoroutine(doLoad(fullLocalURL));
        }

        private IEnumerator doLoad(string fullLocalURL)
        {
            WWW www=new WWW(fullLocalURL);
            while (!www.isDone)
            {
                if (checkProgress)
                {
                    update(www.progress);
                }
                yield return null;
            }

            string error = www.error;
            if (string.IsNullOrEmpty(error))
            {
                _status = LoadState.COMPLETE;
                switch (_parserType)
                {
                    case LoaderXDataType.BYTES:
                    case LoaderXDataType.AMF:
                        _data = www.bytes;
                        break;
                    case LoaderXDataType.MANIFEST:
                    case LoaderXDataType.ASSETBUNDLE:
                        onAssetBundleHandle(www.assetBundle);
                        break;
                    case LoaderXDataType.TEXTURE:
                        Texture2D tex;
                        tex=new Texture2D(2,2,TextureFormat.ARGB32,false,false);
                        www.LoadImageIntoTexture(tex);
                        _data = tex;
                        break;
                }
                www.Dispose();
                www = null;
                this.simpleDispatch(SAEventX.COMPLETE, _data);
            }
            else
            {
                string message = string.Format("加载文件失败：{0} error:{1}", _url, error);
                DebugX.LogWarning(message);
                www.Dispose();
                www = null;
                this.simpleDispatch(SAEventX.FAILED, message);
            }
        }
    }
}