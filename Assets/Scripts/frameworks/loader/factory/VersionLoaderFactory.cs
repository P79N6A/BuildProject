using System.Collections.Generic;
using Sakura;

namespace Sakura
{
    public class VersionLoaderFactory
    {
        public const string KEY = "vdata";

        public Dictionary<string, HashSizeFile> localMapping = new Dictionary<string, HashSizeFile>();
        public Dictionary<string, HashSizeFile> remoteMapping = new Dictionary<string, HashSizeFile>();

        protected static string PRE_HASH = "";

        private static int PRE_HASH_LEN = 0;
        private static VersionLoaderFactory instance;

        public static VersionLoaderFactory GetInstance()
        {
            if (instance == null)
            {
                instance=new VersionLoaderFactory();
            }
            return instance;
        }

        public string getLocalPathByURL(string url, bool isSubFix = false)
        {
            string localPath = "";
            int index = url.IndexOf(PRE_HASH);
            if (index != -1)
            {
                localPath = url.Substring(index + PRE_HASH_LEN);
                localPath = formatedLocalURL(localPath);
                if (isSubFix == false)
                {
                    localPath = PathDefine.getPersistentLocal(localPath);
                }
            }
            return localPath;
        }

        protected virtual string formatedLocalURL(string localPath)
        {
            int index = localPath.IndexOf('?');
            if (index == -1)
            {
                return localPath;
            }
            return localPath.Substring(0, index);
        }
    }
}