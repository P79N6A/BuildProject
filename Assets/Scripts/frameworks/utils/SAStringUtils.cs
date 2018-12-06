using System;
using System.Text;

namespace Sakura
{
    public class SAStringUtils
    {
        /// <summary>
        /// 获取字符串的字节长度(汉字占2字节)
        /// </summary>
        /// <returns></returns>
        public static int GetBytesLength(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return 0;
            }

            int len = System.Text.Encoding.UTF8.GetBytes(str).Length;
            if (len <= 0)
            {
                len = str.Length;
            }

            return len;
        }

        public static string Substitute(string value, params object[] parms)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            string v;
            int len = parms.Length;
            for (int i = 0; i < len; i++)
            {
                object vo = parms[i];
                if (vo == null)
                {
                    continue;
                }

                v = vo.ToString();
                value = value.Replace("{" + i + "}", v);
            }

            return value;
        }

        public static string Substitute(string value, string[] parms)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            if (parms == null)
            {
                return value;
            }

            string v;
            int len = parms.Length;
            for (int i = 0; i < len; i++)
            {
                object vo = parms[i];
                if (vo == null)
                {
                    continue;
                }

                v = vo.ToString();
                value = value.Replace("{" + i + "}", v);
            }

            return value;
        }
    }

}
