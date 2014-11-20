using System;
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
using System.Timers;
using System.Threading;
using System.Diagnostics;

public enum color { RED, GREEN };

namespace StopLight

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum state {NORTHSOUTHGO, NORTHSOUTHSTOP, EASTWESTGO, EASTWESTSTOP};
    //enum color {RED, GREEN};
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        //int iLeftRight;

        StreetLight WestLight;
        StreetLight NorthLight;
        StreetLight EastLight;
        StreetLight SouthLight;

        state currentState;

        public MainWindow()
        {
            InitializeComponent();
            currentState = state.NORTHSOUTHGO;
            if (currentState == state.NORTHSOUTHGO)
            {
                WestLight = new StreetLight(LeftGreen, LeftYellow, LeftRed, color.RED);
                NorthLight = new StreetLight(TopGreen, TopYellow, TopRed, color.GREEN);
                EastLight = new StreetLight(RightGreen, RightYellow, RightRed, color.RED);
                SouthLight = new StreetLight(BottomGreen, BottomYellow, BottomRed, color.GREEN);
            }
            else throw new System.InvalidOperationException("startState must be NorthSouthGo");

            
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
   

            if (currentState == state.EASTWESTSTOP)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);



                NorthSouthGo();
                currentState = state.NORTHSOUTHGO;
            }
            else if (currentState == state.NORTHSOUTHGO)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);




                NorthSouthStop();
                currentState = state.NORTHSOUTHSTOP;
            }
            else if (currentState == state.NORTHSOUTHSTOP)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);
                WestEastGo();
               

                currentState = state.EASTWESTGO;
            }
            else if (currentState == state.EASTWESTGO)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": State:");
                Debug.WriteLine(currentState);
                WestEastStop();
              

                currentState = state.EASTWESTSTOP;
            }
        }

        private void WestEastStop()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("WestEastStop");
            EastLight.TurnRed();
            WestLight.TurnRed();
            
        }

        private void NorthSouthStop()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("NorthSouthStop");
            NorthLight.TurnRed();
            SouthLight.TurnRed();
           
        }

        private void WestEastGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("WestEastGo");
            EastLight.TurnGreen();
            WestLight.TurnGreen();
     
        }

        private void NorthSouthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("NorthSouthGo");
            NorthLight.TurnGreen();
            SouthLight.TurnGreen();
         
        }

        private void btnleft_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.IsEnabled = false;
            NorthSouthStop();
            currentState = state.NORTHSOUTHSTOP;
            dispatcherTimer.IsEnabled = true;
        }
    }
}