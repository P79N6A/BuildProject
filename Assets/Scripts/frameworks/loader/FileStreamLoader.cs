using System.IO;
using UnityEngine;

namespace Sakura
{
    public class FileStreamLoader:RFLoader
    {
        private string fullLocalURL;

        public FileStreamLoader(string fullLocalUrl, string url, LoaderXDataType parserType) : base(url, parserType)
        {
            this.fullLocalURL = fullLocalUrl;
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
                DebugX.LogWarning("localLoading:"+fullLocalURL);
                return;
            }

            if (File.Exists(fullLocalURL) == false)
            {
                _status = LoadState.ERROR;
                string message = string.Format("加载文件失败：{0} error:文件不存在", fullLocalURL);
                DebugX.LogWarning(message);
                this.simpleDispatch(SAEventX.FAILED, message);
                return;
            }

            switch (_parserType)
            {
                case LoaderXDataType.ASSETBUNDLE:
                case LoaderXDataType.MANIFEST:
                    onAssetBundleHandle(AssetBundle.LoadFromFile(fullLocalURL));
                    break;
                case LoaderXDataType.TEXTURE:
                    _status = LoadState.COMPLETE;
                    byte[] bytes = File.ReadAllBytes(fullLocalURL);
                    Texture2D tex=new Texture2D(2,2,TextureFormat.ARGB32,false,false);
                    tex.LoadImage(bytes);
                    _data = tex;
                    this.simpleDispatch(SAEventX.COMPLETE, _data);
                    break;
                case LoaderXDataType.BYTES:
                case LoaderXDataType.AMF:
                    _status = LoadState.COMPLETE;
                    _data = File.ReadAllBytes(fullLocalURL);
                    this.simpleDispatch(SAEventX.COMPLETE, _data);
                    break;
            }
//            selfComplete();
        }
    }
}