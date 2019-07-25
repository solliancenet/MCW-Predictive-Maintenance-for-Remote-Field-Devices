using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;

namespace Fabrikam.FieldDevice.Generator
{
    public class PumpTelemetry
    {
        public IEnumerable<double> MotorPowerKw { get; set; }
        public IEnumerable<double> MotorSpeed { get; set; }
        public IEnumerable<double> PumpRate { get; set; }
        public IEnumerable<double> TimePumpOn { get; set; }
        public IEnumerable<double> CasingFriction { get; set; }

        public PumpTelemetry() { }

        public PumpTelemetry(IEnumerable<double> motorPowerkW,
            IEnumerable<double> motorSpeed,
            IEnumerable<double> pumpRate,
            IEnumerable<double> timePumpOn,
            IEnumerable<double> casingFriction)
        {
            var transformed = TransformValues(motorPowerkW, motorSpeed, pumpRate, timePumpOn, casingFriction);
            MotorPowerKw = transformed.MotorPowerkW;
            MotorSpeed = transformed.MotorSpeed;
            PumpRate = transformed.PumpRate;
            TimePumpOn = transformed.TimePumpOn;
            CasingFriction = transformed.CasingFriction;
        }

        /// <summary>
        /// Returns a new collection of PumpTelemetryItems from the data stored in this object.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PumpTelemetryItem> ToPumpTelemetryItems()
        {
            var numItems = MotorPowerKw.Count();
            var telemetryItems = new List<PumpTelemetryItem>(numItems);
            for (var i = 0; i < numItems; i++)
            {
                telemetryItems.Add(new PumpTelemetryItem(MotorPowerKw.ElementAt(i), MotorSpeed.ElementAt(i),
                    PumpRate.ElementAt(i), TimePumpOn.ElementAt(i), CasingFriction.ElementAt(i)));
            }

            return telemetryItems;
        }

