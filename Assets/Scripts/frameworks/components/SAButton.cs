using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    public class SAButton : SASkinBase
    {
        private Button button;
        private Text label;
        private GameObject disabled;
        private string lableName;
        private GameObject @default;//按钮默认图片
        private Image defaultImage;//按钮默认图片
        private string labelTxt = "";
        private string clickSoundName = "";

        public SAButton(GameObject skin, string lableName = "text")
        {
            this.lableName = lableName;
            this.skin = skin;
        }

        protected override void bindComponents()
        {
            disabled = getGameObject("disabled");
            defaultImage = skin.GetComponent<Image>();
            if (disabled != null)
            {
                disabled.SetActive(false);
            }

            label = getText(lableName);

            @default = getGameObject("default");

            button = getButton("");
            if (button == null)
            {
                button = skin.AddComponent<Button>();
            }
            button.onClick.AddListener(clickHandle);

            base.bindComponents();
        }

        private void clickHandle()
        {
            if (_enabled == false)
            {
                return;
            }

            if (hasEventListener(SAEventX.CLICK))
            {
                SAEventX e=SAEventX.FromPool(SAEventX.CLICK);
                bool b = dispatchEvent(e);
                SAEventX.ToPool(e);
            }
        }

        protected override void doEnabled()
        {
            if (disabled != null)
            {
                disabled.SetActive(!enabled);
            }

            if (button != null)
            {
                button.enabled = enabled;
            }

            base.doEnabled();
        }


        public string text
        {
            get { return labelTxt; }
            set
            {
                labelTxt = value;

                if (label != null)
                {
                    label.text = value;
                }
            }
        }
    }

}
