using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Sakura
{
    public class ResourceLoaderManager : ILoaderFactory
    {
        private VersionLoaderFactory versionLoaderFactory;
        private Dictionary<string,RFLoader> _loadingPool=new Dictionary<string, RFLoader>();

        public RFLoader getLoader(AssetResource resource)
        {
            RFLoader loader = null;
            string url = resource.url;
            LoaderXDataType parserType = resource.parserType;
            if (_loadingPool.TryGetValue(resource.url, out loader))
            {
                return loader;
            }

            string localPath = versionLoaderFactory.getLocalPathByURL(url, true);
            if (resource.isForceRemote == false)
            {
                string fullLocalPath = PathDefine.getPersistentLocal(localPath);
                if (File.Exists(fullLocalPath))
                {
                    loader=new FileStreamLoader(fullLocalPath,url,parserType);
                }
                else
                {
                    fullLocalPath = PathDefine.getStreamingAssetsLocal(localPath, true);
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        loader=new WebRequestLoader(fullLocalPath,parserType);
                    }
                    else
                    {
                        loader=new StreamingAssetsLoader(fullLocalPath, url,parserType);
                    }
                }
            }

            if (loader == null)
            {
                loader=new WebRequestLoader(url,parserType);
                loader.isLocalFile = false;
                if (resource.isForceRemote)
                {
                    loader.postData = resource.postData;
                    loader.timeout = resource.timeout;
                }
            }

            _loadingPool[resource.url] = loader;
            return loader;
        }
    }
}