using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using MathNet.Numerics;

namespace Fabrikam.FieldDevice.Generator
{
    public class GenerateData
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Generates pump telemetry data to send to IoT Central.
        /// </summary>
        /// <param name="sampleSize">The number of telemetry items to generate (excluding the number of gradually failed items, if applicable).</param>
        /// <param name="causeFailure">Whether to cause a pump failure.</param>
        /// <param name="failOverXIterations">If there is a pump failure, how gradual should it be? Enter 0 for immediate failure.</param>
        /// <returns></returns>
        public static IEnumerable<PumpTelemetryItem> GeneratePumpTelemetry(int sampleSize = 500, bool causeFailure = true,
            int failOverXIterations = 250)
        {
            // If causing a failure, set the good sample size to 1/8.
            var normalSampleSize = causeFailure ? sampleSize / 8 : sampleSize;

            // Generate normal pump operation:
            var motorPowerkW = Generate.Sinusoidal(normalSampleSize, PumpNormalState.MotorPowerkW.SamplingRate, PumpNormalState.MotorPowerkW.Frequency, PumpNormalState.MotorPowerkW.Amplitude, RandomizeInitialValue(PumpNormalState.MotorPowerkW.InitialValue));
            var motorSpeed = Generate.Sinusoidal(normalSampleSize, PumpNormalState.MotorSpeed.SamplingRate, PumpNormalState.MotorSpeed.Frequency, PumpNormalState.MotorSpeed.Amplitude, RandomizeInitialValue(PumpNormalState.MotorSpeed.InitialValue));
            var pumpRate = Generate.Normal(normalSampleSize, RandomizeInitialValue(PumpNormalState.PumpRate.InitialValue), PumpNormalState.PumpRate.StandardDeviation);
            var timePumpOn = Generate.Periodic(normalSampleSize, PumpNormalState.TimePumpOn.SamplingRate, RandomizeInitialValue(PumpNormalState.TimePumpOn.Frequency), RandomizeInitialValue(PumpNormalState.TimePumpOn.Amplitude));
            var casingFriction = Generate.Normal(normalSampleSize, RandomizeInitialValue(PumpNormalState.CasingFriction.InitialValue), PumpNormalState.CasingFriction.StandardDeviation);

            var telemetry = new PumpTelemetry();

            if (causeFailure)
            {
                var failedSampleSize = sampleSize - normalSampleSize;

                // Generate failing pump operation:
                var motorPowerkWFailing = Generate.Sinusoidal(failedSampleSize, PumpFailedState.MotorPowerkW.SamplingRate, PumpFailedState.MotorPowerkW.Frequency, PumpFailedState.MotorPowerkW.Amplitude, RandomizeInitialValue(PumpFailedState.MotorPowerkW.InitialValue));
                var motorSpeedFailing = Generate.Sinusoidal(failedSampleSize, PumpFailedState.MotorSpeed.SamplingRate, PumpFailedState.MotorSpeed.Frequency, PumpFailedState.MotorSpeed.Amplitude, RandomizeInitialValue(PumpFailedState.MotorSpeed.InitialValue));
                var pumpRateFailing = Generate.Normal(failedSampleSize, RandomizeInitialValue(PumpFailedState.PumpRate.InitialValue), PumpFailedState.PumpRate.StandardDeviation);
                var timePumpOnFailing = Generate.Periodic(failedSampleSize, PumpFailedState.TimePumpOn.SamplingRate, RandomizeInitialValue(PumpFailedState.TimePumpOn.Frequency), RandomizeInitialValue(PumpFailedState.TimePumpOn.Amplitude));
                var casingFrictionFailing = Generate.Normal(failedSampleSize, RandomizeInitialValue(PumpFailedState.CasingFriction.InitialValue), PumpFailedState.CasingFriction.StandardDeviation);

                telemetry.GraduallyDeteriorateNormalToFailed(motorPowerkW, motorSpeed, pumpRate, timePumpOn,
                    casingFriction,
                    motorPowerkWFailing, motorSpeedFailing, pumpRateFailing, timePumpOnFailing, casingFrictionFailing,
                    failOverXIterations);
            }
            else
            {
                telemetry = new PumpTelemetry(motorPowerkW, motorSpeed, pumpRate, timePumpOn, casingFriction);
            }

            return telemetry.ToPumpTelemetryItems();
        }

        /// <summary>
        /// Creates a random value in a range of += 2% of the initial value.
        /// </summary>
        /// <param name="initialValue">The initial value you wish to randomize.</param>
        /// <returns></returns>
        private static double RandomizeInitialValue(double initialValue)
        {
            var upper = initialValue + (initialValue * 0.02);
            var lower = initialValue - (initialValue * 0.02);

            return _random.NextDouble() * (upper - lower) + lower;
        }

