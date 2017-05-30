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
using System.Windows.Data;
using System.Globalization;
using System.Windows.Threading;
using System.Threading;
using System.Windows.Navigation;
using System.Windows.Media.Imaging;
//using System.Windows.Controls.Primitives;

namespace BeijingTest2
{
    public partial class MainPage : PhoneApplicationPage
    {
        private List<Button> ButtonList=new List<Button>();
        private Int64 cost;
        private MyPopup myPopup;
        private Int32 HospitalPoints;
        private Int64 LotteryNumber;
        private Int64 UsuryRepay;
        private Int64 RentSpace;
        private DispatcherTimer timer;
        private Dictionary<string, Image> ImagesDictionary = new Dictionary<string, Image>();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            MainPageInit();
        }
        private void MainPageInit()
        {
            ButtonImagesDictionaryInit();
            //Start Shining animation
            {
                Shining.AutoReverse = true;
                Shining.RepeatBehavior = RepeatBehavior.Forever;
                Shining.Begin();
            }
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(30);
            timer.Tick += OnTimerTicker;
        }
        private void InitNews()
        {
            News.Text = (Application.Current as App).MyNewsList.NewsListShow();
            TranslateTransform translateTransform = News.RenderTransform as TranslateTransform;
            translateTransform.X = ActualWidth;
        }
        private void OnTimerTicker(object sender, EventArgs e)
        {
            if (News.Text != null)
            {
                TranslateTransform translateTransform = News.RenderTransform as TranslateTransform;
                translateTransform.X = translateTransform.X - 4d;
                if (translateTransform.X + News.ActualWidth < 6)
                    translateTransform.X = ActualWidth;
            }
        }

