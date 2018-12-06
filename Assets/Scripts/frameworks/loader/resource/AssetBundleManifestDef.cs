using UnityEngine;

namespace Sakura
{
    public class AssetBundleManifestDef
    {
        public string manifestKey = "";
        public string manifestPrefix = "";

        public AssetBundleManifest manifest;

        public override string ToString()
        {
            return manifestPrefix;
        }
    }

    public class Hash128Link
    {
        public Hash128 hash128;
        public AssetBundleManifestDef manifestDef;
        public string key;
    }
}