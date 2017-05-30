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
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Text;

namespace BeijingTest2
{
    public class Myproperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Int32 days;//0-30
        private Int32 health;//0-100
        private Int32 reputation;//0-100
        private Int32 space;//100-no 
        private Int64 cash;
        private Int64 deposit;
        private Int64 debt;
        private Int64 stock;
        
        public Myproperty()
        {
            this.days = 30;
            this.cash = 30000;
            this.health = 100;
            this.reputation = 100;
            this.deposit = 0;
            this.debt = 0;
            this.space = 100;
            this.stock = 0;
        }
        //Days:0-30
        public Int32 Days
        {
            set
            {
                if (value >= 0 && value <= 30)
                {
                    if (days != value)
                    {

                        days = value;
                        OnPropertyChanged("Days");
                    }
                }
                else
                    throw (new ArgumentOutOfRangeException("Days: 0-30"));
            }
            get
            {
                return days;
            }
        }
        //Cash:0-no
        public Int64 Cash
        {
            set
            {
                if (value >= 0)
                {
                    if (cash != value)
                    {
                        cash = value;
                        OnPropertyChanged("Cash");
                    }
                }
                else
                    throw (new ArgumentOutOfRangeException("Cash: >=0"));
            }
            get
            {
                return cash;
            }
        }
        //Health:
        public Int32 Health
        {
            set
            {
                if (health != value)
                {
                    if (0 <= value)
                        health = value;
                    else
                        health = 0;
                    OnPropertyChanged("Health");
                }
            }
            get
            {
                return health;
            }
        }
        //Reputation: 
        public Int32 Reputation
        {
            set
            {
                if (reputation != value)
                {
                    if (0 <= value)
                        reputation = value;
                    else
                        reputation = 0;
                    OnPropertyChanged("Reputation");
                }
            }
            get
            {
                return reputation;
            }
        }
        //Deposit:0-no
        public Int64 Deposit
        {
            set
            {
                if (value >= 0)
                {
                    if (deposit != value)
                    {
                        deposit = value;
                        OnPropertyChanged("Deposit");
                    }
                }
                else
                    throw (new ArgumentOutOfRangeException("Deposit: >=0"));
            }
            get
            {
                return deposit;
            }
        }
        //Money:0-no
        public Int64 Money
        {
            get
            {
                return this.cash + this.deposit+this.stock-this.debt;
            }
        }
        //Debt: 0-no
        public Int64 Debt
        {
            set
            {
                if (value >= 0)
                {
                    if (debt != value)
                    {
                        debt = value;
                        OnPropertyChanged("Debt");
                    }
                }
                else
                    throw (new ArgumentOutOfRangeException("Debt: >=0"));
            }
            get
            {
                return debt;
            }
        }
        //Debt: 0-no
        public Int64 Stock
        {
            set
            {
                if (value >= 0)
                {
                    if (stock != value)
                    {
                        stock = value;
                        OnPropertyChanged("Stock");
                    }
                }
                else
                    throw (new ArgumentOutOfRangeException("stock: >=0"));
            }
            get
            {
                return stock;
            }
        }
        //Space: 100-no
        public Int32 Space
        {
            set
            {
                if (value >= 100)
                {
                    if (space != value)
                    {
                        space = value;
                        OnPropertyChanged("Space");
                    }
                }
                else
                    throw (new ArgumentOutOfRangeException("Space: >=100"));
            }
            get
            {
                return space;
            }
        }
        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }
    }
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string) && parameter is string)
                return String.Format(parameter as string, value);
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class LongStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if (value.GetType() == typeof(long))
                {

                    String a = value.ToString();
                    int FirstLength = a.Length % 3;
                    int Length = 0;
                    if (a.Length % 3 == 0)
                        Length = a.Length + a.Length / 3 - 1;
                    else
                        Length = a.Length + a.Length / 3;
                    StringBuilder b = new StringBuilder(Length);
                    for (int i = 0; i < Length; i++)
                    {
                        b.Append(",");
                    }
                        for (int i = 0, j = 0; i < a.Length; i++, j++)
                        {
                            b[j] = a[i];
                            if (0 == ((i - FirstLength+1) % 3))
                            {
                                j++;
                                if (j != Length)
                                    b[j] = ',';
                            }
                        }
                    return b.ToString();
                }
                else
                    return value.ToString();
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class ReputationStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if ((int)value >= 120)
                    return "Great";
                else if ((int)value >= 100)
                    return "Good";
                else if ((int)value >= 80)
                    return "Good";
                else if ((int)value >= 70)
                    return "Normal";
                else if ((int)value >= 60)
                    return "Lose face";
                else if ((int)value >= 50)
                    return "Bad";
                else if ((int)value >= 40)
                    return "Bad";
                else if ((int)value >= 30)
                    return "Evil";
                else if ((int)value >= 20)
                    return "Evil";
                else if ((int)value >= 10)
                    return "Asshole";
                else if ((int)value >= 0)
                    return "Asshole";
                else
                    return "Mysterious";
            }               
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class HealthStringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                if ((int)value >= 100)
                    return "Robust";
                else if ((int)value >= 90)
                    return "Healthy";
                else if ((int)value >= 80)
                    return "Healthy";
                else if ((int)value >= 70)
                    return "Normal";
                else if ((int)value >= 60)
                    return "Normal";
                else if ((int)value >= 50)
                    return "sick";
                else if ((int)value >= 40)
                    return "sick";
                else if ((int)value >= 30)
                    return "Weak";
                else if ((int)value >= 20)
                    return "Weak";
                else if ((int)value >= 10)
                    return "Dying";
                else if ((int)value >= 0)
                    return "Dying";
                else
                    return "Mysterious";
            }               
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class WidthAdjustConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double) return (((double)value -100)/2);
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class Commodity : INotifyPropertyChanged
    {

        string name;
        string image;
        int price;
        int amount;
        int lowMinPrice;
        int lowMaxPrice;
        int midMinPrice;
        int midMaxPrice;
        int highMinPrice;
        int highMaxPrice;
        string lowPriceInfo;
        string field;
        string highPriceInfo;

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void CommodityDelegate(string message);

        public event CommodityDelegate CommodityEvent;

        public void OnCommodityEvent(string message)
        {
            if (CommodityEvent != null)
            {
                CommodityEvent(message);
            }
        }

        public void lowpricing()
        {
            Random k = new Random();
            this.Price = k.Next(LowMinPrice, LowMaxPrice);
            OnCommodityEvent(lowPriceInfo);
        }
        public void midpricing()
        {
            Random k = new Random();
            this.Price = k.Next(MidMinPrice, MidMaxPrice);
        }
        public void highpricing()
        {
            Random k = new Random();
            this.Price = k.Next(HighMinPrice, HighMaxPrice);
            OnCommodityEvent(highPriceInfo);
        }

        //----------------------------------------
        public int LowMinPrice
        {
            set
            {
                if (lowMinPrice != value)
                {
                    lowMinPrice = value;
                    OnPropertyChanged("LowMinPrice");
                }
            }
            get
            {
                return lowMinPrice;
            }
        }
        public int LowMaxPrice
        {
            set
            {
                if (lowMaxPrice != value)
                {
                    lowMaxPrice = value;
                    OnPropertyChanged("LowMaxPrice");
                }
            }
            get
            {
                return lowMaxPrice;
            }
        }
        //------------------------------------------------
        public int MidMinPrice
        {
            set
            {
                if (midMinPrice != value)
                {
                    midMinPrice = value;
                    OnPropertyChanged("MidMinPrice");
                }
            }
            get
            {
                return midMinPrice;
            }
        }
        public int MidMaxPrice
        {
            set
            {
                if (midMaxPrice != value)
                {
                    midMaxPrice = value;
                    OnPropertyChanged("MidMaxPrice");
                }
            }
            get
            {
                return midMaxPrice;
            }
        }
        //----------------------------------------------------------
        public int HighMinPrice
        {
            set
            {
                if (highMinPrice != value)
                {
                    highMinPrice = value;
                    OnPropertyChanged("HighMinPrice");
                }
            }
            get
            {
                return highMinPrice;
            }
        }
        public int HighMaxPrice
        {
            set
            {
                if (highMaxPrice != value)
                {
                    highMaxPrice = value;
                    OnPropertyChanged("HighMaxPrice");
                }
            }
            get
            {
                return highMaxPrice;
            }
        }
        //-----------------------------------------------------------
        public string Field
        {
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged("MidPriceInfo");
                }
            }
            get
            {
                return field;
            }
        }
        public string HighPriceInfo
        {
            set
            {
                if (highPriceInfo != value)
                {
                    highPriceInfo = value;
                    OnPropertyChanged("HighPriceInfo");
                }
            }
            get
            {
                return highPriceInfo;
            }
        }
        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
            get
            {
                return name;
            }
        }
        public string Image
        {
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged("Image");
                }
            }
            get
            {
                return image;
            }
        }
        public int Price
        {
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged("Price");
                }
            }
            get
            {
                return price;
            }
        }

        public int Amount
        {
            set
            {
                if (amount != value)
                {
                    amount = value;
                    OnPropertyChanged("Amount");
                }
            }
            get
            {
                return amount;
            }
        }
        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }
        //+test code
        public Commodity()
        {
        }
        //-test code
        public Commodity(String name, String image, int lowMinPrice, int lowMaxPrice,
        int midMinPrice, int midMaxPrice, int highMinPrice, int highMaxPrice, string lowPriceInfo, string highPriceInfo, string field)
        {
            this.name = name;
            this.image = image;
            this.lowMinPrice = lowMinPrice;
            this.lowMaxPrice = lowMaxPrice;
            this.midMinPrice = midMinPrice;
            this.midMaxPrice = midMaxPrice;
            this.highMinPrice = highMinPrice;
            this.highMaxPrice = highMaxPrice;
            this.lowPriceInfo = lowPriceInfo;
            this.highPriceInfo = highPriceInfo;
            this.field = field;
        }

        public Commodity(String name, String image, int lowPrice, int midPrice, int highPrice, string lowPriceInfo, string highPriceInfo, string field)
        {
            this.name = name;
            this.image = image;
            this.lowMinPrice = (int)(lowPrice*0.8);
            this.lowMaxPrice = (int)(lowPrice*1.2);
            this.midMinPrice = (int)(midPrice*0.6);
            this.midMaxPrice = (int)(midPrice*1.4);
            this.highMinPrice = (int)(highPrice*0.8);
            this.highMaxPrice = (int)(highPrice*1.2);
            this.lowPriceInfo = lowPriceInfo;
            this.highPriceInfo = highPriceInfo;
            this.field = field;
        }

        public Commodity(Commodity temp)
        {
            this.name = temp.name;
            this.image = temp.image;
            this.lowMinPrice = temp.lowMinPrice;
            this.lowMaxPrice = temp.lowMaxPrice;
            this.midMinPrice = temp.midMinPrice;
            this.midMaxPrice = temp.midMaxPrice;
            this.highMinPrice = temp.highMinPrice;
            this.highMaxPrice = temp.highMaxPrice;
            this.amount = temp.amount;
            this.price = temp.price;
            this.field = temp.field;
            this.lowPriceInfo = temp.lowPriceInfo;
            this.highPriceInfo = temp.highPriceInfo;
        }
    }
    public class CommodityStore : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<Commodity> commodities = new ObservableCollection<Commodity>();

        public ObservableCollection<Commodity> Commodities
        {
            set
            {
                if (commodities != value)
                {
                    commodities = value;
                    OnPropertyChanged("Commodities");
                }
            }
            get
            {
                return commodities;
            }
        }
        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }
    }

    public class AllCommodityStore : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        CommodityStore commodityStore = new CommodityStore();
        public AllCommodityStore()
        {
            /*1, 5, 20*/
            commodityStore.Commodities.Add(new Commodity("Skiving Snackboxes", "images/goods/Skiving Snackboxes.jpg", 2, 5, 20, "Bad news! Filch is back from hospital. Escaping class won't be easy anymore!\nThe price of Skiving Snackboxes fall.", "The history of magic is boring to hell, who have some siving snacboxes?\nThe price of Skiving Snackboxes rise.", "中关村"));
            /*5, 20，100*/
            commodityStore.Commodities.Add(new Commodity("The Quibbler", "images/goods/The Quibbler.jpg", 5, 20, 100, "Full of boring Ads, nobody wants to buy The Quibbler.\nThe price of The Quibbler fall.", "More details about the sex scandal in M.O.M? All can be found in The Quibbler!\nThe price of The Quibbler rise.", "崇文门"));

            /*30, 60, 300*/
            commodityStore.Commodities.Add(new Commodity("Auto-Answer Quill", "images/goods/Auto-Answer Quill.jpg", 30, 60, 300, "Hogwards bulletin board: Students who use any kind of tools to cheat will be expelled from school.\nThe price of Auto-Answer Quill fall.", "Damn it! The final exam is coming，boys and girls desire to have an Auto-Answer Quill.\nThe price of Auto-Answer Quill rise.", "通州"));
            /*40, 200, 400*/
            commodityStore.Commodities.Add(new Commodity("Quidditch Tickets", "images/goods/Quidditch Tickets.jpg", 40, 200, 400, "The cold has frozen the earth solid, no one wants to go out.\nThe price of Quidditch Tickets fall.", "The Quidditch world cup opens tonight! There's some tickets only in the black market.\nThe price of Quidditch Tickets rise.", "西直门"));
            

            /*100, 500, 2000*/
            commodityStore.Commodities.Add(new Commodity("Remembrall", "images/goods/Remembrall.jpg", 100, 500, 2000, "Remembrall equals old? No, I never used it, ever!\nThe price of Remembrall fall.", "Kids love remembralls! They can use them to find candies.\nThe price of Remembrall rise.", "亦庄"));
            /*500, 800, 3000*/
            commodityStore.Commodities.Add(new Commodity("Pocket Sneakoscope", "images/goods/Pocket Sneakoscope.jpg", 500, 800, 3000, "Defective pocket sneakoscopes make too much noise, no one want to be even near them.\nThe price of Pocket Sneakoscope fall.", "There are rumors that Death Eaters have escaped from Azkaban, Pocket Sneakoscopes suddenly become popular.\nThe price of Pocket Sneakoscope rise.", "回龙观"));
            /*600, 3000, 5000*/
            commodityStore.Commodities.Add(new Commodity("Nimbus 2012", "images/goods/Nimbus 2012.jpg", 600, 3000, 5000, "Firebolt is more fast than Nimbus 2012? What we still hold that for?\nThe price of Nimbus 2012 fall.", "Everybody witnessed the England National Team winning the 100th Quidditch World Cup with Nimbus 2012, it's a hit!\nThe price of Nimbus 2012 rise.", "公主坟"));
            
            /*5000,10000,50000*/
            commodityStore.Commodities.Add(new Commodity("Baby Dragon", "images/goods/Baby Dragon.jpg", 5000, 10000, 50000, "Breeding dragon? Nobody dare to break law when Severus Snape sits in the chair of minister of magic.\nThe price of Baby Dragon fall.", "\"Taming a dragon is the coolest thing in the world\", Daily Prophet quotes a 13 years old student from Hogwarts.\nThe price of Baby Dragon rise.", "东直门"));
            /*20000, 50000, 200000*/
            commodityStore.Commodities.Add(new Commodity("Horn of The Unicorn", "images/goods/Horn of The Unicorn.jpg", 10000, 20000, 100000, "Professor found alternative for unicorn horn! No more needs to harm unicorn!\nThe price of Horn of The Unicorn fall.", "M.O.M Reiterated that kill a Unicorn is illegal and immoral.\nThe price of Horn of The Unicorn rise.", "天通苑"));
            /*50000, 200000, 500000*/
            commodityStore.Commodities.Add(new Commodity("Invisibility Cloak", "images/goods/Invisibility Cloak.jpg", 50000, 100000, 300000, "Alert! There are fake invisibility cloaks in the market!\nThe price of Invisibility Cloak fall.", "New Disneyland in muggle world! Wear an invisibility cloak and have a good time!\nThe price of Invisibility Cloak rise.", "磁器口"));
        }
        public CommodityStore CommodityStore
        {
            protected set
            {
                if (commodityStore != value)
                {
                    commodityStore = value;
                    OnPropertyChanged("CommodityStore");
                }
            }
            get
            {
                return commodityStore;
            }
        }
        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }
    }

    public class SaleCommodityStore : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        CommodityStore commodityStore = new CommodityStore();
        public SaleCommodityStore(string field)
        {
            Random k = new Random();
            int value = k.Next(6, 8);
            for (int i = 0; i < value; )
            {
                Boolean found = false;
                Commodity temp = (Application.Current as App).AllCommodities[k.Next(0, (Application.Current as App).AllCommodities.Count-1)];
                foreach (Commodity j in commodityStore.Commodities)
                {
                    if (j.Name == temp.Name)
                    {
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    Commodity newCommodity = new Commodity(temp);
                    newCommodity.CommodityEvent += new Commodity.CommodityDelegate(MessageShow);
                    Random m = new Random();
                    //Adjust the random Algrithm
                    int n = m.Next(0, 99);
                    int threshold=0;
                    if (newCommodity.Field == field)
                        threshold = 20;//概率是20%，60%，20%
                    else
                        threshold = 15;//概率是15%，70%，15%
                    //20-->29
                    if (n<30 && n>=(30-threshold))
                        newCommodity.lowpricing();
                    //71-->80
                    else if (n>70 && n<=(70+threshold))
                        newCommodity.highpricing();
                    else
                        newCommodity.midpricing();

                    //newCommodity.Price = k.Next(newCommodity.MinPrice, newCommodity.MaxPrice);
                    commodityStore.Commodities.Add(newCommodity);
                    i++;
                }
            }
        }
        void MessageShow(string message)
        {
            if (message != null)
            {
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show(message);               
            }
        }
        public CommodityStore CommodityStore
        {
            protected set
            {
                if (commodityStore != value)
                {
                    commodityStore = value;
                    OnPropertyChanged("CommodityStore");
                }
            }
            get
            {
                return commodityStore;
            }
        }
        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }
    }
    public class MyCommodityStore : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        CommodityStore commodityStore = new CommodityStore();
        public CommodityStore CommodityStore
        {
            protected set
            {
                if (commodityStore != value)
                {
                    commodityStore = value;
                    OnPropertyChanged("CommodityStore");
                }
            }
            get
            {
                return commodityStore;
            }
        }
        protected virtual void OnPropertyChanged(string propChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propChanged));
        }
    }
    public class Incident
    {
        public string music;
        public string picture;
        public string message;
        public List<string> messageList = new List<string>();

        public delegate void IncidentDelegate(Myproperty myproperty, Incident incident);

        public event IncidentDelegate IncidentEvent;

        public Incident(string music, string picture, string message)
        {
            this.music = music;
            this.picture = picture;
            this.message = message;
        }
        public void OnIncidentEvent(Myproperty myproperty,Incident incident)
        {
            if (IncidentEvent != null)
                IncidentEvent(myproperty, incident);
        }
    }
    public class IncidentList
    {
        List<Incident> incidentList = new List<Incident>();
        public IncidentList()
        {
            Incident temp = new Incident("music/Oppugno.mp3", "Images/Oppugno.png", "What the hell? You are shot in a dark alley out of nowhere.");
            temp.IncidentEvent += new Incident.IncidentDelegate(IncidentHealth5);
            incidentList.Add(temp);
            temp = new Incident("music/WhompingWillow.mp3", "Images/WhompingWillow.png", "Hit by the Whomping Willow.");
            temp.IncidentEvent += new Incident.IncidentDelegate(IncidentHealth5);

            incidentList.Add(temp);
            temp = new Incident("music/dementor.mp3", "Images/dementor.png", "A dementor passing by. ");
            temp.IncidentEvent += new Incident.IncidentDelegate(IncidentHealth10);
            incidentList.Add(temp);
            temp = new Incident("music/Leprechaun.mp3", "Images/Leprechaun.png", "Damn! 15% galleons missing! Am I fooled by the Leprechaun?");
            temp.IncidentEvent += new Incident.IncidentDelegate(IncidentCash15);
            incidentList.Add(temp);
            temp = new Incident("music/murmuring.mp3", "Images/murmuring.png", "A little man murmuring in the corner.");
            temp.IncidentEvent += new Incident.IncidentDelegate(IncidentCash5);
            incidentList.Add(temp);
            temp = new Incident("music/Pickupmoney.mp3", "Images/Pickupmoney.png", "There is a shabby bag on the street corner. What's in it?");
            temp.IncidentEvent += new Incident.IncidentDelegate(IncidentCashAdd5);
            incidentList.Add(temp);
        }
        void IncidentHealth10(Myproperty myproperty, Incident incident)
        {
            if (myproperty != null)
                myproperty.Health -= 10;
            if (incident != null)
                incident.messageList.Add("Your health value reduces 10 points!");
        }
        void IncidentHealth5(Myproperty myproperty, Incident incident)
        {
            if (myproperty != null)
                myproperty.Health -= 5;
            if (incident != null)
                incident.messageList.Add("Your health value reduces 5 points!");
        }
        void IncidentCash5(Myproperty myproperty, Incident incident)
        {
            if (myproperty != null)
                myproperty.Cash -= myproperty.Cash * 1 / 20;
            if (incident != null)
                incident.messageList.Add("You lost 5% galleons!");
        }
        void IncidentCash10(Myproperty myproperty, Incident incident)
        {
            if (myproperty != null)
                myproperty.Cash -= myproperty.Cash/10;
            if (incident != null)
                incident.messageList.Add("You lost 10% galleons!");
        }
        void IncidentCash15(Myproperty myproperty, Incident incident)
        {
            if (myproperty != null)
                myproperty.Cash -= myproperty.Cash*3/20;
            if (incident != null)
                incident.messageList.Add("You lost 15% galleons!");
        }
        void IncidentCashAdd5(Myproperty myproperty, Incident incident)
        {
            if (myproperty != null)
                myproperty.Cash += 5000;
            if (incident != null)
                incident.messageList.Add("Lucky Day! You found 5000 galleons.");
        }
        private void IncidentShow(Incident incident)
        {
            if (incident != null)
            {
                incident.messageList.Clear();
                incident.OnIncidentEvent((Application.Current as App).Property, incident);
                string message = incident.message;
                foreach (string temp in incident.messageList)
                {
                    message += " ";
                    message += temp;
                }
                PixelLab.Common.MessageBoxService MyMessageBox = new PixelLab.Common.MessageBoxService();
                MyMessageBox.Show(message);
            }
        }
        public void IncidentStart()
        {
            //Adjust the Random Algrithm
            Random k = new Random();
            int i = k.Next(0, 999);
            int j;
            if (i > 300 && i < 700)
            {
                j = k.Next(0, 999) % incidentList.Count;
                IncidentShow(incidentList[j]);
            }
        }
    }
    public class News
    {
        private string news;
        public string _news
        {
            set
            {
                if (value != null)
                {
                    news = value;
                }
                else
                    throw (new ArgumentOutOfRangeException("_news: not null"));
            }
            get
            {
                return news;
            }
        }
        public News(string news)
        {
            if (news != null)
            {
                this.news = news;
            }
        }
    }
    public class MyNewsList
    {
        private List<News> NewsList = new List<News>();
        private string NewsString=null;
        private void CreateList()
        {
            NewsString = null;
            NewsList.Add(new News("Hogwarts celebrates its 300th anniversary.     "));
            NewsList.Add(new News("As the new term starts, many pretty little girls all over the world come to Hogwarts.    "));
            NewsList.Add(new News("Harry Potter will be the presenter of the 100th Quidditch World Cup tomorrow.    "));
            NewsList.Add(new News("Severus Snape announced running for Minister of Magic.    "));
            NewsList.Add(new News("World's smallest frog discovered.    "));
            NewsList.Add(new News("The Philosopher's Stone has been stolen again!    "));
            NewsList.Add(new News("England National won Quidditch World Cup!    "));
            NewsList.Add(new News("Survey: which is your favorate shopping avenues?    "));
            NewsList.Add(new News("The Ministry of Magic keen to cooperate with the order of the Phoenix.    "));
            NewsList.Add(new News("O.W.L.S passing rate hits lowest level in Hogwarts.    "));
            NewsList.Add(new News("Wizards urge M.O.M to condemn unicorn' killings.   "));
            NewsList.Add(new News("Why unity is important to Wizards.   "));
            NewsList.Add(new News("The Ministry of Magic Seeks to Merge Agencies.    "));
            NewsList.Add(new News("Fleur Delacour wins the most favorate dancers.    "));
            NewsList.Add(new News("Zonko's Joke Shop, best choice to win Halloween Costume Ball.    "));
            NewsList.Add(new News("Three more trolls were found killed in the Dark Forest.    "));
            NewsList.Add(new News("Break-in at Gringotts, The Philosopher's Stone missing.    "));
            NewsList.Add(new News("Breaking news! The Ministry of Magic takes over Gringotts, Goblins go on strike!    "));
            NewsList.Add(new News("Death-eaters coming back, Aurors are gathered preparing the Third Wizard War.    "));
            NewsList.Add(new News("Daily Prophet Sues Rita Skeeter Over copyrights.    "));
            NewsList.Add(new News("Protesters have held large demonstrations in front of the Ministry of Magic.    "));
            NewsList.Add(new News("Lucius Malfoy admitted taking bribes, and was sent to Azkaban yesterday.   "));
            NewsList.Add(new News("Peter Pettigrew jailed for 150 years.   "));
            NewsList.Add(new News("Harry Potter goes to the movies.    "));
            NewsList.Add(new News("Viktor Krum: I will be retired after my fifth Quidditch World Cup.    "));
            NewsList.Add(new News("Rita Skeeter: My romantic affair with Minister of Finance.   "));
            NewsList.Add(new News("Corrnelius Fudge resigned in his third term of Minister of Magic, due to love affairs with witches.   "));
            NewsList.Add(new News("Quibbler: Witch running into muggle's world became famous singer Lady Gaga.    "));
            NewsList.Add(new News("The Malfoys denoted a million galleons to the M.O.M, hoping to get a penalty reduce.    "));
            NewsList.Add(new News("The Triwizard Tournament will be held in Beauxbatons this summer.   "));
        }
        public string NewsListShow()
        {
            CreateList();
            Random k = new Random();
            while (NewsList.Count != 0)
            {
                int i = k.Next(0, NewsList.Count-1);
                NewsString += NewsList[i]._news;
                NewsList.RemoveAt(i);
            }
            return NewsString;
        }
    }
    public class RankingItemClass
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string ItemName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(ItemName));
        }
        private Int32 rankNumber=1;//0-100
        private Int32 health;//0-100
        private Int32 reputation;//0-100
        private Int64 money;
        private string name;
        public Int32 RankNumber
        {
            set
            {
                if (value >= 1 && value <= 10)
                {
                    if (rankNumber != value)
                    {

                        rankNumber = value;
                        OnPropertyChanged("rankNumber");
                    }
                }
            }
            get
            {
                return rankNumber;
            }
        }
        public Int64 SuccessValue
        {
            get
            {
                return this.Money*(Int64)this.Health*(Int64)this.Reputation/10000;
            }
        }
        public Int32 Health
        {
            set
            {
                if (value >= 0 && value <= 100)
                {
                    if (health != value)
                    {

                        health = value;
                        OnPropertyChanged("health");
                    }
                }
            }
            get
            {
                return health;
            }
        }
        public Int32 Reputation
        {
            set
            {
                if (reputation != value)
                {
                    reputation = value;
                    OnPropertyChanged("reputation");
                }
            }
            get
            {
                return reputation;
            }
        }
        public Int64 Money
        {
            set
            {
                if (value >= 0)
                {
                    if (money != value)
                    {

                        money = value;
                        OnPropertyChanged("money");
                    }
                }
            }
            get
            {
                return money;
            }
        }
        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("name");
                }
            }
            get
            {
                return name;
            }
        }
    }
    public class RankingShowListClass
    {
        const string filename = "RankingList.xml";
        public List<RankingItemClass> RankingShowList = new List<RankingItemClass>();
        public RankingItemClass Add(Myproperty PropertyItem, string name)
        {
            RankingItemClass rankingItem = new RankingItemClass();
            if (PropertyItem != null)
            {
                rankingItem.Health = PropertyItem.Health;
                rankingItem.Money = PropertyItem.Money;
                rankingItem.Reputation = PropertyItem.Reputation;
            }
            rankingItem.Name = name;
            this.RankingShowList.Add(rankingItem);
            return rankingItem;
        }
        public static void Delete()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(filename))
            {
                storage.DeleteFile(filename);
            }
        }
        public void Save()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(filename))
            {
                storage.DeleteFile(filename);
            }
            IsolatedStorageFileStream stream = storage.CreateFile(filename);
            if (null != stream)
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<RankingItemClass>));
                xml.Serialize(stream, RankingShowList);
                stream.Close();
                stream.Dispose();
            }
        }
        public List<RankingItemClass> Load()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(filename))
            {
                IsolatedStorageFileStream stream = storage.OpenFile(filename, FileMode.Open);
                if (null != stream)
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<RankingItemClass>));
                    RankingShowList.Clear();
                    RankingShowList = xml.Deserialize(stream) as List<RankingItemClass>;
                    stream.Close();
                    stream.Dispose();
                }
            }
            else
            {
                RankingShowList.Clear();
            }
            return RankingShowList;
        }
        public void Sort()
        {
            if (RankingShowList.Count > 1)
            {
                bool swapped;
                do
                {
                    int counter = RankingShowList.Count - 1;
                    swapped = false;

                    while (counter > 0)
                    {
                        // Compare the items' length. 
                        if (RankingShowList[counter].SuccessValue
                            > RankingShowList[counter - 1].SuccessValue)
                        {
                            // Swap the items.
                            RankingItemClass temp = RankingShowList[counter];
                            RankingShowList[counter] = RankingShowList[counter - 1];
                            RankingShowList[counter - 1] = temp;
                            swapped = true;
                        }
                        // Decrement the counter.
                        counter -= 1;
                    }
                }
                while ((swapped == true));
                for (int i = 0; i < RankingShowList.Count; i++)
                {
                    RankingShowList[i].RankNumber = i + 1;
                }
            }
        }
        public void Cut()
        {
            if (RankingShowList.Count > 10)
            {
                for (int i = 10; i < RankingShowList.Count; i++)
                {
                    RankingShowList.RemoveAt(i);
                }
            }
        }
    }

    public class AppContextStoreRestoreClass
    {
        public Myproperty Property { get; set; }
        public ObservableCollection<Commodity> AllCommodities { get; set; }
        public ObservableCollection<Commodity> MyCommodities { get; set; }

        const string AppSettingsFileName = "AppContext.xml";

        public AppContextStoreRestoreClass()
        {
            Property = (Application.Current as App).Property;
            AllCommodities=(Application.Current as App).AllCommodities;
            MyCommodities = (Application.Current as App).MyCommodities;
        }
        public static bool IsFileExist()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(AppSettingsFileName))
                return true;
            else
                return false;
        }
        public static void DeleteFile()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (storage.FileExists(AppSettingsFileName))
            {
                storage.DeleteFile(AppSettingsFileName);
            }
        }
        public void Save()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream stream = null;
            if (storage.FileExists(AppSettingsFileName))
            {

                storage.DeleteFile(AppSettingsFileName);
            }
            stream = storage.CreateFile(AppSettingsFileName);
            XmlSerializer xml = new XmlSerializer(typeof(AppContextStoreRestoreClass));
            xml.Serialize(stream, this);
            stream.Close();
            stream.Dispose();
        }
        public static AppContextStoreRestoreClass Load()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            AppContextStoreRestoreClass temp=null;
            if (storage.FileExists(AppSettingsFileName))
            {
                IsolatedStorageFileStream stream =
                    storage.OpenFile(AppSettingsFileName, FileMode.Open);
                XmlSerializer xml = new XmlSerializer(typeof(AppContextStoreRestoreClass));
                temp = xml.Deserialize(stream) as AppContextStoreRestoreClass;
                stream.Close();
                stream.Dispose();
            }
            if (temp == null)
                temp = new AppContextStoreRestoreClass();
            return temp;
        }
        public static void Update()
        {
            AppContextStoreRestoreClass appContextStoreRestoreClass = AppContextStoreRestoreClass.Load();
            (Application.Current as App).Property = appContextStoreRestoreClass.Property;
            (Application.Current as App).AllCommodities = appContextStoreRestoreClass.AllCommodities;
            (Application.Current as App).MyCommodities = appContextStoreRestoreClass.MyCommodities;
        }
        public static void Store()
        {
            AppContextStoreRestoreClass appContextStoreRestoreClass = new AppContextStoreRestoreClass();
            appContextStoreRestoreClass.Save();
        }
    }

    public class DefaultTextTextbox : TextBox 
    { 
        private string _defaultText = string.Empty;
        private string[] _invalidCharacters = {"*","#","-","."," " };
        public string DefaultText 
        { 
            get 
            { 
                return _defaultText; 
            } 
            set 
            { 
                _defaultText = value; 
                SetDefaultText(); 
            } 
        }
        public DefaultTextTextbox() 
        { 
            this.GotFocus += (sender, e) => 
            { 
                if (this.Text.Equals(DefaultText)) 
                { 
                    this.Text = string.Empty; 
                } 
            }; 
            this.LostFocus += (sender, e) => 
            { 
                SetDefaultText(); 
            }; 
        } 
        private void SetDefaultText() 
        { 
            if (this.Text.Trim().Length == 0)
                this.Text = DefaultText; 
        }
    }

    public class ActualSizePropertyProxy : FrameworkElement, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public FrameworkElement Element
        {
            get { return (FrameworkElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        public double ActualHeightValue
        {
            get { return Element == null ? 0 : Element.ActualHeight; }
        }

        public double ActualWidthValue
        {
            get { return Element == null ? 0 : Element.ActualWidth; }
        }

        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(ActualSizePropertyProxy),
                                        new PropertyMetadata(null, OnElementPropertyChanged));

        private static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ActualSizePropertyProxy)d).OnElementChanged(e);
        }

        private void OnElementChanged(DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement oldElement = (FrameworkElement)e.OldValue;
            FrameworkElement newElement = (FrameworkElement)e.NewValue;

            newElement.SizeChanged += new SizeChangedEventHandler(Element_SizeChanged);
            if (oldElement != null)
            {
                oldElement.SizeChanged -= new SizeChangedEventHandler(Element_SizeChanged);
            }
            NotifyPropChange();
        }

        private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotifyPropChange();
        }

        private void NotifyPropChange()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ActualWidthValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("ActualHeightValue"));
            }
        }
    } 
}
