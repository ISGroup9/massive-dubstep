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
    enum state {DEFAULTSOUTHGO, SOUTHSTOP, EASTWESTGO, EASTWESTSTOP,NORTHGO,NORTHSTOP,ALLSTOP};
    //enum color {RED, GREEN};
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer northTimer = new System.Windows.Threading.DispatcherTimer();
        //int iLeftRight;

        StreetLight WestLight;
        StreetLight NorthLight;
        StreetLight EastLight;
        StreetLight SouthLight;

        int carsAtWest = 0;
        int carsAtNorth = 0;
        int carsAtEast = 0;
        int carsAtSouth = 0;

        state currentState;

        public MainWindow()
        {
            InitializeComponent();
            currentState = state.DEFAULTSOUTHGO;
            if (currentState == state.DEFAULTSOUTHGO)
            {
                WestLight = new StreetLight(LeftGreen, LeftYellow, LeftRed, color.RED);
                NorthLight = new StreetLight(TopGreen, TopYellow, TopRed, color.RED);
                EastLight = new StreetLight(RightGreen, RightYellow, RightRed, color.RED);
                SouthLight = new StreetLight(BottomGreen, BottomYellow, BottomRed, color.GREEN);
            }
            else throw new System.InvalidOperationException("startState must be SouthGo");

            
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



                NorthGo();
                currentState = state.NORTHGO;
            }

            else if (currentState == state.NORTHGO)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);

                NorthStop();
                currentState = state.NORTHSTOP;
            }

            else if (currentState == state.NORTHSTOP)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);

                SouthGo();
                currentState = state.DEFAULTSOUTHGO;

            }

            else if (currentState == state.DEFAULTSOUTHGO)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);




                SouthStop();
                currentState = state.SOUTHSTOP;
            }
            else if (currentState == state.SOUTHSTOP)
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

        private void SouthStop()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("SouthStop");
            SouthLight.TurnRed();

            currentState = state.SOUTHSTOP;
           
        }

        private void NorthStop()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("NorthStop");
            NorthLight.TurnRed();

            currentState = state.NORTHSTOP;

        }

        private void WestEastGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("WestEastGo");
            EastLight.TurnGreen();
            WestLight.TurnGreen();
     
        }

        private void NorthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("NorthGo");
            NorthLight.TurnGreen();


            currentState = state.NORTHGO;
         
        }

        private void SouthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("SouthGo");
            SouthLight.TurnGreen();
            currentState = state.DEFAULTSOUTHGO;
        }

        private void btnleft_Click(object sender, RoutedEventArgs e)
        {

            dispatcherTimer.IsEnabled = false;
            SouthStop();
            currentState = state.SOUTHSTOP;
            dispatcherTimer.IsEnabled = true;
        }

        private void CarAtNorth(object sender, RoutedEventArgs e)
        {
            carsAtNorth += 1;
            System.Windows.Threading.DispatcherTimer northTimer = new System.Windows.Threading.DispatcherTimer();
            northTimer.Tick += new EventHandler(northTimer_Tick);
            northTimer.Interval = new TimeSpan(0, 0, 5);
            northTimer.Start();
            StopAll();
            NorthGo();

        }

        private void northTimer_Tick(object sender, EventArgs e)
        {
            if(carsAtSouth==0 & carsAtWest ==0 & carsAtEast ==0)
            {
                StopAll();
            }
            northTimer.Stop();
        }

        private void StopAll()
        {
            throw new NotImplementedException();
        }
    }
}