namespace Fabrikam.FieldDevice.Generator
{
    public static class PumpFailedState
    {
        public struct MotorPowerkW
        {
            public static double StandardDeviation = 1.4;
            public static double SamplingRate =10000;
            public static double Frequency = 70;
            public static double Amplitude = 1.8;
            public static double InitialValue = 15.0;
        }

        public struct MotorSpeed
        {
            public static double StandardDeviation = 1.025;
            public static double SamplingRate = 10000;
            public static double Frequency = 70;
            public static double Amplitude = 2.2;
            public static double InitialValue = 42.0;
        }

        public struct PumpRate
        {
            public static double StandardDeviation = 1.6;
            public static double InitialValue = 12.5;
        }

        public struct CasingFriction
        {
            public static double StandardDeviation = 1.055;
            public static double InitialValue = 600.0;
        }

        public struct TimePumpOn
        {
            public static double SamplingRate = 10000;
            public static double Frequency = 90;
            public static double Amplitude = 350;
        }
    }
}