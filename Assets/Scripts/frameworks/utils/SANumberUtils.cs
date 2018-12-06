using System;

namespace Sakura
{
    public class SANumberUtils
    {
        private static string[] numStr = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        /// <summary>
        /// 阿拉伯数字转换中文数字:1000--->一千
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string GetChineseFormat(int number)
        {
            string res = "";
            string str = number.ToString();
            int length = str.Length;
            for (int i = 0; i < length; i++)
            {
                int c = Int32.Parse(str[i].ToString());
                string s = numStr[c];
                if (c != 0)
                {
                    switch (length - i)
                    {
                        case 2:
                        case 6:
                            if (c == 1 && str.Length == 2)
                            {
                                s = "";
                            }

                            s += "十";
                            break;
                        case 3:
                        case 7:
                            s += "百";
                            break;
                        case 4:
                        case 8:
                            s += "千";
                            break;
                        case 5:
                            s += "万";
                            break;
                        case 9:
                            s += "亿";
                            break;
                        default:
                            s += "";
                            break;
                    }
                }

                if (s != "零" || res.Length == 0 || res[res.Length - 1] != '零')
                {
                    res += s;
                }
            }

            while (res.Length > 1 && res[res.Length - 1] == '零')
            {
                res = res.Substring(0, res.Length - 1);
            }

            return res;
        }

        /// <summary>
        /// 将阿拉伯数字转换为货币格式：1,000,000
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetCurrentFormat(long num)
        {
            return String.Format("{0:N0}", num);
        }

        /// <summary>
        /// 转化为：10.3万
        /// </summary>
        /// <param name="a"></param>
        /// <param name="showFloat"></param>
        /// <returns></returns>
        public static string GetNumberFormat1(long a, bool showFloat = true)
        {
            string coinMes = a.ToString();
            if (a >= 10000)
            {
                coinMes = a / 10000 + "";
                long b = a % 10000 / 100;
                if (b > 0 && showFloat)
                {
                    coinMes += "." + (b >= 10 ? b + "" : "0" + b);
                }

                coinMes += "万";
            }

            if (a >= 100000000)
            {
                coinMes = a / 100000000 + "";
                long b = a % 100000000 / 1000000;
                if (b > 0 && showFloat)
                {
                    coinMes += "." + (b >= 10 ? b + "" : "0" + b);
                }

                coinMes += "亿";
            }

            return coinMes;
        }


        /// <summary>
        /// 转换为：10K9M
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetNumberFormat2(long size)
        {
            float num = 1024.00f; //byte

            if (size < num)
                return size + "";
            if (size < Math.Pow(num, 2))
                return (size / num).ToString("f2") + "K"; //kb
            if (size < Math.Pow(num, 3))
                return (size / Math.Pow(num, 2)).ToString("f2") + "M"; //M
            if (size < Math.Pow(num, 4))
                return (size / Math.Pow(num, 3)).ToString("f2") + "G"; //G

            return (size / Math.Pow(num, 4)).ToString("f2") + "T"; //T
        }
    }

}
