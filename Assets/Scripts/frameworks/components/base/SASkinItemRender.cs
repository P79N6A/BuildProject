using System;
using UnityEngine;

namespace Sakura
{
    public class SASkinItemRender : SASkinBase, IListItemRender
    {
        protected bool _isSelected = false;
        protected int _index;
        protected bool _clickEnable;

        /***********************************Public***********************************/

        public int index
        {
            get { return _index; }
            set { _index = value; }
        }

        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                doSelected(value);
            }
        }

        public Action<string, IListItemRender, object> itemEventHandle { get; set; }

        public override void Dispose()
        {
            GameObject oldSkin = _skin;
            if (oldSkin != null)
            {
                skin = null;
                GameObject.Destroy(oldSkin);
            }
            base.Dispose();
        }

        public void refresh()
        {
            Refresh();
        }

        /***********************************Protected***********************************/

        protected override void doData()
        {
        }

        protected virtual void doSelected(bool value)
        {
        }

        protected void clickHandler()
        {
            this.simpleDispatch(SAEventX.CLICK);
        }


    }
}