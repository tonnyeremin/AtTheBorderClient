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
using System.Windows.Threading;
using System.Collections;
using Microsoft.Phone.Controls;

namespace Onboard
{
    public partial class ExtendedListBox : UserControl
    {
        private const string CompressionTopState = "CompressionTop";
        private const string CompressionBottomState = "CompressionBottom";
        private const string NoVerticalCompressionState = "NoVerticalCompression";
        private const string VerticalCompressionGroupName = "VerticalCompression";
        private const int PullDownInterval = 1;

        private ScrollViewer _scrollViewer = null;
        private bool _alreadyHookedScrollEvents = false;
        private DispatcherTimer _addToHeadTimer;
        private bool _refresh = false;

        public ExtendedListBox()
        {
            InitializeComponent();
            this.UnderlyingListBox.SelectionChanged += UnderlyingListBoxOnSelectionChanged;
        }

        public ICommand SelectionChanged
        {
            get { return (ICommand)GetValue(SelectionChangedProperty); }
            set { SetValue(SelectionChangedProperty, value); }
        }

        bool localUpdate = false;

        public static readonly DependencyProperty SelectionChangedProperty =
            DependencyProperty.Register("SelectionChanged", typeof(ICommand), typeof(ExtendedListBox), new PropertyMetadata(null));

        private void UnderlyingListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (!localUpdate)
            {
                var command = SelectionChanged;
                if (command == null)
                    return;

                command.Execute(UnderlyingListBox.SelectedItem);
                localUpdate = true;
                UnderlyingListBox.SelectedItem = null;
                localUpdate = false;
            }
        }

        public ICommand LoadTail
        {
            get { return (ICommand)GetValue(LoadTailProperty); }
            set { SetValue(LoadTailProperty, value); }
        }

        public static readonly DependencyProperty LoadTailProperty =
            DependencyProperty.Register("LoadTail", typeof(ICommand), typeof(ExtendedListBox), new PropertyMetadata(null));

        public ICommand LoadHead
        {
            get { return (ICommand)GetValue(LoadHeadProperty); }
            set { SetValue(LoadHeadProperty, value); }
        }

        public static readonly DependencyProperty LoadHeadProperty =
            DependencyProperty.Register("LoadHead", typeof(ICommand), typeof(ExtendedListBox), new PropertyMetadata(null));



        public ICommand TapItem
        {
            get { return (ICommand)GetValue(TapItemProperty); }
            set { SetValue(TapItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TapItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TapItemProperty =
            DependencyProperty.Register("TapItem", typeof(ICommand), typeof(ExtendedListBox), new PropertyMetadata(null));

        

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ExtendedListBox), new PropertyMetadata(null, OnItemsSourceChanged));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ExtendedListBox), new PropertyMetadata(null, OnItemTemplateChanged));



        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemContainerStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ExtendedListBox), new PropertyMetadata(null, OnItemContainerStyleChanged));

        


        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = (ExtendedListBox)d;
            parent.UnderlyingListBox.ItemsSource = (IEnumerable)e.NewValue;
        }

        private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = (ExtendedListBox)d;

            parent.UnderlyingListBox.ItemTemplate = (DataTemplate)e.NewValue;
        }

        private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var parent = (ExtendedListBox)d;

            parent.UnderlyingListBox.ItemContainerStyle = (Style)e.NewValue;
        }

        private void listBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (_alreadyHookedScrollEvents)
                return;

            _alreadyHookedScrollEvents = true;

            _scrollViewer = (ScrollViewer)FindElementRecursive(UnderlyingListBox, typeof(ScrollViewer));

            if (_scrollViewer != null)
            {
                // Visual States are always on the first child of the control template 
                FrameworkElement element = VisualTreeHelper.GetChild(_scrollViewer, 0) as FrameworkElement;
                if (element != null)
                {
                    VisualStateGroup verticalGroup = FindVisualState(element, VerticalCompressionGroupName);

                    if (verticalGroup != null)
                        verticalGroup.CurrentStateChanging += VerticalGroupCurrentStateChanging;
                }
            }

        }

        private void VerticalGroupCurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            if (e.NewState.Name == CompressionTopState)
            {
                _addToHeadTimer = new DispatcherTimer();
                _refresh = false;
                _addToHeadTimer.Interval = TimeSpan.FromSeconds(PullDownInterval);
                _addToHeadTimer.Tick += (s, ea) => { _refresh = true; textBlockAddToHead.Text = Onboard.Resources.Resource.Release ; };
                _addToHeadTimer.Start();

                textBlockAddToHead.Text = Onboard.Resources.Resource.HoldText;
                stackPanelAddToHead.Visibility = System.Windows.Visibility.Visible;
            }

            if (e.NewState.Name == CompressionBottomState)
                if (LoadTail != null &&
                    LoadTail.CanExecute(ItemsSource))
                    LoadTail.Execute(ItemsSource);

            if (e.NewState.Name == NoVerticalCompressionState)
            {
                stackPanelAddToHead.Visibility = System.Windows.Visibility.Collapsed;

                if (_addToHeadTimer != null &&
                    _addToHeadTimer.IsEnabled)
                {
                    _addToHeadTimer.Stop();

                    if (_refresh &&
                        LoadHead != null &&
                        LoadHead.CanExecute(ItemsSource))
                    {
                        _refresh = false;
                        LoadHead.Execute(ItemsSource);
                    }
                }
            }
        }

        private static UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;

            if (childCount > 0)
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                        return element as UIElement;
                    else
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                }

            return returnElement;
        }

        private static VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            var groups = VisualStateManager.GetVisualStateGroups(element);

            foreach (VisualStateGroup group in groups)
                if (group.Name == name)
                    return group;

            return null;
        }
    }
}
