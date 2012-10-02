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
using System.ComponentModel;

namespace Onboard.ViewModels
{
    public class SendModel : INotifyPropertyChanged
    {
        public SendModel()
        {
            _SelectedCountry = 0;
            _SelectedPost = 0;
        }
        private int _SelectedCountry =0;

        public int SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                _SelectedCountry = value;
                NotifyPropertyChanged("SelectedCountry");
            }
        }
        private int _SelectedPost =0;

        public int SelectedPost
        {
            get { return _SelectedPost; }
            set
            {
                _SelectedPost = value;
                NotifyPropertyChanged("SelectedPost");
            }
        }
        private int _CarNums;

        public int CarNums
        {
            get { return _CarNums; }
            set
            {
                _CarNums = value;
                NotifyPropertyChanged("CarNums");
            }
        }
        private string _Message;

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                NotifyPropertyChanged("Message");
            }
        }
        private string _Author;

        public string Author
        {
            get { return _Author; }
            set
            {
                _Author = value;
                NotifyPropertyChanged("Author");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
