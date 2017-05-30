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

namespace BeijingTest2
{
    public partial class Introduce : PhoneApplicationPage
    {
        public Introduce()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!(Application.Current as App).IsContinueOldGame)
            {
                this.LeftButton.IsEnabled = false;
                this.LeftButton.Content = null;
                this.LeftButton.Opacity = 0;
            }
        }
        private void Click_Continue(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?Param=Continue", UriKind.Relative));
            (Application.Current as App).IsFromIntroduce = true;
        }
        private void Click_NewGame(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml?Param=NewGame", UriKind.Relative));
            (Application.Current as App).IsFromIntroduce = true;
        }
    }
}