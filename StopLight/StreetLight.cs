using StopLight;
using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

//enum color { RED, GREEN };

public partial class StreetLight
{
    System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
    private Ellipse greenLight;
    private Ellipse yellowLight;
    private Ellipse redLight;
    private TurnArrow rightTurnArrow;
    private TurnArrow leftTurnArrow;
    SolidColorBrush red = new SolidColorBrush(Colors.Red);
    SolidColorBrush green = new SolidColorBrush(Colors.Green);
    SolidColorBrush yellow = new SolidColorBrush(Colors.Yellow);
    SolidColorBrush gray = new SolidColorBrush(Colors.LightGray);
    Sensor sensor = new Sensor();

    public StreetLight(Ellipse green, Ellipse yellow, Ellipse red, color startingColor)
    {
        greenLight = green;
        yellowLight = yellow;
        redLight = red;
        if (startingColor == color.GREEN)
        {
            GoGreen();
        }
        else if(startingColor == color.RED)
        {
            GoRed();
        } else throw new System.ArgumentException("StreetLights can only be green or red on start");
    }

    /*public StreetLight(Ellipse green, Ellipse yellow, Ellipse red)
    {
        greenLight = green;
        yellowLight = yellow;
        redLight = red;
    }*/

    public void TurnGreen()
    {
        GoGreen();
    }

    public void TurnRed()
    {
        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
        if (isGreen())
        {
            GoYellow();
            dispatcherTimer.Start();
        }

        
    }
    private void dispatcherTimer_Tick(object sender, EventArgs e)
    {

        GoRed();
        dispatcherTimer.Stop();
    }

    private bool isGreen()
    {
        return greenLight.Fill == green;
    }

    private void GoRed()
    {
        Debug.Write(DateTime.Now);
        Debug.Write(": ");
        Debug.WriteLine("Going Red!");
        greenLight.Fill = gray;
        yellowLight.Fill = gray;
        redLight.Fill = red;
    }

    private void GoYellow()
    {
        Debug.Write(DateTime.Now);
        Debug.Write(": ");
        Debug.WriteLine("Going Yellow!");
        greenLight.Fill = gray;
        yellowLight.Fill = yellow;
        redLight.Fill = gray;
    }

    private void GoGreen()
    {
        Debug.Write(DateTime.Now);
        Debug.Write(": ");
        Debug.WriteLine("Going Green!");
        greenLight.Fill = green;
        yellowLight.Fill = gray;
        redLight.Fill = gray;
    }



}