using System.Text;
using UnityEngine;

namespace Sakura
{
    public class PathDefine
    {
        public const string U3D = ".unity3d";

        public static string soundPath;
        public static string uiPath;

        private static StringBuilder sbBuilder=new StringBuilder();

        private static string persistentDataPath;
        private static string streamingAssetsPath;

        static PathDefine()
        {
            persistentDataPath = Application.persistentDataPath;
            streamingAssetsPath = Application.streamingAssetsPath;
        }

        public static string getPersistentLocal(string uri = "", bool isWWW = false)
        {
            string prefix = "";
            if (isWWW)
            {
                prefix = getLocalHttpPrefix();
            }
            sbBuilder.Clear();
            sbBuilder.Append(prefix);
            sbBuilder.Append(persistentDataPath);
            sbBuilder.Append("/");
            sbBuilder.Append(uri);
            return sbBuilder.ToString();
        }

        public static string getLocalHttpPrefix()
        {
            string prefix = "file://";
            if (Application.isEditor)
            {
                prefix = "file:///";
            }

            return prefix;
        }

        public static string getStreamingAssetsLocal(string uri = "", bool isWWW = false)
        {
            string prefix = "";
            if (isWWW)
            {
                prefix = "file://";
                if (Application.platform==RuntimePlatform.Android)
                {
                    prefix = "";
                }
            }
            sbBuilder.Clear();
            sbBuilder.Append(prefix);
            sbBuilder.Append(streamingAssetsPath);
            sbBuilder.Append("/");
            sbBuilder.Append(uri);

            return sbBuilder.ToString();
        }
    }
}