namespace Fabrikam.FieldDevice.Generator
{
    public static class PumpNormalState
    {
        public struct MotorPowerkW
        {
            public static double StandardDeviation = 1.0;
            public static double SamplingRate = 10000;
            public static double Frequency = 85;
            public static double Amplitude = 2.4;
            public static double InitialValue = 70.0;
        }

        public struct MotorSpeed
        {
            public static double StandardDeviation = 0.5;
            public static double SamplingRate = 10000;
            public static double Frequency = 85;
            public static double Amplitude = 3.25;
            public static double InitialValue = 200.0;
        }

        public struct PumpRate
        {
            public static double StandardDeviation = 1.5;
            public static double InitialValue = 60.0;
        }

        public struct CasingFriction
        {
            public static double StandardDeviation = 1.8;
            public static double InitialValue = 1450.0;
        }

        public struct TimePumpOn
        {
            public static double SamplingRate = 10000;
            public static double Frequency = 40;
            public static double Amplitude = 800;
        }
    }
}