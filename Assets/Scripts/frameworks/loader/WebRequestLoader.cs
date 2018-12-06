using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Sakura
{
    public class WebRequestLoader : RFLoader
    {


        public WebRequestLoader(string url, LoaderXDataType parserType) : base(url, parserType)
        {
        }

        public override void load()
        {
            if (string.IsNullOrEmpty(_url))
            {
                this.dispatchEvent(new SAEventX(SAEventX.FAILED, "文件路径为空：" + _url));
                return;
            }

            if (_data != null)
            {
                this.simpleDispatch(SAEventX.COMPLETE, _data);
                return;
            }

            if (_status == LoadState.LOADING)
            {
                DebugX.LogWarning("正在load资源：" + _url);
            }

            if (!isLocalFile)
            {
                DebugX.LogWarning("从远程加载资源：" + _url);
            }

            _retriedCount = 0;
            _status = LoadState.LOADING;
            StartCoroutine(doLoad(_url));
        }

        protected void retryLoad(float delay = 0)
        {
            DebugX.LogWarning("{0}重试：{1}", _retriedCount, _url);
            _status = LoadState.LOADING;
            StartCoroutine(doLoad(_url));
        }

        private IEnumerator doLoad(string url)
        {
            UnityWebRequest request;
            switch (_parserType)
            {
                case LoaderXDataType.BYTES:
                case LoaderXDataType.AMF:
                case LoaderXDataType.TEXTURE:
                case LoaderXDataType.MANIFEST:
                case LoaderXDataType.ASSETBUNDLE:
                    request = UnityWebRequest.Get(url);
                    break;
                case LoaderXDataType.POST:
                    request = UnityWebRequest.Post(url, postData);
                    break;
                case LoaderXDataType.GET:
                    string fullPath = url;
                    if (string.IsNullOrEmpty(postData) == false)
                    {
                        fullPath = url + "?" + postData;
                    }
                    request = UnityWebRequest.Get(fullPath);
                    break;
                default:
                    request = UnityWebRequest.Get(url);
                    break;
            }

            float startTime = Time.realtimeSinceStartup;
            bool isTimeout = false;
            if (timeout > 0)
            {
                request.timeout = timeout;
            }

            while (!request.isDone)
            {
                if (timeout > 0 && Time.realtimeSinceStartup - startTime > timeout * 2)
                {
                    isTimeout = true;
                    break;
                }

                if (checkProgress)
                {
                    update(request.downloadProgress);
                }

                yield return request.SendWebRequest();
            }

            long responseCode = request.responseCode;
            if (request.isHttpError || (responseCode != 200 && responseCode != 204))
            {
                string error = "code=" + responseCode;
                if (isTimeout)
                {
                    error += ",error=isTimeout:" + timeout;
                }
                else if (request.isHttpError)
                {
                    error += "error=" + request.error;
                }
                else
                {
                    if (responseCode == 404)
                    {
                        mapping404[_url] = true;
                    }
                }

                _status = LoadState.ERROR;
                string message = string.Format("下载文件失败：{0} reson:{1}", _url, error);
                DebugX.LogWarning(message);

                request.Dispose();
                request = null;

                if (retryCount > _retriedCount)
                {
                    _retriedCount++;
                    CallLater.Add(retryLoad, 1.0f);
                    yield break;
                }

                this.simpleDispatch(SAEventX.FAILED, message);
            }
            else
            {
                _status = LoadState.COMPLETE;
                switch (_parserType)
                {
                    case LoaderXDataType.BYTES:
                    case LoaderXDataType.AMF:
                        _data = request.downloadHandler.data;
                        break;
                    case LoaderXDataType.ASSETBUNDLE:
                    case LoaderXDataType.MANIFEST:
                        onAssetBundleHandle(AssetBundle.LoadFromMemory(request.downloadHandler.data));
                        break;
                    case LoaderXDataType.TEXTURE:
                        Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false, false);
                        tex.LoadImage(request.downloadHandler.data);
                        _data = tex;
                        break;
                    default:
                        _data = request.downloadHandler.data;
                        break;
                }
                request.Dispose();
                request = null;
                this.simpleDispatch(SAEventX.COMPLETE, _data);
            }
//            selfComplete();
        }
    }
}