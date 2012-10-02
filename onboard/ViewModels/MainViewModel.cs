using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Windows.Threading;
using System.Windows.Navigation;
using Onboard.ViewModels;


namespace Onboard
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            if (DesignerProperties.IsInDesignTool)
            {
                ToRussia = new List<CrossingPoint>();
                FromRussia = new List<CrossingPoint>();
                CrossingPoint point = new CrossingPoint();
                point.Info = new CrossingPointInfo()
                {
                    Author = "Author",
                    Changes = -3,
                    Comment = "Comment",
                    Date = Utils.ToUnixTimeFormat(DateTime.Now),
                    Number = "5",
                    Url = "url",
                    Waiting = 10
                };
                point.Name = CrossingPointName.Brusnichnoe;
                ToRussia.Add(point);
                FromRussia.Add(point);

                _Journal = new List<JournalItemInfo>();
                JournalItemInfo info = new JournalItemInfo();
                info.Direction = CrossingDirection.ToRussia;
                info.Station = CrossingPointName.Brusnichnoe;
                info.Comment = "Comment";
                info.Date = Utils.ToUnixTimeFormat(DateTime.Now);
                info.Author = "Author";
                _Journal.Add(info);
                _IsUpdating = true;
            }

            _UpdateCommand = new DelegateCommand(new Action<object>(Update));
            _UpdateTrafficCommand = new DelegateCommand(new Action<object>(UpdateTraffic));
            _UpdateJournalCommand = new DelegateCommand(new Action<object>(UpdateJournal));
            _GetPrevious = new DelegateCommand(new Action<object>(GetJoutnalPrevious));
            _SendCommand = new DelegateCommand(new Action<object>(Send));

        }

        private AboutViewModel _About;

        public AboutViewModel About
        {
            get
            {
                if (_About == null)
                    _About = new AboutViewModel();
                return _About;
            }
        }

        private int _SelectedCountry = 0;

        public int SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                _SelectedCountry = value;
                NotifyPropertyChanged("SelectedCountry");
            }
        }
        private int _SelectedPost = 0;

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

        #region Public Properties
        private List<CrossingPoint> _ToRussia;
        /// <summary>
        /// 
        /// </summary>
        public List<CrossingPoint> ToRussia
        {
            get
            {
                return _ToRussia;
            }
            set
            {
                _ToRussia = value;
                NotifyPropertyChanged("ToRussia");
            }
        }
        private List<CrossingPoint> _FromRussia;
        /// <summary>
        /// 
        /// </summary>
        public List<CrossingPoint> FromRussia
        {
            get
            {
                return _FromRussia;
            }
            set
            {
                _FromRussia = value;
                NotifyPropertyChanged("FromRussia");
            }
        }
        private List<JournalItemInfo> _Journal;
        /// <summary>
        /// 
        /// </summary>
        public List<JournalItemInfo> Journal
        {
            get
            {
                return _Journal;
            }
            set
            {
                _Journal = value;
                NotifyPropertyChanged("Journal");
            }
        }

        private bool _IsUpdating;

        public bool IsUpdating
        {
            get { return _IsUpdating; }
            set
            {
                _IsUpdating = value;
                NotifyPropertyChanged("IsUpdating");
            }
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void SaveData()
        {
            XmlDataWrapper wrapper = new XmlDataWrapper(this);
            StorageHelper.SaveData(wrapper);
        }

        public void LoadData()
        {
            XmlDataWrapper data = StorageHelper.LoadData();
            bool needUpdate = false;
            if (data != null)
            {
                FromRussia = data.FromRussia;
                needUpdate = _FromRussia.Count == 0;
                ToRussia = data.ToRussia;
                needUpdate = _ToRussia.Count == 0;
                Journal = data.Journal;
                needUpdate = _Journal.Count == 0;
                Author = data.Author;
                SelectedCountry = data.SelectedCountry;
                SelectedPost = data.SelectedCity;
            }
            else
            {
                needUpdate = true;
            }

            if (needUpdate)
            {
                IsUpdating = true;
                App.Client.Update();
            }
        }

        private void Update(object param)
        {
            IsUpdating = true;
            App.Client.Update();
        }

        private void UpdateTraffic(object param)
        {
            IsUpdating = true;
            App.Client.Update(UpdateMode.Traffic);
        }

        private void UpdateJournal(object param)
        {
            IsUpdating = true;
            App.Client.Update(UpdateMode.Journal);
        }

        private void GetJoutnalPrevious(object param)
        {
            int index = this._Journal.Count / 10;
            App.Client.GetJournal(index);
        }

        private void RunAppReview(object param)
        {
            Microsoft.Phone.Tasks.MarketplaceReviewTask task = new Microsoft.Phone.Tasks.MarketplaceReviewTask();
            task.Show();
        }

        private void Send(object param)
        {
            App.Client.SendInfo(this);
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

        #region Commands
        private DelegateCommand _UpdateCommand;
        public ICommand UpdateCommand
        {
            get { return _UpdateCommand; }
        }

        private DelegateCommand _UpdateTrafficCommand;
        public ICommand UpdateTrafficCommand
        {
            get { return _UpdateTrafficCommand; }
        }

        private DelegateCommand _UpdateJournalCommand;
        public ICommand UpdateJournalCommand
        {
            get { return _UpdateJournalCommand; }
        }

        private DelegateCommand _ViewInfoCommand;
        public ICommand ViewInfoCommand
        {
            get { return _ViewInfoCommand; }
        }

        private DelegateCommand _GetPrevious;
        public ICommand GetPrevious
        {
            get { return _GetPrevious; }
        }

        private DelegateCommand _SelectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get { return _SelectionChangedCommand; }
        }

        private DelegateCommand _SendCommand;
        public ICommand SendCommand
        {
            get { return _SendCommand; }
        }



        #endregion


    }

}