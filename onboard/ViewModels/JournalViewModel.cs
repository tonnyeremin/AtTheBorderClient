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

namespace Onboard.ViewModels
{
    public class JournalViewModel : INotifyPropertyChanged
    {
        public JournalViewModel()
        {
          
        }

        void Client_OnJournalUpdated(object sender, JournalEventArgs e)
        {
            Items = e.JournalItems;
            NotifyPropertyChanged("Items");
        }

        #region Public Fields
        public List<JournalItemInfo> Items
        {
            get
            {
                if (_Items == null)
                    _Items = new List<JournalItemInfo>();
                return _Items;
            }
            set
            {
                _Items = value;
                NotifyPropertyChanged("Items");

            }
        }
        #endregion
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

        private List<JournalItemInfo> _Items;

    }
}
