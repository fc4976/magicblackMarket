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
    public partial class RankingList : PhoneApplicationPage
    {
        public RankingList()
        {
            InitializeComponent();
            Loaded += OnLoaded; 
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            RankingShowListClass abc = new RankingShowListClass();
            abc.Load();
            abc.Sort();
            RankingListListBox.ItemsSource = abc.RankingShowList;
        }
        private void OnDoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            NavigationService.GoBack();
            (Application.Current as App).IsFromIntroduce = false;
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.GoBack();
            (Application.Current as App).IsFromIntroduce = false;
        }
    }
}