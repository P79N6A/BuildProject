using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    /// <summary>
    /// 面板基类
    /// 只有关闭，没有打开，因为自己没法创建自己
    /// (这个类很鸡肋……)
    /// </summary>
    public class SAAbstractView : SAMonoDispatcher
    {
        [Tooltip("背景层颜色和透明度")]
        [SerializeField]
        protected Color backgroundColor = Color.white;

        [Tooltip("是否点击背景关闭界面（true-点击关闭）")]
        [SerializeField]
        protected bool clickBackgroundHide = false;

        [Tooltip("是否模态框（true-模态有背景）")]
        [SerializeField]
        protected bool isModel = true;

        private const string backgroundName = "Background";

        private GameObject _background;

        public SAAbstractView()
        {
        }

        void Start()
        {
            if (isModel)
            {
                showBackground();
            }
        }

        public bool isShow
        {
            get
            {
                if (skin != null)
                {
                    return skin.activeInHierarchy;
                }

                return false;
            }
        }

        public GameObject skin
        {
            get { return this.gameObject; }
        }


        public T getComponent<T>(string path = "", GameObject go = null) where T : Component
        {
            return SAUIExtend.GetComponent<T>(go, path);
        }

        public Text getText(string name, GameObject parent = null)
        {
            return getComponent<Text>(name, parent);
        }

        public Image getRawImage(string name, GameObject parent = null)
        {
            return getComponent<Image>(name, parent);
        }

        public Button getButton(string name, GameObject parent = null)
        {
            return getComponent<Button>(name, parent);
        }

        public Image getImage(string name, GameObject parent = null)
        {
            return getComponent<Image>(name, parent);
        }

        public GameObject getGameObject(string name, GameObject go = null)
        {
            if (go == null)
            {
                go = skin;
            }
            Transform transform = go.transform.Find(name);
            if (transform != null)
            {
                return transform.gameObject;
            }

            return null;
        }


        //----------------------------------------Protected---------------------------------------//
        //只能内部调用，防止被意外销毁
        protected void hide(SAEventX e = null)
        {
            if (!isShow)
            {
                return;
            }

            doHide();

            if (skin != null)
            {
//                UIBase uiBase = skin.GetComponent<UIBase>();
//                if (uiBase != null)
//                {
//                    LfUI.DespawnUI(uiBase.Id);
//                }
                GameObject.Destroy(skin);
            }
        }

        protected virtual void doHide()
        {
            
        }

        protected void backgroundClickHandle(SAEventX e)
        {
            this.hide(e);
        }

        //----------------------------------------Private---------------------------------------//

        private void showBackground()
        {
            _background = getGameObject(backgroundName);//说明已有背景，且被放到面板最底层

            if (_background == null)
            {
                _background = getBackground();

                _background.transform.SetParent(skin.transform, false);
                _background.transform.SetAsFirstSibling();
                _background.SetActive(true);
            }

            if (clickBackgroundHide)
            {
                Get(_background).addEventListener(SAEventX.CLICK, backgroundClickHandle);
            }
        }

        private GameObject getBackground()
        {
            GameObject go = null;
            Image rawImage = SAUIExtend.CreateImage(backgroundName);
            rawImage.raycastTarget = true;
            rawImage.color = backgroundColor;
            go = rawImage.gameObject;

//            go.transform.SetParent(skin.transform, false);
//            go.transform.SetAsFirstSibling();
//            go.SetActive(true);

            RectTransform transform = go.GetComponent<RectTransform>();
            transform.sizeDelta=new Vector2(1920,1080);

            return go;
        }

    }

}
