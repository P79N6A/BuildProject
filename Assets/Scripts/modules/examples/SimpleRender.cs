using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    public class SimpleVO
    {
        public string txt;

        public SimpleVO(string str)
        {
            this.txt = str;
        }
    }

    public class SimpleRender:SAListItemRender
    {
        [SerializeField] private GameObject bgGameObject;
        [SerializeField] private Text display;

        void Awake()
        {
            SAButton btn=new SAButton(bgGameObject);
            btn.addEventListener(SAEventX.CLICK, ClickHandler);
        }

        private void ClickHandler(SAEventX obj)
        {
            display.text = "点到了";
            this.simpleDispatch(SAEventX.CLICK);
        }

        protected override void doData()
        {
            SimpleVO vo = data as SimpleVO;
            display.text = vo.txt;
            base.doData();
        }
    }
}