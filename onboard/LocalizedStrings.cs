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
using Onboard.Resources;

namespace Onboard
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {

        }

        private static Onboard.Resources.Resource _Resources = new Onboard.Resources.Resource();

        public static Onboard.Resources.Resource Resources
        {
            get { return _Resources; }
        }
    }
}
