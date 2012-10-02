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
    public class AboutViewModel : INotifyPropertyChanged
    {
        public AboutViewModel()
        {
            _AppReviewCommand = new DelegateCommand(new Action<object>(RunAppReview));
            _SendFeedbackCommand = new DelegateCommand(new Action<object>(SendFeedback));
            _GoSocialCommand = new DelegateCommand(new Action<object>(GoToSocialPage));
        }

        private DelegateCommand _AppReviewCommand;
        public ICommand AppReviewCommand
        {
            get { return _AppReviewCommand; }
        }

        private void RunAppReview(object param)
        {
            Microsoft.Phone.Tasks.MarketplaceReviewTask task = new Microsoft.Phone.Tasks.MarketplaceReviewTask();
            task.Show();
        }

        private DelegateCommand _SendFeedbackCommand;
        public ICommand SendFeedbackCommand
        {
            get { return _SendFeedbackCommand; }
        }

        private void SendFeedback(object param)
        {
            Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
            email.To = Constants.SUPPORT_EMAIL;
            email.Subject = Resources.Resource.SendRequest;
            email.Show();
        }

        private DelegateCommand _GoSocialCommand;
        public ICommand GoSocialCommand
        {
            get { return _GoSocialCommand; }
        }

        private void GoToSocialPage(object param)
        {
            string URI = param.ToString();
            Microsoft.Phone.Tasks.WebBrowserTask task = new Microsoft.Phone.Tasks.WebBrowserTask();
            task.Uri = new Uri(URI);
            task.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
