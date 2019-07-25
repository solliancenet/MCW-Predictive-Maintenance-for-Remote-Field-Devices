using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Fabrikam.FieldDevice.Generator.OutputHelpers;
using Microsoft.Extensions.Configuration;

namespace Fabrikam.FieldDevice.Generator
{
    internal class Program
    {
        private static IConfigurationRoot _configuration;
        private static List<PumpDevice> devices = new List<PumpDevice>();
        private static readonly object LockObject = new object();
        // AutoResetEvent to signal when to exit the application.
        private static readonly AutoResetEvent WaitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            // Setup configuration to either read from the appsettings.json file (if present) or environment variables.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var cancellationSource = new CancellationTokenSource();
            var cancellationToken = cancellationSource.Token;

            WriteLineInColor("Pump Telemetry Generator", ConsoleColor.White);
            Console.WriteLine("=============");
            WriteLineInColor("** Enter 1 to generate and send pump device telemetry to IoT Central.", ConsoleColor.Green);
            WriteLineInColor("** Enter 2 to generate anomaly model training data in CSV files.", ConsoleColor.Green);
            Console.WriteLine("=============");
            Console.WriteLine(string.Empty);
            WriteLineInColor("Press Ctrl+C or Ctrl+Break to cancel while generator is running.", ConsoleColor.Cyan);
            Console.WriteLine(string.Empty);

            // Handle Control+C or Control+Break.
            Console.CancelKeyPress += (o, e) =>
            {
                WriteLineInColor("Stopped generator. No more events are being sent.", ConsoleColor.Yellow);
                cancellationSource.Cancel();

                // Allow the main thread to continue and exit...
                WaitHandle.Set();
            };

            var userInput = "";

            while (true)
            {
                Console.Write("Enter the number of the operation you would like to perform > ");

                var input = Console.ReadLine();
                if (input.Equals("1", StringComparison.InvariantCultureIgnoreCase) ||
                    input.Equals("2", StringComparison.InvariantCultureIgnoreCase))
                {
                    userInput = input.Trim();
                    break;
                }

                Console.WriteLine("Invalid input entered. Please enter 1 or 2");
            }

            switch (userInput)
            {
                case "1":
                    try
                    {
                        // Start sending telemetry to simulated devices:
                        var deviceRunTasks = SetupDeviceRunTasks(cancellationToken);
                        Task.WhenAll(deviceRunTasks).Wait(cancellationToken);
                    }
                    catch (OperationCanceledException canceled)
                    {
                        Console.WriteLine("The device telemetry operation was canceled.");
                        // No need to throw, as this was expected.
                    }
                    break;
                case "2":
                    GenerateTrainingData();
                    break;
            }

            cancellationSource.Cancel();
            Console.WriteLine();
            WriteLineInColor("Done sending generated pump data", ConsoleColor.Cyan);
            Console.WriteLine();
            Console.WriteLine();

            // Keep the console open.
            Console.ReadLine();
            WaitHandle.WaitOne();
        }

        /// <summary>
        /// Generates anomaly model training data to new CSV files.
        /// </summary>
        private static void GenerateTrainingData()
        {
            var sampleSize = 10000;

            Console.WriteLine("Generating data for ML model training. This may take a while...");

            // Generate with no failures:
            Console.WriteLine("\r\nGenerating data with no failures...");
            GenerateData.GenerateModelTrainingData(sampleSize, false, 0, false, true);
            // Generate with immediate failures:
            Console.WriteLine("\r\nGenerating data with immediate failures...");
            GenerateData.GenerateModelTrainingData(sampleSize, true, 0, false, true);
            // Generate with gradual failures:
            Console.WriteLine("\r\nGenerating data with gradual failures...");
            GenerateData.GenerateModelTrainingData(sampleSize, true, 625, false, true);

            Console.WriteLine("\r\n---------------------------\r\nGeneration complete.");
        }

        /// <summary>
        /// Creates the set of tasks that will send data to the IoT hub.
        /// </summary>
        /// <param name="fileManager">TrainingFileManager for the data set for devices to send.</param>
        /// <returns></returns>
        private static IEnumerable<Task> SetupDeviceRunTasks(CancellationToken cancellationToken)
        {
            var deviceTasks = new List<Task>();
            var config = ParseConfiguration();
            const int sampleSize = 10000;
            const int failOverXIterations = 2500;

            Console.WriteLine("Setting up simulated pump devices and generating random sample data. This may take a while...");

            // Add a pump that gradually fails.
            devices.Add(new PumpDevice(1, config.Device1ConnectionString, "DEVICE001", "192.168.1.1", new Location(35.815090, -101.043192), 
                GenerateData.GeneratePumpTelemetry(sampleSize, true, failOverXIterations)));
            // Add a pump that never fails.
            devices.Add(new PumpDevice(2, config.Device2ConnectionString, "DEVICE002", "192.168.1.2", new Location(35.815862, -101.042167),
                GenerateData.GeneratePumpTelemetry(sampleSize + failOverXIterations, false, 0)));
            // Add a pump that immediately fails after a period of time.
            devices.Add(new PumpDevice(3, config.Device3ConnectionString, "DEVICE003", "192.168.1.3", new Location(35.815743, -101.048099),  
                GenerateData.GeneratePumpTelemetry(sampleSize, true, 0)));

            foreach (var device in devices)
            {
                device.SendDeviceProperties();
                deviceTasks.Add(device.RunDeviceAsync(cancellationToken));
            }

            return deviceTasks;
        }

        /// <summary>
        /// Extracts properties from either the appsettings.json file or system environment variables.
        /// </summary>
        /// <returns>
        /// Device1ConnectionString: Connection string for the first pump device in IoT Central.
        /// Device2ConnectionString: Connection string for the second pump device in IoT Central.
        /// Device3ConnectionString: Connection string for the third pump device in IoT Central.
        /// </returns>
        private static (string Device1ConnectionString,
                        string Device2ConnectionString,
                        string Device3ConnectionString) ParseConfiguration()
        {
            try
            {
                // The Configuration object will extract values either from the machine's environment variables, or the appsettings.json file.
                var device1ConnectionString = _configuration["DEVICE_1_CONNECTION_STRING"];
                var device2ConnectionString = _configuration["DEVICE_2_CONNECTION_STRING"];
                var device3ConnectionString = _configuration["DEVICE_3_CONNECTION_STRING"];

                if (string.IsNullOrWhiteSpace(device1ConnectionString))
                {
                    throw new ArgumentException("DEVICE_1_CONNECTION_STRING must be provided");
                }

                if (string.IsNullOrWhiteSpace(device2ConnectionString))
                {
                    throw new ArgumentException("DEVICE_2_CONNECTION_STRING must be provided");
                }

                if (string.IsNullOrWhiteSpace(device3ConnectionString))
                {
                    throw new ArgumentException("DEVICE_3_CONNECTION_STRING must be provided");
                }

                return (device1ConnectionString, device2ConnectionString, device3ConnectionString);
            }
            catch (Exception e)
            {
                WriteLineInColor(e.Message, ConsoleColor.Red);
                Console.ReadLine();
                throw;
            }
        }

        public static void WriteLineInColor(string msg, ConsoleColor color)
        {
            lock (LockObject)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(msg);
                Console.ResetColor();
            }
        }
    }
}
