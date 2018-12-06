using System;

namespace Sakura
{
    public static class DateFormatUtils
    {
        public static int ZoneMinuteOffset = 0;

        public static string ServerTimeDisplay(this long serverTimeMilliseconds, string format)
        {
            DateTime dtStart = new DateTime(1970, 1, 1);
            long value = serverTimeMilliseconds + ZoneMinuteOffset * SADateUtils.ONE_MINUTE_MILLISECOND;
            dtStart = dtStart.Add(new TimeSpan(value * 10000));

            return String.Format(format, dtStart);
        }

        /// <summary>
        /// 返回的格式：XX天XX时XX分XX秒
        /// </summary>
        /// <param name="milliseconds">毫秒</param>
        /// <returns></returns>
        public static string GetFormatTimeByMilliseconds(long milliseconds)
        {
            string result = "";
            int day = (int)(milliseconds / SADateUtils.ONE_DAY_MILLISECOND);
            milliseconds %= SADateUtils.ONE_DAY_MILLISECOND;
            int hour = (int)(milliseconds / SADateUtils.ONE_HOURS_MILLISECOND);
            milliseconds %= SADateUtils.ONE_HOURS_MILLISECOND;
            int minutes = (int)(milliseconds / SADateUtils.ONE_MINUTE_MILLISECOND);
            milliseconds %= SADateUtils.ONE_MINUTE_MILLISECOND;
            int second = (int)(milliseconds / SADateUtils.ONE_SECOND_MILLISECOND);

            if (day != 0)
            {
                result += String.Format("{0}天", day);
            }
            if (hour != 0)
            {
                result += String.Format("{0}时", hour);
            }
            if (minutes != 0)
            {
                result += String.Format("{0}分", minutes);
            }
            if (second != 0)
            {
                result += String.Format("{0}秒", second);
            }

            return result;
        }
    }
}