using StopLight;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

//enum color { RED, GREEN };

public partial class StreetLight
{
    //creates a new thread in charge of waiting to turn the light red after it goes yellow
    System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
    private Ellipse greenLight;
    private Ellipse yellowLight;
    private Ellipse redLight;
    SolidColorBrush red = new SolidColorBrush(Colors.Red);
    SolidColorBrush green = new SolidColorBrush(Colors.Green);
    SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
    SolidColorBrush gray = new SolidColorBrush(Colors.LightGray);

    public StreetLight(Ellipse green, Ellipse yellow, Ellipse red, color startingColor) //constructor
    {

        //brings in the objects
        greenLight = green;
        yellowLight = yellow;
        redLight = red;

        //it turns the lights the right starting color based on the param
        if (startingColor == color.GREEN)
        {
            GoGreen();
        }
        else if(startingColor == color.RED)
        {
            GoRed();
        } else throw new System.ArgumentException("StreetLights can only be green or red on start");
    }

    public void TurnGreen()
    {
        GoGreen();
    }

    public void TurnRed()
    {
        //sets a wait time for three seconds that happens right after the light turns yellow
        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
        if (isGreen()) //we'll only turn the light yellow if it isn't green
        {
            GoYellow();
            dispatcherTimer.Start();
        }

        
    }
    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {
        //as soon as the wait is up, we can turn the light red
        GoRed();
        dispatcherTimer.Stop();
    }

    private bool isGreen()
    {
        //checks to see if the ellipse is filled with green
        return greenLight.Fill == green;
    }

    private void GoRed()
    {
        //fills the ellipses grey and red
        Debug.Write(DateTime.Now);
        Debug.Write(": ");
        Debug.WriteLine("Going Red!");
        greenLight.Fill = gray;
        yellowLight.Fill = gray;
        redLight.Fill = red;
    }

    private void GoYellow()
    {
        //fills the ellipses grey and yellow

        Debug.Write(DateTime.Now);
        Debug.Write(": ");
        Debug.WriteLine("Going Yellow!");
        greenLight.Fill = gray;
        yellowLight.Fill = yellow;
        redLight.Fill = gray;
    }

    private void GoGreen()
    {
        //fills the ellipses green and yellow

        Debug.Write(DateTime.Now);
        Debug.Write(": ");
        Debug.WriteLine("Going Green!");
        greenLight.Fill = green;
        yellowLight.Fill = gray;
        redLight.Fill = gray;
    }



}