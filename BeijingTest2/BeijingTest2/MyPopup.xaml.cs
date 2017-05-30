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
using System.Windows.Controls.Primitives;

namespace BeijingTest2
{
    public partial class MyPopup : UserControl
    {
        public delegate void LeftDelegate(object sender, RoutedEventArgs e);
        public event LeftDelegate LeftEvent;
        public delegate void RightDelegate(object sender, RoutedEventArgs e);
        public event RightDelegate RightEvent;
        public MyPopup()
        {
            InitializeComponent();
        }
        public void SetInputScope()
        {
            InputScopeNameValue digitsInputNameValue = InputScopeNameValue.Text;
            Input.InputScope = new InputScope()
            {
                Names = { new InputScopeName() { NameValue = digitsInputNameValue } }
            };
        }
        private void Click_Left(object sender, RoutedEventArgs e)
        {
            OnLeftEvent(sender, e);
        }
        private void Click_Right(object sender, RoutedEventArgs e)
        {
            OnRightEvent(sender, e);
        }
        public void OnLeftEvent(object sender, RoutedEventArgs e)
        {
            if (LeftEvent != null)
                LeftEvent(sender, e);
        }
        public void OnRightEvent(object sender, RoutedEventArgs e)
        {
            if (RightEvent != null)
                RightEvent(sender, e);
        }
    }
}
