using UnityEngine;
using UnityEngine.UI;

namespace Sakura
{
    public class SANumericStepper : EventDispatcher
    {
        protected int _min = 0;
        protected int _max = 10;
        protected int _pad = 1;
        protected int _value;

        protected Text _text;
        protected SAButton addBtn;
        protected SAButton minusBtn;

        public SANumericStepper(GameObject add, GameObject minus, Text txt,
            int min = 0, int max = 10, int pad = 1)
        {
            addBtn=new SAButton(add);
            addBtn.addEventListener(SAEventX.CLICK, onAdd);

            minusBtn =new SAButton(minus);
            minusBtn.addEventListener(SAEventX.CLICK, onMinus);

            _text = txt;

            _min = min;
            _max = max;
            _pad = pad;

            value = _min;
        }


        public int value
        {
            get { return _value; }
            set
            {
                _value = value;
                invalidate();
            }
        }

        /// <summary>
        /// 单纯改变数字
        /// </summary>
        /// <param name="v"></param>
        public void setValue(int v)
        {
            _value = v;
            _text.text = value.ToString();
        }

        public void setMaxMin(int min = 0, int max = 10, int pad = 1)
        {
            _min = min;
            _max = max;
            _pad = pad;
            value = _min;

            if (max < min)
            {
                _max = _min = value = 0;
            }
            
            checkValue(value);
        }

        private void onAdd(SAEventX obj)
        {
            var newValue = value + _pad;
            checkValue(newValue);
        }

        private void onMinus(SAEventX obj)
        {
            var newValue = value - _pad;
            checkValue(newValue);
        }

        private void checkValue(int newValue)
        {
            if (_max == _min && newValue == _max)
            {
                addBtn.enabled = minusBtn.enabled = false;
            }
            else
            {
                addBtn.enabled = true;
                minusBtn.enabled = true;

                if (newValue >= _max)
                {
                    addBtn.enabled = false;
                    minusBtn.enabled = true;
                    newValue = _max;
                }

                if (newValue <= _min)
                {
                    minusBtn.enabled = false;
                    addBtn.enabled = true;
                    newValue = _min;
                }

                if (newValue != value)
                {
                    value = newValue;
                }
            }
        }

        private void invalidate()
        {
            _text.text = value.ToString();
            simpleDispatch(SAEventX.CHANGE);
        }

    }
}
