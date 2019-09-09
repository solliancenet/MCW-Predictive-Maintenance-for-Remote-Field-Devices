namespace Fabrikam.Oil.Pumps
{
    public class Telemetry 
    {
        public double MotorPowerKw { get; set; }
        public double MotorSpeed { get; set; }
        public double PumpRate { get; set; }
        public double TimePumpOn { get; set; }
        public double CasingFriction { get; set; }
        public string PowerState {get; set;}

    }
}