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
    /// Stoplight simulator for the stoplight at state street in provo.
    /// Clicking on a car button simulates a car coming to that stoplight
    /// Lights stay on a min of 5 seconds (if there aren't coming to that light still) and max of 30 seconds if cars are waiting at other lights
    /// The south light stays on by default if no other cars are coming
    /// created by Landon Hulet, Ben Willard, John Meservy, Taylor Curtis
    /// Updated 11/24/14
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    //The following enum is used to keep track of the state of the stoplight
    enum state {DEFAULTSOUTHGO, EASTWESTGO,NORTHGO,ALLSTOP};
    
    public partial class MainWindow : Window
    {

        //creates three threads. The first is a dispatch timer which dictates the order of the lights and checks every five seconds
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        //the wait timer is started if there are no cars waiting and waits 5 seconds to make sure no other cars come.
        System.Windows.Threading.DispatcherTimer waitTimer = new System.Windows.Threading.DispatcherTimer();

        //the stop timer keeps track of the time it takes to transition the light to red.
        System.Windows.Threading.DispatcherTimer stopTimer = new System.Windows.Threading.DispatcherTimer();
        Stopwatch timer = new Stopwatch();
   
        //these are the 4 street light objects
        StreetLight WestLight;
        StreetLight NorthLight;
        StreetLight EastLight;
        StreetLight SouthLight;


        //these are the numbe of cars at each light
        
        int carsAtWest = 0;
        int carsAtNorth = 0;
        int carsAtEast = 0;
        int carsAtSouth = 0;

        //the temp cars keep track of the cars that were at the light when the dispatcher was started so that the lights don't skip cars
        int carsAtWestTemp = 0;
        int carsAtNorthTemp = 0;
        int carsAtEastTemp = 0;
        int carsAtSouthTemp = 0;

        //the app keeps track of the current state of the lights, the state that you want to transition to, and also the past state.
        state currentState;
        state desiredState;
        state pastState = state.ALLSTOP;

        public MainWindow()
        {
            InitializeComponent();

            //sets the current state to the south light green
            currentState = state.DEFAULTSOUTHGO;
            if (currentState == state.DEFAULTSOUTHGO)
            {

                //initializes the stop lights with the ellipse objects and the desired color
                WestLight = new StreetLight(LeftGreen, LeftYellow, LeftRed, color.RED);
                NorthLight = new StreetLight(TopGreen, TopYellow, TopRed, color.RED);
                EastLight = new StreetLight(RightGreen, RightYellow, RightRed, color.RED);
                SouthLight = new StreetLight(BottomGreen, BottomYellow, BottomRed, color.GREEN);
            }
            else throw new System.InvalidOperationException("startState must be SouthGo");

            //sets up the timer intervals
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            waitTimer.Tick += new EventHandler(waitTimer_Tick);
            waitTimer.Interval = new TimeSpan(0, 0, 5);
            stopTimer.Tick += new EventHandler(stopTimer_Tick);
            stopTimer.Interval = new TimeSpan(0, 0, 5);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //calls the main thread every five sends when there is a car waiting
            everyFiveSecondsWhileCars();
            

        }

        private void everyFiveSecondsWhileCars()
        {

            //checks to see if the state has changed
            if (currentState != pastState)
            {
                //if so, it resets the thirty second timer
                timer.Restart();

            }
           
            
            //brings the current state into the past so you can make the comparision the next time through
            pastState = currentState;

            //checks to see how many cars there are at the south light
            if (carsAtSouthTemp > 0)
            {
                //says that no more cars are waiting because we are going to turn the light green
                carsAtSouthTemp = 0;
               

                //if the light isn't already green stop all lights and set the desired state
                if (currentState != state.DEFAULTSOUTHGO)
                {
                    desiredState = state.DEFAULTSOUTHGO;
                    StopAllAndGo();
                }
                else
                {
                    carsAtSouth = 0;
                }
                //
            }
            //checks to see  if there are cars at the east and west lights

            else if (carsAtEastTemp > 0 || carsAtWestTemp > 0)
            {
                //says that no more cars are waiting because we are going to turn the light green

                carsAtEastTemp = 0;
                
                carsAtWestTemp = 0;
    

                //if the light isn't already green stop all lights and set the desired state

                if (currentState != state.EASTWESTGO)
                {
                    desiredState = state.EASTWESTGO;
                    StopAllAndGo();
                }
                else
                {
                    carsAtEast = 0;

                    carsAtWest = 0;
                }

            }
            //checks to see  if there are cars at the north lights

            else if (carsAtNorthTemp > 0)
            {
                //checks to see  if there are cars at the north lights

                carsAtNorthTemp = 0;
          

                //if the light isn't already green stop all lights and set the desired state

                if (currentState != state.NORTHGO)
                {
                   
               
                    desiredState = state.NORTHGO;
                    StopAllAndGo();
                }
                else { carsAtNorth = 0; }
            }

                //if there isn't any cars waiting anywhere, go back to the default light color and stop the timer and the thread
            else if (!isAnyCarsWaiting())
            {
                timer.Stop();
                if (currentState != state.DEFAULTSOUTHGO)
                {
                    desiredState = state.DEFAULTSOUTHGO;
                    StopAllAndGo();
                }
                dispatcherTimer.Stop();
            }
                //if there are cars still waiting, create a snapshot of the cars
            else
            {
                copyCarsToTemp();
                dispatcherTimer.Stop();
                dispatcherTimer.Start();
                everyFiveSecondsWhileCars();
                
                
               

            }
        }

       
            //this turns the east and west light green on call 
        private void WestEastGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("WestEastGo");
            EastLight.TurnGreen();
            WestLight.TurnGreen();

            //decrements the cars at these lights
            carsAtEast = 0;
            carsAtWest = 0;

            //sets the state
            currentState = state.EASTWESTGO;

            //turns on the right arrow
            rightarrow.Visibility = System.Windows.Visibility.Visible;
        }

        //this turns the north light green on call 

        private void NorthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("NorthGo");
            NorthLight.TurnGreen();

            //decrements the cars at this light

            carsAtNorth = 0;
            //sets the state

            currentState = state.NORTHGO;
           
         
        }

        private void SouthGo()
        {
            Debug.Write(DateTime.Now);
            Debug.Write(": ");
            Debug.WriteLine("SouthGo");
            SouthLight.TurnGreen();

            //decrements the cars at this light


            carsAtSouth = 0;

            //sets the state

            currentState = state.DEFAULTSOUTHGO;
            //turns on the left arrow
            leftarrow.Visibility = System.Windows.Visibility.Visible;
           
            //turns on the right arrow
            rightarrow.Visibility = System.Windows.Visibility.Visible;
        }


      



        private void StopAllAndGo() //turns all the lights red
        {
            //stops the main thread for now. At least until the light is ready to go.
            dispatcherTimer.Stop();

            //sets the state
            currentState = state.ALLSTOP;

            NorthLight.TurnRed();
            SouthLight.TurnRed();
            EastLight.TurnRed();
            WestLight.TurnRed();

            //turns off the turn arrows
            rightarrow.Visibility = System.Windows.Visibility.Hidden;
            leftarrow.Visibility = System.Windows.Visibility.Hidden;
            
            //starts the timer that waits until the lights are red
            stopTimer.Start();
        }


        private void stopTimer_Tick(object sender, EventArgs e) //calls the approriate method based on the desired state
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

            //stops the timer that called this
            stopTimer.Stop();

            //starts the main thread again
            dispatcherTimer.Stop();
            dispatcherTimer.Start();
        }

        private void CarAtWest(object sender, RoutedEventArgs e) //button press
        {

            //checks if there are any other cars waiting
            if (isAnyCarsWaiting())
            {
                //if the light is already green
                if (currentState == state.EASTWESTGO)
                {
                    //if the timer is less than 30 seconds
                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        //stops the main timer, to prevent the light from turning red for another 5 seconds
                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }

                }
                else                 //if the light isn't green increment the number of cars waiting

                carsAtWest += 1;
            }
            else //if there aren't any cars waiting, say there is a car waiting and wait five seconds before calling the main thread
            {
                carsAtWest += 1;
                WaitFiveSeconds();
            }
        }

        private void CarAtEast(object sender, RoutedEventArgs e)  //button press
        {
            //checks if there are any other cars waiting

            if (isAnyCarsWaiting())
            {
                //if the light is already green

                if (currentState == state.EASTWESTGO)
                {
                    //if the timer is less than 30 seconds

                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        //stops the main timer, to prevent the light from turning red for another 5 seconds

                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }
                    //if the light isn't green increment the number of cars waiting

                } else 
                carsAtEast += 1;
            }
            else //if there aren't any cars waiting, say there is a car waiting and wait five seconds before calling the main thread
            {
                carsAtEast += 1;
                WaitFiveSeconds();
            }
        }

        private void CarAtSouth(object sender, RoutedEventArgs e)  //button press
        {            
            //checks if there are any other cars waiting

            if (isAnyCarsWaiting())
            {
                //if the light is already green

                if (currentState == state.DEFAULTSOUTHGO)
                {
                    //if the timer is less than 30 seconds

                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        //stops the main timer, to prevent the light from turning red for another 5 seconds

                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }
                //if the light isn't green increment the number of cars waiting
                } else
                carsAtSouth += 1;
            }
            else //if there aren't any cars waiting, say there is a car waiting and wait five seconds before calling the main thread
            {
                carsAtSouth += 1;
                WaitFiveSouthSeconds();
            }
        }

        private void CarAtNorth(object sender, RoutedEventArgs e)
        {
            //checks if there are any other cars waiting

            if (isAnyCarsWaiting())
            {
                //if the light is already green

                if (currentState == state.NORTHGO)
                {
                    //if the timer is less than 30 seconds

                    if (timer.Elapsed < new TimeSpan(0, 0, 30))
                    {
                        //stops the main timer, to prevent the light from turning red for another 5 seconds

                        dispatcherTimer.Stop();
                        dispatcherTimer.Start();
                    }
                
                    //if the light isn't green increment the number of cars waiting

                } else carsAtNorth += 1;
            }
            else //if there aren't any cars waiting, say there is a car waiting and wait five seconds before calling the main thread
            {
                carsAtNorth += 1;
                WaitFiveSeconds();
            }

        }

        private void WaitFiveSouthSeconds()
        {
            //checks to make sure that we aren't in the default state before setting things in motion
            if (currentState != state.DEFAULTSOUTHGO) 
            { 
                     waitTimer.Start();
                }
        }

        private void WaitFiveSeconds()
        {
            //starts the wait timer that waits five seconds
            waitTimer.Start();
        }

        private bool isAnyCarsWaiting() //checks to see if any cars are waiting
        {
            return Convert.ToBoolean(carsAtSouth + carsAtNorth + carsAtWest + carsAtEast);
        }
        private void waitTimer_Tick(object sender, EventArgs e)
        {

            //stops the wait timer, creates a snapshot and starts the main thread
            waitTimer.Stop();
            copyCarsToTemp();


            dispatcherTimer.Start();
            everyFiveSecondsWhileCars();
            
            
        }

        private void copyCarsToTemp() // this just creates the snapshot by copying the number of cars at each light into temp variables
        {
            carsAtEastTemp = carsAtEast;
            carsAtNorthTemp = carsAtNorth;
            carsAtSouthTemp = carsAtSouth;
            carsAtWestTemp = carsAtWest;
        }
 



    }
}