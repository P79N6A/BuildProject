using System;

namespace Sakura
{
    public class SAHtmlUtil
    {
        public static string renderColor(object value, string color)
        {
            if (value == null)
            {
                return "";
            }
            return renderColor(value.ToString(), color);
        }

        public static string renderColor(string value, string color)
        {
            if (String.IsNullOrEmpty(value))
            {
                return "";
            }
            if (color.IndexOf("#") != 0)
            {
                color = "#" + color;
            }
            return "<color='" + color + "'>" + value.ToString() + "</color>";
        }

        /// <summary>
        /// 根据string 生成 带有超链接事件 标签的HtmlText
        /// </summary>
        /// <param name="linkName"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string renderLink(string linkName, string e)
        {
            return "<a href=[" + e + "]>" + linkName + "</a>";
        }

        /// <summary>
        /// 超链接文本解析，actionStr 超链接点击文本返回，color 超链接颜色
        /// </summary>
        /// <param name="str"></param>
        /// <param name="actionStr"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string renderActionColor(string str, string actionStr, string color)
        {
            if (color.IndexOf("#") != 0)
            {
                color = "#" + color;
            }

            return "<a href=[" + actionStr + "]><color=" + color + ">" + str + "</color></a>";
        }
    }

}