        /// <summary>
        /// Generates pump telemetry data for training an ML model and outputs the data to a console window and/or CSV files.
        /// </summary>
        /// <param name="sampleSize">The number of telemetry items to generate (excluding the number of gradually failed items, if applicable).</param>
        /// <param name="causeFailure">Whether to cause a pump failure.</param>
        /// <param name="failOverXIterations">If there is a pump failure, how gradual should it be? Enter 0 for immediate failure.</param>
        /// <param name="outputToConsole">If true, displays all of the generated telemetry in the console window.</param>
        /// <param name="writeFile">If true, writes the telemetry to a CSV file that can be used to train the model.</param>
        public static void GenerateModelTrainingData(int sampleSize = 500, bool causeFailure = true,
            int failOverXIterations = 250, bool outputToConsole = true, bool writeFile = true)
        {
            // Cut the sample size in half if we are generating failure data sets.
            sampleSize = causeFailure ? sampleSize / 2 : sampleSize;

            // Generate normal pump operation:
            var motorPowerkW = Generate.Sinusoidal(sampleSize, PumpNormalState.MotorPowerkW.SamplingRate, PumpNormalState.MotorPowerkW.Frequency, PumpNormalState.MotorPowerkW.Amplitude, PumpNormalState.MotorPowerkW.InitialValue);
            var motorSpeed = Generate.Sinusoidal(sampleSize, PumpNormalState.MotorSpeed.SamplingRate, PumpNormalState.MotorSpeed.Frequency, PumpNormalState.MotorSpeed.Amplitude, PumpNormalState.MotorSpeed.InitialValue);
            var pumpRate = Generate.Normal(sampleSize, PumpNormalState.PumpRate.InitialValue, PumpNormalState.PumpRate.StandardDeviation);
            var timePumpOn = Generate.Periodic(sampleSize, PumpNormalState.TimePumpOn.SamplingRate, PumpNormalState.TimePumpOn.Frequency, PumpNormalState.TimePumpOn.Amplitude);
            var casingFriction = Generate.Normal(sampleSize, PumpNormalState.CasingFriction.InitialValue, PumpNormalState.CasingFriction.StandardDeviation);

            var telemetry = new PumpTelemetry();

            if (causeFailure)
            {
                // Generate failing pump operation:
                var motorPowerkWFailing = Generate.Sinusoidal(sampleSize, PumpFailedState.MotorPowerkW.SamplingRate, PumpFailedState.MotorPowerkW.Frequency, PumpFailedState.MotorPowerkW.Amplitude, PumpFailedState.MotorPowerkW.InitialValue);
                var motorSpeedFailing = Generate.Sinusoidal(sampleSize, PumpFailedState.MotorSpeed.SamplingRate, PumpFailedState.MotorSpeed.Frequency, PumpFailedState.MotorSpeed.Amplitude, PumpFailedState.MotorSpeed.InitialValue);
                var pumpRateFailing = Generate.Normal(sampleSize, PumpFailedState.PumpRate.InitialValue, PumpFailedState.PumpRate.StandardDeviation);
                var timePumpOnFailing = Generate.Periodic(sampleSize, PumpFailedState.TimePumpOn.SamplingRate, PumpFailedState.TimePumpOn.Frequency, PumpFailedState.TimePumpOn.Amplitude);
                var casingFrictionFailing = Generate.Normal(sampleSize, PumpFailedState.CasingFriction.InitialValue, PumpFailedState.CasingFriction.StandardDeviation);

                telemetry.GraduallyDeteriorateNormalToFailed(motorPowerkW, motorSpeed, pumpRate, timePumpOn,
                    casingFriction,
                    motorPowerkWFailing, motorSpeedFailing, pumpRateFailing, timePumpOnFailing, casingFrictionFailing,
                    failOverXIterations);
            }
            else
            {
                telemetry = new PumpTelemetry(motorPowerkW, motorSpeed, pumpRate, timePumpOn, casingFriction);
            }

            if (writeFile)
            {
                // Write to CSV file:
                var fileName = "TelemetryNormal.csv";
                if (causeFailure)
                {
                    fileName = failOverXIterations > 0
                        ? "TelemetryWithGradualFailures.csv"
                        : "TelemetryWithImmediateFailures.csv";
                }

                var telemetryItems = telemetry.ToPumpTelemetryItems();

                using (var writer = new StreamWriter(fileName))
                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.RegisterClassMap<PumpTelemetryItemMap>();
                    csv.WriteRecords(telemetryItems);
                }

                Console.WriteLine($"{fileName} successfully written.");
            }

            if (outputToConsole) Console.WriteLine(JsonConvert.SerializeObject(telemetry));
        }
    }

    /// <summary>
    /// CsvWriter map.
    /// </summary>
    public sealed class PumpTelemetryItemMap : ClassMap<PumpTelemetryItem>
    {
        public PumpTelemetryItemMap()
        {
            Map(m => m.MotorPowerKw).Index(0);
            Map(m => m.MotorSpeed).Index(1);
            Map(m => m.PumpRate).Index(2);
            Map(m => m.TimePumpOn).Index(3);
            Map(m => m.CasingFriction).Index(4);
        }
    }
}
