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
using System.Windows.Data;
using Microsoft.Expression.Media;

namespace Onboard
{
    public class DerivativeIconSourceValueConverters : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int derivative = (int)value;
            ArrowOrientation returnValue = ArrowOrientation.Down;

            if (derivative > 0)
                returnValue = ArrowOrientation.Up;
            else if (derivative < 0)
                returnValue = ArrowOrientation.Down;

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class ArrowVisibilityValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int derivative = (int)value;
            Visibility returnValue = Visibility.Visible;

            if (derivative == 0)
                returnValue = Visibility.Collapsed;

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class TimeValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double d = (double)value;
            DateTime dt = Utils.ToDateTime(d);
            string time = "";
            if (dt.Date == DateTime.Now.Date)
                time = dt.TimeOfDay.ToString();
            else 
                time =  Onboard.Resources.Resource.ResourceManager.GetString(dt.DayOfWeek.ToString()) + ", " + 
                    dt.TimeOfDay.ToString();
            
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class WaitingTimeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int waiting = (int)value;
            double h = waiting / 60;
            if (h > 0)
                return string.Format("{0} {1} {2} {3}",
                    h.ToString(), Onboard.Resources.Resource.Hour,
                    (waiting - (int)h * 60).ToString(),
                    Onboard.Resources.Resource.Min);
            else
            {
                return string.Format("{0} {1}", waiting,
                    Onboard.Resources.Resource.Min);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class DirectionValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = (string)value;
            string retVal = "from Russia";
            if (s == "0")
                retVal = "from Russia";
            else if (s == "1")
                retVal = "to Russia";
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class ControlPointValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = (string)value;
            string retVal = "from Russia";
            if (s == "1")
                retVal = "Trofyanovka";
            else if (s == "2")
                retVal = "Brushichnoe";
            else if (s == "3")
                retVal = "Svetogorsk";
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    public class PointNameTranslateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = "Warning value not set";
            if (value != null)
            {
                CrossingPointName name = (CrossingPointName)value;
                switch (name)
                {
                    case CrossingPointName.Brusnichnoe:
                        str = Resources.Resource.Brusnichnoe;
                        break;
                    case CrossingPointName.Svetogorsk:
                         str = Resources.Resource.Svetogorsk;
                        break;
                    case CrossingPointName.Torfynovka:
                        str = Resources.Resource.Torfynovka;
                        break;
                }
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class DirectionNameTranslateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = "Warning value not set";
            if (value != null)
            {
                CrossingDirection name = (CrossingDirection)value;
                switch (name)
                {
                    case CrossingDirection.FromRussia:
                        str = Onboard.Resources.Resource.FromRussiaTabHeader;
                        break;
                    case CrossingDirection.ToRussia:
                        str = Onboard.Resources.Resource.ToRussiaTabHeader;
                        break;
                }
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class DistanceValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string distance = "0 м";
            if (value != null)
            {
                double d_value = (double)value;
                //if (d_value > 10000)
                //    distance = Resources.Localization.ToFar;
                //else
                    distance = d_value.ToString("F0") + " " + "м";
            }
            return distance;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
