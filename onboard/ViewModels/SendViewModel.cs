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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Onboard.ViewModels
{
    public class SendViewModel : INotifyPropertyChanged
    {
        public SendViewModel()
        {
            Directions.Add("from Russia");
            Directions.Add("to Russia");
            ControlPoints.Add("Torfynovka");
            ControlPoints.Add("Brushichoe");
            ControlPoints.Add("Svetogorsk");
            _SelectedDirection = "from Russia";
            _SelectedControlPoint = "Torfynovka";
            _Comment = "";
            Lenght = 0;
            Number = 0;
            try
            {
                Author = (string)System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["Author"];
            }
            catch
            {
                Author = "";
            }

            _PostDataCommand = new DelegateCommand(new Action<object>(PostData));
        }

        private ObservableCollection<string> _Directions;

        public ObservableCollection<string> Directions
        {
            get
            {
                if (_Directions == null)
                    _Directions = new ObservableCollection<string>();
                return _Directions;
            }
            set { _Directions = value; }
        }
        private ObservableCollection<string> _ControlPoints;

        public ObservableCollection<string> ControlPoints
        {
            get
            {
                if (_ControlPoints == null)
                    _ControlPoints = new ObservableCollection<string>();
                return _ControlPoints;
            }
            set
            {
                _ControlPoints = value;
                NotifyPropertyChanged("ControlPoints");
            }
        }
        private string _SelectedDirection;

        public string SelectedDirection
        {
            get { return _SelectedDirection; }
            set
            {
                _SelectedDirection = value;
                NotifyPropertyChanged("SelectedDirection");
            }
        }
        private string _SelectedControlPoint;

        public string SelectedControlPoint
        {
            get { return _SelectedControlPoint; }
            set
            {
                _SelectedControlPoint = value;
                NotifyPropertyChanged("SelectedControlPoint");
            }
        }
        private int _Number;

        public int Number
        {
            get { return _Number; }
            set
            {
                _Number = value;
                NotifyPropertyChanged("Number");
            }
        }

        private int _Lenght;

        public int Lenght
        {
            get { return _Lenght; }
            set
            {
                _Lenght = value;
                NotifyPropertyChanged("Lenght");
            }
        }
        private string _Comment;

        public string Comment
        {
            get { return _Comment; }
            set
            {
                _Comment = value;
                NotifyPropertyChanged("Comment");
            }
        }
        private string _Author;

        public string Author
        {
            get { return _Author; }
            set
            {
                _Author = value;
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings["Author"] = value;
                NotifyPropertyChanged("Author");
            }
        }

        private ICommand _PostDataCommand;

        public ICommand PostDataCommand
        {
            get { return _PostDataCommand; }
            set { _PostDataCommand = value; }
        }

        private void PostData(object parameters)
        {
            string dir = _Directions.IndexOf(_SelectedDirection).ToString();
            string cp = _ControlPoints.IndexOf(_SelectedControlPoint).ToString();
            string num = "";
            if (_Number > 0)
                num = _Number.ToString();
            string lenght = "";
            if (_Lenght > 0)
                lenght = _Lenght.ToString();
            App.Client.PostMessage(dir, cp, num, lenght, _Comment, _Author);
        }

        #region INotifyPropertyChanged Members

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
