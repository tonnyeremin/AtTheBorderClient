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

namespace Onboard.Controls
{
    public partial class IconButton : Button
    {
        public IconButton()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string FirstLine
        {
            get { return (string)GetValue(FirstLineProperty); }
            set { SetValue(FirstLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FirstLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FirstLineProperty =
            DependencyProperty.Register("FirstLine", typeof(string), typeof(IconButton), new PropertyMetadata(""));



        public string SecondLine
        {
            get { return (string)GetValue(SecondLineProperty); }
            set { SetValue(SecondLineProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SecondLine.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SecondLineProperty =
            DependencyProperty.Register("SecondLine", typeof(string), typeof(IconButton), new PropertyMetadata(""));


        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(IconButton), new PropertyMetadata(null));

        protected override void OnClick()
        {
            base.OnClick();
            if (Command != null)
                Command.Execute(CommandParameter);
        }

      
    }
}
