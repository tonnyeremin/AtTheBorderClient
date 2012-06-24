using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Onboard
{
    public static class Utils
    {
        public static DateTime ToDateTime(double unixTimeFormat)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeFormat).ToLocalTime();
            return dtDateTime;
        }

        public static double ToUnixTimeFormat(DateTime dateTime)
        {
            TimeSpan span = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(span.TotalSeconds);
        }
    }
}
