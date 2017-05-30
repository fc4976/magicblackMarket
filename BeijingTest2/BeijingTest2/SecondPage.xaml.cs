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
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace BeijingTest2
{
    public partial class SecondPage : PhoneApplicationPage
    {
        string content;
        private MyPopup myPopup;
        private int amount;
        private Commodity commodity { get; set; }
        private Commodity _commodity { get; set; }
        private Commodity stupid { get; set; }

        public SecondPage()
        {
            InitializeComponent();
            Loaded += SecondPage_Loaded;
        }
        private void SecondPage_Loaded(object sender, RoutedEventArgs e)
        {            
            SalesViewer.ItemsSource = (Application.Current as App).SaleCommodities;
            MyViewer.ItemsSource = (Application.Current as App).MyCommodities;
            StackProperty.DataContext = (Application.Current as App).Property;
        }
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {

            IDictionary<string, string> parameters = this.NavigationContext.QueryString;
            if (parameters.ContainsKey("Name"))
            {
                content = parameters["Name"];
                SaleCommodityStore saleCommodityStore = new SaleCommodityStore(content);
                (Application.Current as App).SaleCommodities = saleCommodityStore.CommodityStore.Commodities;
                (Application.Current as App).MyincidentList.IncidentStart();
            }
            base.OnNavigatedTo(args);
        }
        private void Click_Buy(object sender, RoutedEventArgs e)
        {
            commodity = SalesViewer.SelectedItem as Commodity;
            if (commodity != null)
            {
                if (myPopup != null)
                {
                    if (LayoutRootCanvas.Children.Contains(myPopup))
                    {
                        LayoutRootCanvas.Children.Remove(myPopup);
                    }
                }
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Buy_OnLoaded);
                myPopup.LeftEvent += new MyPopup.LeftDelegate(Buy_Cancel);
                myPopup.RightEvent += new MyPopup.RightDelegate(Buy_Confirm);
                OpenPopup();
            }
            else
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("Please select first!");
            }
        }
        private void Buy_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.LeftButton.IsEnabled = true;
            myPopup.LeftButton.Opacity = 1;
            myPopup.LeftButton.Content = "Cancel";
            myPopup.RightButton.Content = "Confirm";
            int leftspace=0;
            int sum = 0;
            int budget = (Int32)((Application.Current as App).Property.Cash / commodity.Price);
            foreach (Commodity i in (Application.Current as App).MyCommodities)
            {
                sum += i.Amount;
            }
            if (sum <= (Application.Current as App).Property.Space)
                leftspace = (Application.Current as App).Property.Space - sum;
            else
                throw (new ArgumentOutOfRangeException("sum: <=Space"));
            amount = Math.Min(leftspace, budget);
            Int64 cash = (Application.Current as App).Property.Cash;
            myPopup.Reminder.Text = String.Format("Now you have {0} G,", cash) + String.Format(" can buy {0} {1}!", amount, commodity.Name); 
            myPopup.Question.Text = "How many to buy？";
            myPopup.Input.IsEnabled = true;
            myPopup.Input.Opacity = 1;
            myPopup.Input.DefaultText = amount.ToString();
        }
        private void Buy_Confirm(object sender, RoutedEventArgs e)
        {
            int num;
            if (Int32.TryParse(myPopup.Input.Text, out num))
            {
                int i=0;
                if ((0 < num) && (num <= amount))
                {
                    Int64 cost = num * commodity.Price;
                    if ((Application.Current as App).Property.Cash >= cost)
                    {
                        (Application.Current as App).Property.Cash -= cost;
                        for (i = 0; i < (Application.Current as App).MyCommodities.Count; i++)
                        {
                            if ((Application.Current as App).MyCommodities[i].Name == commodity.Name)
                            {
                                Int64 OldSum = (Application.Current as App).MyCommodities[i].Amount * (Application.Current as App).MyCommodities[i].Price;
                                (Application.Current as App).MyCommodities[i].Amount += num;
                                (Application.Current as App).MyCommodities[i].Price = Convert.ToInt32(((OldSum + cost) / 
                                    Convert.ToInt64((Application.Current as App).MyCommodities[i].Amount)));
                                break;
                            }
                        }
                        if (i == (Application.Current as App).MyCommodities.Count)
                        {
                            Commodity temp = new Commodity(commodity);
                            temp.Amount = num;
                            (Application.Current as App).MyCommodities.Add(temp);
                        }
                        ClosePopup();
                    }
                    else
                        throw (new ArgumentOutOfRangeException("Impossible"));
                }
                else if (num == 0)
                {
                    ClosePopup();
                }
                else if (num > amount)
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Not enough cash or space！");
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input a number！");
                }
            }
            else
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("Please input a number！");
            }
        }
        private void Buy_Cancel(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
        private void Click_Sale(object sender, RoutedEventArgs e)
        {
            _commodity = MyViewer.SelectedItem as Commodity;
            if (_commodity != null)
            {
                bool Isfound = false;
                foreach (Commodity temp in (Application.Current as App).SaleCommodities)
                {
                    if (temp.Name == _commodity.Name)
                    {
                        Isfound = true;
                        break;
                    }
                }
                if (Isfound)
                {
                    if (myPopup != null)
                    {
                        if (LayoutRootCanvas.Children.Contains(myPopup))
                        {
                            LayoutRootCanvas.Children.Remove(myPopup);
                        }
                    }
                    myPopup = new MyPopup();
                    myPopup.Loaded += new RoutedEventHandler(Sale_OnLoaded);
                    myPopup.LeftEvent += new MyPopup.LeftDelegate(Sale_Cancel);
                    myPopup.RightEvent += new MyPopup.RightDelegate(Sale_Confirm);
                    OpenPopup();
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("You can't sell it!");
                }
            }
            else
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("What do you want to sell?");
            }
        }
        private void Sale_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.LeftButton.IsEnabled = true;
            myPopup.LeftButton.Opacity = 1;
            myPopup.LeftButton.Content = "Cancel";
            myPopup.RightButton.Content = "Confirm";
            myPopup.Reminder.Text = String.Format("You can sell {0} {1}", _commodity.Amount, _commodity.Name);
            myPopup.Question.Text = "How many to sell?";
            myPopup.Input.IsEnabled = true;
            myPopup.Input.Opacity = 1;
            myPopup.Input.DefaultText = _commodity.Amount.ToString();
        }
        private void Sale_Confirm(object sender, RoutedEventArgs e)
        {
            int num;
            if (Int32.TryParse(myPopup.Input.Text, out num))
            {
                if ((0 < num) && (num <= _commodity.Amount))
                {
                    Int64 Cost = 0;
                    foreach (Commodity i in (Application.Current as App).SaleCommodities)
                    {
                        if (i.Name == _commodity.Name)
                        {
                            Cost = num * i.Price;
                            break;
                        }
                    }
                    
                    (Application.Current as App).Property.Cash += Cost;
                    _commodity.Amount -= num;

                    if (_commodity.Amount <= 0)
                    {
                        (Application.Current as App).MyCommodities.Remove(_commodity);
                    }
                    if (_commodity.Name == "Invisibility Cloak")
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("You sold the invisibility cloak, fame decrease by 10 points!");
                        (Application.Current as App).Property.Reputation -= 10;
                    }
                    else if (_commodity.Name == "Horn of The Unicorn")
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("You sold the horn of the unicorn, fame decrease by 5 points!");
                        (Application.Current as App).Property.Reputation -= 5;
                    }
                    else if (_commodity.Name == "Baby Dragon")
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("You sold the baby dragon, fame decrease by 5 points!");
                        (Application.Current as App).Property.Reputation -= 5;
                    }
                    ClosePopup();
                }
                else if (num == 0)
                {
                    ClosePopup();
                }
                else if (num > _commodity.Amount)
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show(String.Format("You only have {0} {1}!", _commodity.Amount, _commodity.Name));
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input a number！");
                }

            }
            else
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("Please input a number！");
            }
        }
        private void Sale_Cancel(object sender, RoutedEventArgs e)
        {
            ClosePopup();
        }
        //卖东西结束
        private void Click_Return(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
            (Application.Current as App).IsFromIntroduce = false;
        }
        /*
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            NavigationService.GoBack();
            (Application.Current as App).IsFromIntroduce = false;
        }*/
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
            var rootVisual = MyMessageBox.FindRootVisual();
            //+Fu Cong
            if (rootVisual.Children.Count > 1)
            {
                for (var a = rootVisual.Children.Last(); rootVisual.Children.Count > 1; a = rootVisual.Children.Last())
                {
                    if (a.GetType() == typeof(Grid))
                        rootVisual.Children.Remove(a);
                }
            }
            //-Fu Cong
            else if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
                e.Cancel = true;
            }
            else
            {
                NavigationService.GoBack();
                (Application.Current as App).IsFromIntroduce = false;
            }
        }
        private void OpenPopup()
        {
            if (myPopup != null)
            {
                Canvas.SetZIndex(myPopup, 1);
                Canvas.SetLeft(myPopup, LayoutRootCanvas.ActualWidth / 2);
                Canvas.SetTop(myPopup, ContentPanel.ActualHeight / 2);
                Storyboard a = myPopup.Resources["LoadStoryboard"] as Storyboard;
                LayoutRootCanvas.Children.Add(myPopup);
                a.Begin();
                a.Completed += (s, e1) =>
                {
                    a.Stop();
                    Canvas.SetLeft(myPopup, LayoutRootCanvas.ActualWidth / 2 - myPopup.BorderRoot.ActualWidth / 2);
                    Canvas.SetTop(myPopup, ContentPanel.ActualHeight / 2 - myPopup.BorderRoot.ActualHeight / 2);
                };
            }
        }
        private void ClosePopup()
        {
            if (myPopup != null)
            {
                if (LayoutRootCanvas.Children.Contains(myPopup))
                {
                    Storyboard a = myPopup.Resources["UnloadStoryboard"] as Storyboard;
                    a.Begin();
                    a.Completed += (s, e1) =>
                    {
                        a.Stop();
                        LayoutRootCanvas.Children.Remove(myPopup);
                    };
                }               
            }
        }
        
        public void cleanMessageBox()
        {
            PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
            PhoneApplicationFrame RootFrame = MyMessageBox.RootFrame;
            PhoneApplicationPage _page = MyMessageBox.CurrentPage;
            var rootVisual = MyMessageBox.FindRootVisual();
            /*
            PhoneApplicationPage _page = null;
            PhoneApplicationFrame RootFrame = Application.Current.RootVisual as PhoneApplicationFrame;
            if ((null == _page) &&
                    (null != RootFrame))
            {
                _page = RootFrame.GetVisualDescendants().OfType<PhoneApplicationPage>().FirstOrDefault();
                if (null == _page)
                {
                    _page = RootFrame.Content as PhoneApplicationPage;
                }
            }*/

        }
        /*************************The test code for drag********************************
        /*
        Random rand = new Random();
        bool isDrawing, isDragging;
        Path path;
        EllipseGeometry ellipseGeo;
        protected override void OnManipulationStarted(ManipulationStartedEventArgs args) 
        { 
            if (isDrawing || isDragging) return;
            if (args.OriginalSource is Path) 
            { 
                ellipseGeo = (args.OriginalSource as Path).Data as EllipseGeometry;
                isDragging = true;
                args.ManipulationContainer = ContentPanel;
                args.Handled = true;
            }
            else if (args.OriginalSource == ContentPanel)
            {
                ellipseGeo = new EllipseGeometry();
                ellipseGeo.Center = args.ManipulationOrigin;
                path = new Path();
                path.Stroke = this.Resources["PhoneForegroundBrush"] as Brush;
                path.Data = ellipseGeo;
                ContentPanel.Children.Add(path);
                isDrawing = true;
                args.Handled = true;
            }
            base.OnManipulationStarted(args);
        }
        protected override void OnManipulationDelta(ManipulationDeltaEventArgs args)
        {
            if (isDragging)
            {
                Point center = ellipseGeo.Center;
                center.X += args.DeltaManipulation.Translation.X;
                center.Y += args.DeltaManipulation.Translation.Y;
                ellipseGeo.Center = center;
                args.Handled = true;
            }
            else if (isDrawing)
            {
                Point translation = args.CumulativeManipulation.Translation; double radius = Math.Max(Math.Abs(translation.X),
                Math.Abs(translation.Y));
                ellipseGeo.RadiusX = radius;
                ellipseGeo.RadiusY = radius;
                args.Handled = true;
            }
            base.OnManipulationDelta(args);
        }
        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs args)
        {
            if (isDragging)
            {
                isDragging = false;
                args.Handled = true;
            }
            else if (isDrawing)
            {
                Color clr = Color.FromArgb(255, (byte)rand.Next(256),
                (byte)rand.Next(256),
                (byte)rand.Next(256));
                path.Fill = new SolidColorBrush(clr);
                isDrawing = false;
                args.Handled = true;
            }
            base.OnManipulationCompleted(args);
        }
         */
    }
}