﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StopLight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        int iLeftRight;
        public MainWindow()
        {

            //I AM TESTING GIT. - Taylor

            SolidColorBrush red = new SolidColorBrush(Colors.Red);
            SolidColorBrush green = new SolidColorBrush(Colors.Green);
            SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
            SolidColorBrush gray = new SolidColorBrush(Colors.LightGray);
            InitializeComponent();

            
            iLeftRight = 0;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
            if (iLeftRight == 0)
            {
                LeftGreen.Fill = gray;
                LeftYellow.Fill = gray;
                LeftRed.Fill = red;

                //RightGreen.Fill =
            }
        }
        

            private void dispatcherTimer_Tick(object sender, EventArgs e)
                {
                SolidColorBrush red = new SolidColorBrush(Colors.Red);
                SolidColorBrush green = new SolidColorBrush(Colors.Green);
                SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
                SolidColorBrush gray = new SolidColorBrush(Colors.LightGray);

                //put your code here
                }

            private void btnleft_Click(object sender, RoutedEventArgs e)
            {

            }
        }
    }

