using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Onboard
{
    public partial class InfoPage : PhoneApplicationPage
    {
        public InfoPage()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            _GeoService = new GeoService();
            _GeoService.Start(App.ViewModel.SelectedPoint);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (_GeoService != null)
            {
                _GeoService.Stop();
                _GeoService = null;
            }
        }

        private GeoService _GeoService;

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            App.ViewModel.PostMessage(App.ViewModel.SelectedPoint.Info.Comment, App.ViewModel.Author);
        }
    }
}