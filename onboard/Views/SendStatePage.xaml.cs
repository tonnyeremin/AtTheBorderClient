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
using Onboard.ViewModels;

namespace Onboard.Views
{
    public partial class SendStatePage : PhoneApplicationPage
    {
        public SendStatePage()
        {
            InitializeComponent();
            Model = new SendViewModel();
            DataContext = Model;
        }

        private static SendViewModel _Model;

        public static SendViewModel Model
        {
            get { return _Model; }
            set { _Model = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

      
    }
}