        public void GraduallyDeteriorateNormalToFailed(IEnumerable<double> motorPowerkWNormal,
            IEnumerable<double> motorSpeedNormal,
            IEnumerable<double> pumpRateNormal,
            IEnumerable<double> timePumpOnNormal,
            IEnumerable<double> casingFrictionNormal,
            IEnumerable<double> motorPowerkWFailed,
            IEnumerable<double> motorSpeedFailed,
            IEnumerable<double> pumpRateFailed,
            IEnumerable<double> timePumpOnFailed,
            IEnumerable<double> casingFrictionFailed,
            int failOverXIterations = 250)
        {
            var normal = TransformValues(motorPowerkWNormal, motorSpeedNormal, pumpRateNormal, timePumpOnNormal,
                casingFrictionNormal);
            var failed = TransformValues(motorPowerkWFailed, motorSpeedFailed, pumpRateFailed, timePumpOnFailed,
                casingFrictionFailed);

            if (failOverXIterations > 0)
            {
                var rnd = new Random();
                // Percentage of wobble during degradation when randomizing the next value.
                const double wobblePercentage = 0.02;

                // Retrieve the last normal entry:
                var motorPowerkWLastNormal = normal.MotorPowerkW.Last();
                var motorSpeedLastNormal = normal.MotorSpeed.Last();
                var pumpRateLastNormal = normal.PumpRate.Last();
                var timePumpOnLastNormal = normal.TimePumpOn.Last();
                var casingFrictionLastNormal = normal.CasingFriction.Last();

                // Retrieve the first failed entry:
                var motorPowerkWFirstFailed = failed.MotorPowerkW.First();
                var motorSpeedFirstFailed = failed.MotorSpeed.First();
                var pumpRateFirstFailed = failed.PumpRate.First();
                var timePumpOnFirstFailed = failed.TimePumpOn.First();
                var casingFrictionFirstFailed = failed.CasingFriction.First();

                // How much to subtract from original normal last value in each fail iteration?
                var motorPowerkWSubtractBy = (motorPowerkWLastNormal - motorPowerkWFirstFailed) / failOverXIterations;
                var motorSpeedSubtractBy = (motorSpeedLastNormal - motorSpeedFirstFailed) / failOverXIterations;
                var pumpRateSubtractBy = (pumpRateLastNormal - pumpRateFirstFailed) / failOverXIterations;
                var casingFrictionSubtractBy =
                    (casingFrictionLastNormal - casingFrictionFirstFailed) / failOverXIterations;

                // Keep track of the last value during loop:
                var lastMotorPowerkWValue = motorPowerkWLastNormal;
                var lastMotorSpeedValue = motorSpeedLastNormal;
                var lastPumpRateValue = pumpRateLastNormal;
                var lastTimePumpOnValue = timePumpOnLastNormal;
                var lastCasingFrictionValue = casingFrictionLastNormal;

                // Collection of gradiated values for gradual failure:
                var motorPowerkWGradualFailure = new List<double>();
                var motorSpeedGradualFailure = new List<double>();
                var pumpRateGradualFailure = new List<double>();
                var timePumpOnGradualFailure = new List<double>();
                var casingFrictionGradualFailure = new List<double>();

                // Sawtooth degradation:
                var sampleSize = failOverXIterations / 2;
                var normalToFailureFrequencyDelta =
                    PumpFailedState.TimePumpOn.Frequency - PumpNormalState.TimePumpOn.Frequency;
                var normalToFailureAmplitudeDelta =
                    PumpNormalState.TimePumpOn.Amplitude - PumpFailedState.TimePumpOn.Amplitude;
                // Create sawtooth 3/4 normal frequency and amplitude and half fail-over iteration value for the sample size.
                timePumpOnGradualFailure.AddRange(Generate.Periodic(sampleSize, 10000,
                    normalToFailureFrequencyDelta * 1.5,
                    normalToFailureAmplitudeDelta * 1.5));
                // Create sawtooth delta of normal frequency and amplitude and half fail-over iteration value for the sample size.
                timePumpOnGradualFailure.AddRange(Generate.Periodic(sampleSize, 10000, normalToFailureFrequencyDelta,
                    normalToFailureAmplitudeDelta));

                for (var i = 0; i < failOverXIterations; i++)
                {
                    // Subtract values for this step:
                    lastMotorPowerkWValue -= motorPowerkWSubtractBy;
                    lastMotorSpeedValue -= motorSpeedSubtractBy;
                    lastPumpRateValue -= pumpRateSubtractBy;
                    lastCasingFrictionValue -= casingFrictionSubtractBy;

                    // Add subtracted value to gradual failure collections, or the first failed value, whichever is greater:
                    motorPowerkWGradualFailure.Add(rnd.Next(
                        Convert.ToInt32(lastMotorPowerkWValue) -
                        Convert.ToInt32(lastMotorPowerkWValue * wobblePercentage),
                        Convert.ToInt32(lastMotorPowerkWValue) +
                        Convert.ToInt32(lastMotorPowerkWValue * wobblePercentage))
                    );
                    motorSpeedGradualFailure.Add(rnd.Next(
                        Convert.ToInt32(lastMotorSpeedValue) -
                        Convert.ToInt32(lastMotorSpeedValue * wobblePercentage),
                        Convert.ToInt32(lastMotorSpeedValue) +
                        Convert.ToInt32(lastMotorSpeedValue * wobblePercentage))
                    );
                    pumpRateGradualFailure.Add(rnd.Next(
                        Convert.ToInt32(lastPumpRateValue) -
                        Convert.ToInt32(lastPumpRateValue * wobblePercentage),
                        Convert.ToInt32(lastPumpRateValue) +
                        Convert.ToInt32(lastPumpRateValue * wobblePercentage))
                    );
                    casingFrictionGradualFailure.Add(rnd.Next(
                        Convert.ToInt32(lastCasingFrictionValue) -
                        Convert.ToInt32(lastCasingFrictionValue * wobblePercentage),
                        Convert.ToInt32(lastCasingFrictionValue) +
                        Convert.ToInt32(lastCasingFrictionValue * wobblePercentage))
                    );
                }

                // Concatenate the normal, gradual failure, and failure items and save to the class properties.
                MotorPowerKw = normal.MotorPowerkW.Concat(motorPowerkWGradualFailure).Concat(failed.MotorPowerkW);
                MotorSpeed = normal.MotorSpeed.Concat(motorSpeedGradualFailure).Concat(failed.MotorSpeed);
                PumpRate = normal.PumpRate.Concat(pumpRateGradualFailure).Concat(failed.PumpRate);
                TimePumpOn = normal.TimePumpOn.Concat(timePumpOnGradualFailure).Concat(failed.TimePumpOn);
                CasingFriction = normal.CasingFriction.Concat(casingFrictionGradualFailure)
                    .Concat(failed.CasingFriction);
            }
            else
            {
                // Concatenate the normal and failure items for immediate failure, and save to the class properties.
                MotorPowerKw = normal.MotorPowerkW.Concat(failed.MotorPowerkW);
                MotorSpeed = normal.MotorSpeed.Concat(failed.MotorSpeed);
                PumpRate = normal.PumpRate.Concat(failed.PumpRate);
                TimePumpOn = normal.TimePumpOn.Concat(failed.TimePumpOn);
                CasingFriction = normal.CasingFriction.Concat(failed.CasingFriction);
            }
        }

        /// <summary>
        /// Converts the passed in values to their simplified counterparts and transforms the data.
        /// </summary>
        /// <param name="motorPowerkW"></param>
        /// <param name="motorSpeed"></param>
        /// <param name="pumpRate"></param>
        /// <param name="timePumpOn"></param>
        /// <param name="casingFriction"></param>
        /// <returns>The converted and transformed data.</returns>
        protected (IEnumerable<double> MotorPowerkW,
            IEnumerable<double> MotorSpeed,
            IEnumerable<double> PumpRate,
            IEnumerable<double> TimePumpOn,
            IEnumerable<double> CasingFriction) TransformValues(IEnumerable<double> motorPowerkW,
                IEnumerable<double> motorSpeed,
                IEnumerable<double> pumpRate,
                IEnumerable<double> timePumpOn,
                IEnumerable<double> casingFriction)
        {
            var motorPowerkWTransformed = motorPowerkW.Select(x => Math.Round(x, 2));
            var motorSpeedTransformed = motorSpeed.Select(x => Math.Round(x, 0));
            var pumpRateTransformed = pumpRate.Select(x => Math.Round(x, 1));
            var timePumpOnTransformed = timePumpOn.Select(x => Math.Round(x, 2));
            var casingFrictionTransformed = casingFriction.Select(x => Math.Round(x, 2));

            return (motorPowerkWTransformed, motorSpeedTransformed, pumpRateTransformed, timePumpOnTransformed,
                casingFrictionTransformed);
        }
    }
}