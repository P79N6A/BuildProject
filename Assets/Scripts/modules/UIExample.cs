using Msdk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Save = System.IO;

namespace Sakura
{
    public class UIExample:MonoBehaviour
    {
        [SerializeField] private GameObject buttonObj;
        [SerializeField] private GameObject pagelistObj;
        [SerializeField] private GameObject renderObj;
        [SerializeField] private RawImage image;

        private SAButton btn;
        private SAPageList pageList;
        private Texture2D t;
        private string path;

        void Awake()
        {
//            ScreenShot();

            btn = new SAButton(buttonObj);
            btn.addEventListener(SAEventX.CLICK, btnHandler);

//            List<SimpleVO> list=new List<SimpleVO>(4);
//            list.Add(new SimpleVO("111"));
//            list.Add(new SimpleVO("222"));
//            list.Add(new SimpleVO("333"));
//            list.Add(new SimpleVO("444"));
//            list.Add(new SimpleVO("555"));
//            list.Add(new SimpleVO("666"));
//            list.Add(new SimpleVO("777"));
//            list.Add(new SimpleVO("888"));
//            list.Add(new SimpleVO("999"));
//            list.Add(new SimpleVO("452"));
//            list.Add(new SimpleVO("452"));
//            list.Add(new SimpleVO("452"));
//
//            SAListRenderFactory<SimpleRender> factory=new SAListRenderFactory<SimpleRender>(renderObj);
//            pageList=new SAPageList(factory,pagelistObj,0,0,false,3);
//            pageList.dataProvider = list;
//            pageList.addEventListener(SAEventX.ITEM_CLICK, ItemClickHandler);
        }


        void OnGUI()
        {
            if (GUI.Button(new Rect(1000, 50, 200, 100), "隐藏UI层"))
            {
                Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
            }

            if (GUI.Button(new Rect(1000, 200, 200, 100), "显示"))
            {
                Camera.main.cullingMask |= (1 << LayerMask.NameToLayer("UI"));
            }

            if (GUI.Button(new Rect(1000, 400, 200, 100), "全部隐藏"))
            {
                Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("UI"));
            }
        }

        private void ItemClickHandler(SAEventX obj)
        {
            Debug.Log("pagelist点到了->"+pageList.selectedIndex);
        }

        private void btnHandler(SAEventX obj)
        {
            Camera.main.cullingMask &= ~(1 << 8);

            //            string imgUrl = "UI/newShare/btn_close";
            //            Texture2D texture2D = Resources.Load<Texture2D>(imgUrl);
            //            texture2D.EncodeToPNG();
            //            byte[] bytes = texture2D.GetRawTextureData();
            //#if UNITY_IOS
            //            mediaTagName = "MSG_INVITE";
            //            byte[] imgData = bytes;
            //            int imgDataLen = imgData.Length;
            //            WGPlatform.Instance.WGSendToWeixinWithPhoto(eWechatScene.WechatScene_Timeline, mediaTagName, imgData,
            //                imgDataLen, messageExt, messageAction);
            //#elif UNITY_ANDROID
            //            string imgLocalUrl = imgUrl;
            //            WGPlatform.Instance.WGSendToWeixinWithPhotoPath(eWechatScene.WechatScene_Session, "777", imgLocalUrl,
            //                "444", "555");
            //#endif
        }



        private Texture2D texture2D;
        public void ScreenShot()
        {
            StartCoroutine(WaitToCapture());
            StartCoroutine(readPic());
        }

        private IEnumerator WaitToCapture()
        {
            yield return new WaitForEndOfFrame();
            Texture2D t = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
            //以左下角为原点读取
            t.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            t.Apply();

            SaveToPic(t);
        }

        private IEnumerator readPic()
        {
            yield return new WaitForEndOfFrame();
            byte[] bytes = FileHelper.GetBytes(path);
            Texture2D texture2D = new Texture2D(Screen.width, Screen.height);
            texture2D.LoadImage(bytes);
            image.texture = texture2D;
        }

        private void SaveToPic(Texture2D tex)
        {
            if (null != tex)
            {
                path = Application.persistentDataPath + "/" + "test1.jpg";
                DebugX.Log(path);
                Save.File.WriteAllBytes(path, tex.EncodeToJPG());
            }
        }
    }
}