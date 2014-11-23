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
        System.Windows.Threading.DispatcherTimer waitTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer stopTimer = new System.Windows.Threading.DispatcherTimer();
        Stopwatch timer = new Stopwatch();
   

        StreetLight WestLight;
        StreetLight NorthLight;
        StreetLight EastLight;
        StreetLight SouthLight;

        int carsAtWest = 0;
        int carsAtNorth = 0;
        int carsAtEast = 0;
        int carsAtSouth = 0;
        int carsAtWestTemp = 0;
        int carsAtNorthTemp = 0;
        int carsAtEastTemp = 0;
        int carsAtSouthTemp = 0;

        state currentState;
        state desiredState;
        state pastState = state.ALLSTOP;

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
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            waitTimer.Tick += new EventHandler(waitTimer_Tick);
            waitTimer.Interval = new TimeSpan(0, 0, 5);
            stopTimer.Tick += new EventHandler(stopTimer_Tick);
            stopTimer.Interval = new TimeSpan(0, 0, 5);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            everyFiveSecondsWhileCars();
            

            /*else if (currentState == state.EASTWESTSTOP)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);



                NorthGo();
                
            }

            else if (currentState == state.NORTHGO)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);
                desiredState == state.
                StopAll();
            
            }

            else if (currentState == state.NORTHSTOP)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);

                SouthGo();
               

            }

            else if (currentState == state.DEFAULTSOUTHGO)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);



                
            }
            else if (currentState == state.SOUTHSTOP)
            {
                Debug.Write(DateTime.Now);
                Debug.Write(": ");
                Debug.WriteLine(currentState);
                WestEastGo();
               

                
            }
            else if (currentState == state.EASTWESTGO)
            {
                
                Debug.Write(DateTime.Now);
                Debug.Write(": State:");
                Debug.WriteLine(currentState);
                WestEastStop();
              

                
            }*/
        }

        private void everyFiveSecondsWhileCars()
        {

            if (currentState != pastState)
            {
                
                timer.Restart();

            }
            pastState = currentState;

            if (carsAtSouthTemp > 0)
            {
                carsAtSouthTemp = 0;

                if (currentState == state.DEFAULTSOUTHGO)
                {
                    /*if (timer.Elapsed > new TimeSpan(0,0,30))
                    {

                        everyFiveSecondsWhileCars();
                        return;
                    }*/
                }
                else
                {
                    desiredState = state.DEFAULTSOUTHGO;
                    StopAllAndGo();
                }
            }
            else if (carsAtEastTemp > 0 || carsAtWestTemp > 0)
            {
                
                carsAtEastTemp = 0;
                
                carsAtWestTemp = 0;
                if (currentState == state.EASTWESTGO)
                {
                    /*if (timer.Elapsed > new TimeSpan(0,0,30))
                    {
                        everyFiveSecondsWhileCars();

                        return;
                    }*/
                }
                else
                {
                    desiredState = state.EASTWESTGO;
                    StopAllAndGo();
                }
            }

            else if (carsAtNorthTemp > 0)
            {
                carsAtNorthTemp = 0;
                if (currentState == state.NORTHGO)
                {
                   //if (timer.Elapsed > new TimeSpan(0,0,30))
                   // {
                   //     everyFiveSecondsWhileCars();

                   //     return;
                   // }
                }
                else
                {


                    desiredState = state.NORTHGO;
                    StopAllAndGo();
                }
            }
            else if (!isAnyCarsWaiting())
            {
                timer.Stop();
                if (currentState == state.DEFAULTSOUTHGO)
                {

                }
                else
                {
                    desiredState = state.DEFAULTSOUTHGO;
                    StopAllAndGo();
                }
                dispatcherTimer.Stop();
            }
            else
            {
                copyCarsToTemp();
                everyFiveSecondsWhileCars();
                return;

            }
        }

       

        private void WestEastGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("WestEastGo");
            EastLight.TurnGreen();
            WestLight.TurnGreen();
            carsAtEast = 0;
            carsAtWest = 0;
            currentState = state.EASTWESTGO;
            rightarrow.Visibility = System.Windows.Visibility.Visible;
        }

        private void NorthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("NorthGo");
            NorthLight.TurnGreen();

            carsAtNorth = 0;
            currentState = state.NORTHGO;
           
         
        }

        private void SouthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("SouthGo");
            SouthLight.TurnGreen();

            carsAtSouth = 0;
            currentState = state.DEFAULTSOUTHGO;
            leftarrow.Visibility = System.Windows.Visibility.Visible;
            rightarrow.Visibility = System.Windows.Visibility.Visible;
        }


      



        private void StopAllAndGo()
        {
            dispatcherTimer.Stop();
            currentState = state.ALLSTOP;
            NorthLight.TurnRed();
            SouthLight.TurnRed();
            EastLight.TurnRed();
            WestLight.TurnRed();
            rightarrow.Visibility = System.Windows.Visibility.Hidden;
            leftarrow.Visibility = System.Windows.Visibility.Hidden;
            
            stopTimer.Start();
        }


        private void stopTimer_Tick(object sender, EventArgs e)
        {
            if(currentState == state.ALLSTOP){
                if (desiredState == state.NORTHGO)
                {
                    NorthGo();
                }
                else if (desiredState == state.EASTWESTGO)
                {
                    WestEastGo();
                }
                else if (desiredState == state.DEFAULTSOUTHGO)
                {
                    SouthGo();
                }
            }
            stopTimer.Stop();
            dispatcherTimer.Start();
        }

        private void CarAtWest(object sender, RoutedEventArgs e)
        {
            if (isAnyCarsWaiting())
            {
                if (currentState == state.EASTWESTGO)
                {
                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }

                } else
                carsAtWest += 1;
            }
            else
            {
                carsAtWest += 1;
                WaitFiveSeconds();
            }
        }

        private void CarAtEast(object sender, RoutedEventArgs e)
        {
            if (isAnyCarsWaiting())
            {
                if (currentState == state.EASTWESTGO)
                {
                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }

                } else
                carsAtEast += 1;
            }
            else
            {
                carsAtEast += 1;
                WaitFiveSeconds();
            }
        }

        private void CarAtSouth(object sender, RoutedEventArgs e)
        {
            if (isAnyCarsWaiting())
            {
                if (currentState == state.DEFAULTSOUTHGO)
                {
                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }

                } else
                carsAtSouth += 1;
            }
            else
            {
                carsAtSouth += 1;
                WaitFiveSouthSeconds();
            }
        }

        private void CarAtNorth(object sender, RoutedEventArgs e)
        {
            if (isAnyCarsWaiting())
            {
                if (currentState == state.NORTHGO)
                {
                    if (timer.Elapsed < new TimeSpan(0, 0, 25))
                    {
                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }

                } else carsAtNorth += 1;
            }
            else
            {
                carsAtNorth += 1;
                WaitFiveSeconds();
            }

        }

        private void WaitFiveSouthSeconds()
        {
            if (currentState == state.DEFAULTSOUTHGO)
            {

            }
            else waitTimer.Start();
        }

        private void WaitFiveSeconds()
        {
            waitTimer.Start();
        }

        private bool isAnyCarsWaiting()
        {
            return Convert.ToBoolean(carsAtSouth + carsAtNorth + carsAtWest + carsAtEast);
        }
        private void waitTimer_Tick(object sender, EventArgs e)
        {
            waitTimer.Stop();
            copyCarsToTemp();


            dispatcherTimer.Start();
            everyFiveSecondsWhileCars();
            
            
        }

        private void copyCarsToTemp()
        {
            carsAtEastTemp = carsAtEast;
            carsAtNorthTemp = carsAtNorth;
            carsAtSouthTemp = carsAtSouth;
            carsAtWestTemp = carsAtWest;
        }
 



    }
}