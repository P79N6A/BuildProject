using System;
using System.Collections.Generic;

namespace Sakura
{
    public delegate AssetResource RouterResourceDelegate(string url, string uri, string prefix);

    public class AssetsManager
    {
        public static RouterResourceDelegate routerResourceDelegate;

        private Dictionary<string, AssetResource> _resourceMap = new Dictionary<string, AssetResource>();
        private static Dictionary<LoaderXDataType,Type> resourceTypeMapping=new Dictionary<LoaderXDataType, Type>();

        private static AssetsManager _instance;
        public static AssetsManager instance()
        {
            if (_instance == null)
            {
                _instance=new AssetsManager();
            }
            return _instance;
        }

        public static AssetResource getResource(string url, LoaderXDataType autoCreateTypy = LoaderXDataType.BYTES)
        {
            return instance()._getResource(url, autoCreateTypy);
        }

        public static void bindEventHandle(IEventDispatcher resource, Action<SAEventX> handle, bool isBind = true)
        {
            if (isBind)
            {
                resource.addEventListener(SAEventX.COMPLETE, handle);
                resource.addEventListener(SAEventX.FAILED, handle);
            }
            else
            {
                resource.removeEventListener(SAEventX.COMPLETE, handle);
                resource.removeEventListener(SAEventX.FAILED, handle);
            }
        }

        public AssetResource findResource(string url)
        {
            if (url == null)
            {
                return null;
            }
            AssetResource res = null;
            string key = url.ToLower();
            if (_resourceMap.TryGetValue(key, out res))
            {
                return res;
            }
            return null;
        }

        private AssetResource _getResource(string url, LoaderXDataType autoCreateType = LoaderXDataType.BYTES)
        {
            AssetResource res = findResource(url);
            if (res == null)
            {
                Type cls = null;
                if (resourceTypeMapping.TryGetValue(autoCreateType, out cls)==false)
                {
                    res=new AssetResource();
                }
                else
                {
                    res = (AssetResource) Activator.CreateInstance(cls, url);
                }

                res.parserType = autoCreateType;
                res.addEventListener(SAEventX.DISPOSE, resourceDisposeHandle);

                string key = url.ToLower();
                _resourceMap[key] = res;
            }

            return res;
        }

        private void resourceDisposeHandle(SAEventX e)
        {
            AssetResource res=e.target as AssetResource;
            res.removeEventListener(SAEventX.DISPOSE, resourceDisposeHandle);

            string uri = res.url.ToLower();
            if (_resourceMap.ContainsKey(uri))
            {
                _resourceMap.Remove(uri);
            }
        }
    }
}