        //所有地名的button click
        private void Gzf_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Tty_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Tz_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Wj_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Hlg_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Dzm_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Pgy_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Zgc_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Cqk_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Cwm_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Yz_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Xzm_Button_Click(object sender, RoutedEventArgs e)
        {
            Button_Click(sender, e);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.GetType() == typeof(Image))
            {
                ProcessTwo();
                bool IsGameOver = ProcessOne();

                if (false == IsGameOver)
                {
                    string destination = "/SecondPage.xaml";
                    if (button.Name != null)
                        destination += String.Format("?Name={0}", button.Name);
                    this.NavigationService.Navigate(new Uri(destination, UriKind.Relative));
                }
            }
            else
            {
                Image image = null;
                ImagesDictionary.TryGetValue(button.Name, out image);
                if (image != null)
                {
                    image.Stretch = Stretch.Fill;
                    button.Content = image;
                }
                else
                    throw (new ArgumentOutOfRangeException("image is null"));
                if (button.Parent == canvas)
                {
                    canvas.Children.Remove(button);

                    foreach (FrameworkElement fe in canvas.Children)
                    {
                        if (fe.GetType() == typeof(Button))
                        {
                            (fe as Button).Visibility = Visibility.Collapsed;
                            (fe as Button).Opacity = 0;
                            (fe as Button).IsEnabled = false;
                        }
                    }

                    button.Opacity = 1;
                    button.IsEnabled = true;
                    Canvas.SetZIndex(button, 1);
                    (button.RenderTransform as CompositeTransform).ScaleX = 1;
                    (button.RenderTransform as CompositeTransform).ScaleY = 1;
                    button.Width = Double.NaN;
                    button.Height = Double.NaN;
                    MapGrid.Children.Add(button);
                }
                else
                    throw (new ArgumentOutOfRangeException("Button is not a child of Canvas"));
            }
        }
        void SellLeftGoods()
        {
            foreach (Commodity i in (Application.Current as App).MyCommodities)
            {
                (Application.Current as App).Property.Cash += i.Amount * i.Price;
                if (i.Name == "Invisibility Cloak")
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("You sold the invisibility cloak, fame decrease by 10 points!");
                    (Application.Current as App).Property.Reputation -= 10;
                }
                else if (i.Name == "Horn of The Unicorn")
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("You sold the horn of the unicorn, fame decrease by 5 points!");
                    (Application.Current as App).Property.Reputation -= 5;
                }
                else if (i.Name == "Baby Dragon")
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("You sold the baby dragon, fame decrease by 5 points!");
                    (Application.Current as App).Property.Reputation -= 5;
                }
            }
        }
        private bool ProcessOne()
        {
            (Application.Current as App).Property.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Property_PropertyChanged);
            if ((Application.Current as App).Property.Days <= 0)
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("Your time in magical world is up! The Ministry of Magic help to sell all your left.");
                SellLeftGoods();
                GameOver();
                return true;
            }
            else if ((Application.Current as App).Property.Health <= 0)
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("You have collapsed while crossing the street.The Ministry of Magic help to sell all your left.");
                SellLeftGoods();
                GameOver();
                return true;
            }
            else if ((Application.Current as App).Property.Reputation <= 0)
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("Considering your notorious reputation, you are no longer welcome here!The Ministry of Magic help to sell all your left.");
                SellLeftGoods();
                GameOver();
                return true;
            }
            else
                return false;
        }

        void Property_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyGrid.DataContext = (Application.Current as App).Property;
        }
        //在点“能消耗一天”的按钮时执行
        private void ProcessTwo()
        {
            (Application.Current as App).Property.Days--;
            //当健康值低于40，它就每天减少5点
            if ((Application.Current as App).Property.Health <= 40)
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("You are too weak, health value reduces 5 points each day!");
                (Application.Current as App).Property.Health -= 5;
            }
            //欠款增加20%
            if ((Application.Current as App).Property.Debt > 0)
                (Application.Current as App).Property.Debt += (Application.Current as App).Property.Debt / 5;
            //银行存款增加5%
            if ((Application.Current as App).Property.Deposit > 0)
                (Application.Current as App).Property.Deposit += (Application.Current as App).Property.Deposit / 20;
            
        }
        //+Game Over
        private RankingListNamePopup rankingListNamePopup;
        private RankingShowListClass rankingShowListClass;
        private RankingItemClass rankingItem;
        private bool InputNameIsNeed;
        private string PlayerName;
        void GameOver()
        {
            rankingShowListClass = new RankingShowListClass();
            rankingShowListClass.Load();
            if (rankingShowListClass != null)
            {
                if (rankingShowListClass.RankingShowList.Count>=10)
                {
                    if ((Application.Current as App).Property.Money * (Int64)(Application.Current as App).Property.Health * 
                        (Int64)(Application.Current as App).Property.Reputation/10000 > rankingShowListClass.RankingShowList.Last().SuccessValue)
                    {
                        InputNameIsNeed = true;
                    }
                    else
                    {
                        InputNameIsNeed = false;
                    }
                }
                else
                    InputNameIsNeed = true;
            }
            DisableButtonsInCanvas();
            DisableButtonsInIconGrid();
            myPopup = new MyPopup();
            if (InputNameIsNeed == true)
            {
                myPopup.Loaded += new RoutedEventHandler(RankingNameSave_OnLoaded);
                myPopup.LeftEvent += new MyPopup.LeftDelegate(RankingNameSave_Confirm);
                myPopup.RightEvent += new MyPopup.RightDelegate(RankingNameSave_Reset);
            }
            else
            {
                myPopup.Loaded += new RoutedEventHandler(RankingNameNoSave_OnLoaded);
                myPopup.RightEvent += new MyPopup.RightDelegate(RankingNameNoSave_Confirm);
            }
            myPopup.SetInputScope();
            OpenPopup();
        }
        private void RankingNameNoSave_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = "Ranking List";
            myPopup.LeftButton.IsEnabled = false;
            myPopup.LeftButton.Opacity = 0;
            myPopup.RightButton.IsEnabled = true;
            myPopup.RightButton.Opacity = 1;
            myPopup.RightButton.Content = "Confirm";
            myPopup.Reminder.Text = String.Format("Your score is {0}. Good luck next time!", (Application.Current as App).Property.Money *
                (Application.Current as App).Property.Health * (Application.Current as App).Property.Reputation/10000);
        }
        private void RankingNameNoSave_Confirm(object sender, RoutedEventArgs e)
        {
            //No need to use the ClosePopup function
            LayoutRootCanvas.Children.Remove(myPopup);
            //Even you are not in the rank list, the whole Game is also over, delete the status store file
            AppContextStoreRestoreClass.DeleteFile();
            (Application.Current as App).IsContinueOldGame = false;
            DoYouWantANewGameFunction();
        }
        private void RankingNameSave_OnLoaded(object sender, RoutedEventArgs e)
        {
            PlayerName = null;
            myPopup.Title.Text = "Ranking List";
            myPopup.LeftButton.IsEnabled = true;
            myPopup.LeftButton.Opacity = 1;
            myPopup.LeftButton.Content = "Confirm";
            myPopup.RightButton.Opacity = 1;
            myPopup.RightButton.Content = "Reset";
            rankingItem = rankingShowListClass.Add((Application.Current as App).Property, null);
            if (rankingItem != null)
            {
                rankingShowListClass.Sort();
                rankingShowListClass.Cut();
                myPopup.Reminder.Text = String.Format("Congratulations! Your score is {0}, num {1} in the list of best magic merchant！"
                    , rankingItem.SuccessValue, rankingShowListClass.RankingShowList.IndexOf(rankingItem) + 1);
                myPopup.Question.Text = String.Format("Your name:");
                myPopup.Input.IsReadOnly = true;
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                myPopup.Input.MouseLeftButtonDown += new MouseButtonEventHandler(Input_MouseLeftButtonDown);
            }
            else
            {
                throw (new ArgumentOutOfRangeException("The item is null!"));
            }
        }

        void Input_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point ClickPointer = e.GetPosition(LayoutRootCanvas);
            if (PlayerName == null)
            {
                OnNameInputClick(ClickPointer);
            }
            else if (PlayerName == "Create New Name")
            {
                myPopup.Input.IsReadOnly = false;
                myPopup.Input.Text = "";
                myPopup.Input.MouseLeftButtonDown -= new MouseButtonEventHandler(Input_MouseLeftButtonDown);
            }
            else
            {
                myPopup.Input.Text = PlayerName;
                myPopup.Input.MouseLeftButtonDown -= new MouseButtonEventHandler(Input_MouseLeftButtonDown);
            }
            
        }

        private void RankingNameSave_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.Text == "")
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show("Please input your name!");
            }
            else
            {
                rankingItem.Name = myPopup.Input.Text;
                rankingShowListClass.Save();
                //No need to use the ClosePopup function
                LayoutRootCanvas.Children.Remove(myPopup);
                //After the name is inserted in the rank list, whole Game is over, delete the status store file
                AppContextStoreRestoreClass.DeleteFile();
                (Application.Current as App).IsContinueOldGame = false;
                DoYouWantANewGameFunction();
            }
        }
        private void DoYouWantANewGameFunction()
        {
            this.NavigationService.Navigate(new Uri("/Introduce.xaml", UriKind.Relative));           
        }
        private void RankingNameSave_Reset(object sender, RoutedEventArgs e)
        {
            PlayerName = null;
            myPopup.Input.Text = "";
            myPopup.Input.IsReadOnly = true;
            myPopup.Input.MouseLeftButtonDown += new MouseButtonEventHandler(Input_MouseLeftButtonDown);
        }

        void OnNameInputClick(Point ClickPointer)
        {
            List<string> NameList = new List<string>();
            NameList.Add("Create New Name");
            NameList.Add("Muggle");
            NameList.Add("Harry Potter");
            if (null != rankingShowListClass)
            {
                foreach (RankingItemClass i in rankingShowListClass.RankingShowList)
                {
                    if (NameList.Contains(i.Name) == false)
                    {
                        NameList.Add(i.Name);
                    }
                }
            }
            if (rankingListNamePopup != null)
            {
                if (LayoutRootCanvas.Children.Contains(rankingListNamePopup))
                    LayoutRootCanvas.Children.Remove(rankingListNamePopup);
            }
            rankingListNamePopup = new RankingListNamePopup();
            Canvas.SetLeft(rankingListNamePopup, ClickPointer.X+10);
            Canvas.SetTop(rankingListNamePopup, ClickPointer.Y - rankingListNamePopup.Height/2);
            Canvas.SetZIndex(rankingListNamePopup, 2);
            LayoutRootCanvas.Children.Add(rankingListNamePopup);
            rankingListNamePopup.listBox.ItemsSource = NameList;          
            rankingListNamePopup.listBox.SelectionChanged += new SelectionChangedEventHandler(OnRankingListNameSelectionChanged);
        }
        void OnRankingListNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlayerName = rankingListNamePopup.listBox.SelectedItem as string;
            myPopup.Input.Text = PlayerName;
        }
        //-Game Over

        //---------------------按钮制作开始------------------------
        //医院开始
        private void Hospital_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
            }
            else
            {
                Hospitalsb.Begin();
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Hospital_OnLoaded);
                myPopup.RightEvent += new MyPopup.RightDelegate(Hospital_Confirm);
                OpenPopup();
            }
        }
        private void Hospital_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = "St. Mungo's Hospital";
            if ((Application.Current as App).Property.Health >= 100)
            {
                myPopup.Reminder.Text = "You are in good shape, no need to see a doctor :)";
            }
            else if ((Application.Current as App).Property.Health < 100 && (Application.Current as App).Property.Health >= 0)
            {
                myPopup.RightButton.Content = "Confirm";
                if ((Application.Current as App).Property.Health >= 80)
                    cost = Math.Max((Application.Current as App).Property.Money / 200, 100);//0.5%的钱，或100
                else if ((Application.Current as App).Property.Health >= 60)
                    cost = Math.Max(2 * (Application.Current as App).Property.Money / 200, 500);//1%的钱，或500
                else if ((Application.Current as App).Property.Health >= 40)
                    cost = Math.Max(3 * (Application.Current as App).Property.Money / 200, 1000);//1.5%的钱，或1000
                else if ((Application.Current as App).Property.Health >= 20)
                    cost = Math.Max(4 * (Application.Current as App).Property.Money / 200, 5000);//2%的钱，或5000
                else
                    cost = Math.Max(5 * (Application.Current as App).Property.Money / 200, 10000);//2.5%的钱，或10000
                HospitalPoints = (Int32)Math.Min((Application.Current as App).Property.Cash / cost, 100 - (Application.Current as App).Property.Health);
                myPopup.Reminder.Text = String.Format("It will take you {0} galleons to recover 1 health value. You have {1} galleons in cash, can recover {2} health values at most.", cost, (Application.Current as App).Property.Cash, HospitalPoints);
                myPopup.Question.Text = String.Format("How many to recover?");
                //the TextBox input
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                //myPopup.Input.Text = HospitalPoints.ToString();
                myPopup.Input.DefaultText = HospitalPoints.ToString();
            }
            else
            {
                throw (new ArgumentOutOfRangeException("Health: 0-100"));
            }
        }
        private void Hospital_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if ((0 <= num) && (num <= HospitalPoints))
                    {
                        (Application.Current as App).Property.Cash -= cost * num;
                        (Application.Current as App).Property.Health += num;
                        ClosePopup();
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("Have you been comfunded? Try to input again!");
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input your needed recovery health values.");
                }
            }
            else
            ClosePopup();
        }
        //医院结束
        //银行开始
        private void Bank_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
            }
            else
            {
                Banksb.Begin();
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Bank_OnLoaded);
                myPopup.LeftEvent += new MyPopup.LeftDelegate(Bank_Save);
                myPopup.RightEvent += new MyPopup.RightDelegate(Bank_Draw);
                OpenPopup();
            }
        }
        private void Bank_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = "Gringotts";
            myPopup.Reminder.Text = "Welcome to Gringotts! \nGoblins wholeheartedly at your service.\nCan I help you?";
            //the left button
            myPopup.LeftButton.Content = "Deposit";
            myPopup.LeftButton.IsEnabled = true;
            //the right button
            myPopup.RightButton.Content="Draw";
        }
        private void Bank_Save(object sender, RoutedEventArgs e)
        {
            myPopup.LeftEvent -= new MyPopup.LeftDelegate(Bank_Save);
            myPopup.RightEvent -= new MyPopup.RightDelegate(Bank_Draw);
            myPopup.LeftButton.Content = null;
            myPopup.LeftButton.IsEnabled = false;
            myPopup.LeftButton.Opacity = 0;
            myPopup.RightEvent += new MyPopup.RightDelegate(BankSave_Confirm);
            
            if ((Application.Current as App).Property.Cash <= 0)
            {
                myPopup.RightButton.Content = null;
                myPopup.RightButton.IsEnabled = false;
                myPopup.RightButton.Opacity = 0;
                myPopup.Reminder.Text = String.Format("You have no cash!");
            }
            else
            {
                myPopup.RightButton.Content = "Confirm";
                myPopup.Reminder.Text = String.Format("A goblin drive you to your vault.\n") + String.Format("You can save {0} galleons at most, with 5% interest everyday.", (Application.Current as App).Property.Cash);
                myPopup.Question.Text = "How many to deposit?";
                //the TextBox input
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                //myPopup.Input.Text = (Application.Current as App).Property.Cash.ToString();
                myPopup.Input.DefaultText = (Application.Current as App).Property.Cash.ToString();
            }
        }
        private void BankSave_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if ((0 <= num) && (num <= (Application.Current as App).Property.Cash))
                    {
                        (Application.Current as App).Property.Cash -= num;
                        (Application.Current as App).Property.Deposit += num;
                        ClosePopup();
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("You don't have enough galleons!");
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input the number!");
                }
            }
            else
                ClosePopup();
            
        }
        private void Bank_Draw(object sender, RoutedEventArgs e)
        {
            myPopup.LeftEvent -= new MyPopup.LeftDelegate(Bank_Save);
            myPopup.RightEvent -= new MyPopup.RightDelegate(Bank_Draw);
            myPopup.LeftButton.Content = null;
            myPopup.LeftButton.IsEnabled = false;
            myPopup.LeftButton.Opacity = 0;
            myPopup.RightEvent += new MyPopup.RightDelegate(BankDraw_Confirm);          
            if ((Application.Current as App).Property.Deposit <= 0)
            {
                myPopup.RightButton.Content = null;
                myPopup.RightButton.IsEnabled = false;
                myPopup.RightButton.Opacity = 0;
                myPopup.Reminder.Text = String.Format("You have no deposit!");
            }
            else
            {
                myPopup.RightButton.Content = "Confirm";
                myPopup.Reminder.Text = String.Format("A goblin come to you with a smile:") + String.Format("\"You have {0} galleons in the bank.\"", (Application.Current as App).Property.Deposit);
                myPopup.Question.Text = "How many to draw?";
                //the TextBox input
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                //myPopup.Input.Text = (Application.Current as App).Property.Deposit.ToString();
                myPopup.Input.DefaultText = (Application.Current as App).Property.Deposit.ToString();
            }
        }
        private void BankDraw_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if ((0 <= num) && (num <= (Application.Current as App).Property.Deposit))
                    {
                        (Application.Current as App).Property.Cash += num;
                        (Application.Current as App).Property.Deposit -= num;
                        ClosePopup();
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("You don't have enough deposit！");
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input the number!");
                }
            }
            else
                ClosePopup();
        }
        //银行结束
        //彩票开始
        private void Lottery_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
            }
            else
            {
                Lotterysb.Begin();
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Lottery_OnLoaded);
                myPopup.RightEvent += new MyPopup.RightDelegate(Lottery_Confirm);
                OpenPopup();
            }
        }
        private void Lottery_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = "Quidditch Lottery";
           
            if ((Application.Current as App).Property.Cash < 2)
            {
                myPopup.Reminder.Text = String.Format("You can't pay two galleons for a lottery ticket！I can't believe");
            }
            else
            {
                myPopup.RightButton.Content = "Confirm";
                LotteryNumber = (Application.Current as App).Property.Cash / 2;
                Int64 SuggestNumber = LotteryNumber / 10;
                myPopup.Reminder.Text = String.Format("Welcome! Two galleons for one lottery ticket. You have {0} galleons, can buy {1} lottery ticket, but we suggest you not to spend more than 1/10 of your galleons. You will spend one day for each time！", (Application.Current as App).Property.Cash, LotteryNumber);
                myPopup.Question.Text = String.Format("How many to buy?");
                //the TextBox input
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                //myPopup.Input.Text = SuggestNumber.ToString();
                myPopup.Input.DefaultText = SuggestNumber.ToString();
            }
        }
        private void Lottery_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if ((0 <= num) && (num <= LotteryNumber))
                    {
                        (Application.Current as App).Property.Cash -= 2 * num;
                        Random k = new Random();
                        int i =k.Next(0, 9);
                        if (i == 0)
                        {
                            //20倍奖金
                            PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                            MyMessageBox.Show(String.Format("You are so lucky! Win {0} galleons!", 2 * 20 * num));
                            (Application.Current as App).Property.Cash += 2 * 20 * num;
                        }
                        else
                        {
                            PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                            MyMessageBox.Show("It's a pity！Welcome tomorrow!");
                        }
                        ClosePopup();
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("You have not enough money");
                    }
                    (Application.Current as App).Property.Days--;
                    //if Days==0, Game Over
                    if ((Application.Current as App).Property.Days <= 0)
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("Your time in magical world is up!The Ministry of Magic help to sell all your left.");
                        SellLeftGoods();
                        GameOver();
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input the number!");
                }
            }
            else
                ClosePopup();
        }
        //彩票结束
        //高利贷开始
        private void Usury_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
            }
            else
            {
                Usurysb.Begin();
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Usury_OnLoaded);
                myPopup.LeftEvent += new MyPopup.LeftDelegate(Usury_Borrow);
                myPopup.RightEvent += new MyPopup.RightDelegate(Usury_Repay);
                OpenPopup();
            }
        }
        private void Usury_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = "Usury";
            if ((Application.Current as App).Property.Debt == 0)
            {
                myPopup.Reminder.Text = "You come to a dark cave. Several strange-looking leprechauns are talking and drinking, looks happy but evil. Borrow or repay?";
                //the left button
                myPopup.LeftButton.IsEnabled = true;
                myPopup.LeftButton.Opacity = 1;
                myPopup.LeftButton.Content = "Borrow";
                //the right button
                myPopup.RightButton.Content = "Repay";
            }
            else
            {
                myPopup.Reminder.Text = "Please repay the last borrowed money, then we can deal again.";
                //the right button
                myPopup.RightButton.Content = "Repay";
            }
        }
        private void Usury_Borrow(object sender, RoutedEventArgs e)
        {
            myPopup.LeftEvent -= new MyPopup.LeftDelegate(Usury_Borrow);
            myPopup.RightEvent -= new MyPopup.RightDelegate(Usury_Repay);

            myPopup.LeftButton.Content = null;
            myPopup.LeftButton.IsEnabled = false;
            myPopup.LeftButton.Opacity = 0;

            myPopup.RightEvent += new MyPopup.RightDelegate(UsuryBorrow_Confirm);
            if ((Application.Current as App).Property.Money < 10000)
            {
                myPopup.RightButton.Content = null;
                myPopup.RightButton.IsEnabled = false;
                myPopup.RightButton.Opacity = 0;
                myPopup.Reminder.Text = String.Format("\"Stupid human! You have no money. We have no interest in the business with you.\" a leprechaun nicknamed \"angel investor\" said.");
            }
            else
            {
                myPopup.RightButton.Content = "Confirm";
                myPopup.Reminder.Text = String.Format("\"OK. We can lend the same amount of money as yours at most. But remember, 20% compound interest everyday.\" a leprechaun nicknamed \"Sheldon Cooper\" said.") + String.Format("\nYou can borrow {0} galleons at most.", (Application.Current as App).Property.Money);    
                myPopup.Question.Text = String.Format("How many to borrow?");
                //the TextBox input
                myPopup.Input.Opacity = 1;
                myPopup.Input.IsEnabled = true;
                //myPopup.Input.Text = (Application.Current as App).Property.Money.ToString();
                myPopup.Input.DefaultText = (Application.Current as App).Property.Money.ToString();
            }
        }
        private void UsuryBorrow_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if ((0 <= num) && (num <= (Application.Current as App).Property.Money))
                    {
                        (Application.Current as App).Property.Cash += num;
                        (Application.Current as App).Property.Debt += num;
                        ClosePopup();
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("We only lend the same amount of money as yours at most!");
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input the number you want to borrow.");
                }
            }
            else
                ClosePopup();
        }

        private void Usury_Repay(object sender, RoutedEventArgs e)
        {
            myPopup.LeftEvent -= new MyPopup.LeftDelegate(Usury_Borrow);
            myPopup.RightEvent -= new MyPopup.RightDelegate(Usury_Repay);
            myPopup.LeftButton.Content = null;
            myPopup.LeftButton.IsEnabled = false;
            myPopup.LeftButton.Opacity = 0;
            myPopup.RightEvent += new MyPopup.RightDelegate(UsuryRepay_Confirm);
      
            if ((Application.Current as App).Property.Debt <= 0)
            {
                myPopup.RightButton.Content = null;
                myPopup.RightButton.IsEnabled = false;
                myPopup.RightButton.Opacity = 0;
                myPopup.Reminder.Text = String.Format("No need to repay!");
            }
            else
            {
                myPopup.RightButton.Content = "Confirm";
                UsuryRepay = Math.Min((Application.Current as App).Property.Debt, (Application.Current as App).Property.Cash);
                myPopup.Reminder.Text = String.Format("You owe {0} galleons, now have {1} galleons cash,", (Application.Current as App).Property.Debt, (Application.Current as App).Property.Cash) + String.Format(",can repay {0} galleons at most.", UsuryRepay);
                myPopup.Question.Text = String.Format("How many to repay");
                //the TextBox input
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                //myPopup.Input.Text = UsuryRepay.ToString();
                myPopup.Input.DefaultText = UsuryRepay.ToString();
            }          
        }
        private void UsuryRepay_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if ((0 <= num) && (num <= UsuryRepay))
                    {
                        (Application.Current as App).Property.Cash -= num;
                        (Application.Current as App).Property.Debt -= num;
                        ClosePopup();
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("No need to repay so many galleons！");
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input the gallons you want to repay!");
                }
            }
            else
                ClosePopup();
        }
        //高利贷结束
        //中介开始
        private void Renting_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
            }
            else
            {
                Rentsb.Begin();
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Renting_OnLoaded);
                myPopup.RightEvent += new MyPopup.RightDelegate(Renting_Confirm);
                OpenPopup();
            }
        }
        private void Renting_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = String.Format("WareHouse Rent");
            
            if ((Application.Current as App).Property.Cash <= 50000)
            {
                myPopup.Reminder.Text = String.Format("Dreamed to rent warehouse without 50000 galleons? Poor little guy!");
            }
            else
            {
                myPopup.RightButton.Content = "Confirm";
                RentSpace = (Application.Current as App).Property.Cash / 5000;
                myPopup.Reminder.Text = String.Format("A young man with a nice suit and tie smiled:\"The rent is 5000 galleons per square feet. The minimum rent area is 10 square feet.\"") + String.Format("(You can rent {0} square feet at most.)", RentSpace);
                myPopup.Question.Text = String.Format("How many to rent?");
                myPopup.Input.IsEnabled = true;
                myPopup.Input.Opacity = 1;
                //myPopup.Input.Text = RentSpace.ToString();
                myPopup.Input.DefaultText = RentSpace.ToString();
            }
        }
        private void Renting_Confirm(object sender, RoutedEventArgs e)
        {
            if (myPopup.Input.IsEnabled == true)
            {
                int num;
                if (Int32.TryParse(myPopup.Input.Text, out num))
                {
                    if (10 <= num && num<=RentSpace)
                    {
                        (Application.Current as App).Property.Cash -= num * 5000;
                        (Application.Current as App).Property.Space += num;
                        ClosePopup();
                    }
                    else if (num > RentSpace)
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("Hi, your cash is not enough!");
                    }
                    else
                    {
                        PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                        MyMessageBox.Show("Hi, the minimum rent area is 10 square feet.");
                    }
                }
                else
                {
                    PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                    MyMessageBox.Show("Please input the area you want to rent.");
                }
            }
            else
            {
                ClosePopup();
            }
        }
        //中介结束
        //股市开始
        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
            }
            else
            {
                Stocksb.Begin();
                myPopup = new MyPopup();
                myPopup.Loaded += new RoutedEventHandler(Stock_OnLoaded);
                myPopup.RightButton.IsEnabled = false;
                OpenPopup();
            }
        }


        private void Stock_OnLoaded(object sender, RoutedEventArgs e)
        {
            myPopup.Title.Text = "Job in Zonko's";
            myPopup.Reminder.Text = "You help Zonko's, get fee of 2 galleons.";
            (Application.Current as App).Property.Cash += 2;
        }
        //股市结束
        //公益组织开始
        private void RedHeart_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RankingList.xaml", UriKind.Relative));
        }
        //公益组织结束
        private void OpenPopup()
        {
            if (myPopup != null)
            {
                Canvas.SetZIndex(myPopup, 1);
                Canvas.SetLeft(myPopup, LayoutRootCanvas.ActualWidth / 2);
                Canvas.SetTop(myPopup, PropMapGrid.ActualHeight / 2);
                Storyboard a = myPopup.Resources["LoadStoryboard"] as Storyboard;
                LayoutRootCanvas.Children.Add(myPopup);
                a.Begin();
                a.Completed += (s, e1) =>
                    {
                        a.Stop();
                        Canvas.SetLeft(myPopup, LayoutRootCanvas.ActualWidth / 2 - myPopup.BorderRoot.Width / 2);
                        Canvas.SetTop(myPopup, PropMapGrid.ActualHeight / 2 - myPopup.BorderRoot.Height / 2);
                    };
            }
        }
        private void ClosePopup()
        {
            if (myPopup != null)
            {
                if (LayoutRootCanvas.Children.Contains(myPopup))
                {
                    foreach (FrameworkElement fe in IconGrid.Children)
                    {
                        if (fe.GetType() == typeof(Button))
                        {
                            (fe.RenderTransform as CompositeTransform).ScaleX = 1;
                            (fe.RenderTransform as CompositeTransform).ScaleY = 1;
                        }
                    }
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
        private void ButtonImagesDictionaryInit()
        {
            if (ImagesDictionary.Count == 0)
            {
                foreach (FrameworkElement fe in canvas.Children)
                {
                    if (fe.GetType() == typeof(Button))
                    {
                        if ((fe as Button) != null)
                        {
                            //initial buttons' image content
                            Image img = new Image();
                            Uri uri = new Uri("images/places/" +(fe as Button).Name+".jpg", UriKind.Relative);
                            BitmapImage bmp = new BitmapImage(uri);
                            img.Source = bmp;
                            ImagesDictionary.Add((fe as Button).Name, img);
                        }
                    }
                }
            }
        }
        private void DisableButtonsInIconGrid()
        {
            foreach (FrameworkElement fe in IconGrid.Children)
            {
                if (fe.GetType() == typeof(Button))
                {
                    if ((fe as Button) != null)
                    {
                        (fe as Button).IsEnabled = false;
                    }
                }
            }
        }
        private void DisableButtonsInCanvas()
        {
            foreach (FrameworkElement fe in canvas.Children)
            {
                if (fe.GetType() == typeof(Button))
                {
                    if ((fe as Button) != null)
                    {
                        (fe as Button).IsEnabled = false;
                    }
                }
            }
        }
        void RecoverTextButton()
        {
            foreach (FrameworkElement fe in MapGrid.Children)
            {
                if (fe.GetType() == typeof(Button))
                {
                    MapGrid.Children.Remove(fe as Button);
                    foreach (KeyValuePair<String, Image> entry in ImagesDictionary)
                    {
                        if (entry.Value == (fe as Button).Content as Image)
                        {
                            (fe as Button).Content = entry.Key as String;
                            canvas.Children.Add(fe as Button);
                            break;
                        }
                    }
                    break;
                }
            }
            /*
            if (SelectedButton != null)
            {
                if (MapGrid.Children.Contains(SelectedButton))
                {
                    MapGrid.Children.Remove(SelectedButton);
                    SelectedButton.Content = SelectedButtonText;
                    canvas.Children.Add(SelectedButton);
                }
            }*/
        }
        //*******************************Ranking List*******************************
        private void Ranking_Click(object sender, RoutedEventArgs e)
        {
            /*
            //RankingShowListClass.DeleteFile();
            if (ThePopup.IsOpen == true)
            {
                ThePopup.IsOpen = false;
            }
            else
            {
                RankingListPage rankingListPage = new RankingListPage();
                ThePopup.Child = rankingListPage;
                ThePopup.IsOpen = true;
            }*/
        }
        private Int64 GetAllWealth()
        {
            Int64 a=0;
            foreach (Commodity i in (Application.Current as App).MyCommodities)
            {
                a += i.Amount * i.Price;
            }
            return a + (Application.Current as App).Property.Money;
        }
        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            base.OnNavigatedTo(args);
            if((Application.Current as App).IsFromIntroduce)
            {
                IDictionary<string, string> parameters = this.NavigationContext.QueryString;
                if (parameters.ContainsKey("Param"))
                {
                    if (parameters["Param"] == "Continue")
                    {
                        AppContextStoreRestoreClass.Update();
                    }
                    else if (parameters["Param"] == "NewGame")
                    {
                        (Application.Current as App).StartNewGame();
                    }
                }
            }
            foreach (FrameworkElement fe in canvas.Children)
            {
                if (fe.GetType() == typeof(Button))
                {
                    (fe as Button).Visibility = Visibility.Visible;
                    (fe as Button).Opacity = 0;
                    (fe as Button).IsEnabled = true;
                }
            }
            PropertyGrid.DataContext = (Application.Current as App).Property;
            RecoverTextButton();
            InitNews();
            timer.Start();
            ProcessOne();            
        }
        protected override void OnNavigatedFrom(NavigationEventArgs args)
        {
            timer.Stop();
        }
        
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (LayoutRootCanvas.Children.Contains(myPopup))
            {
                ClosePopup();
                e.Cancel = true;
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }
        
    }
}