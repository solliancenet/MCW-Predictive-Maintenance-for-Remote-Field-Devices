using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Message = Microsoft.Azure.Devices.Client.Message;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace Fabrikam.FieldDevice.Generator
{
    public class PumpDevice
    {
        private static readonly TimeSpan CycleTime = TimeSpan.FromMilliseconds(250);
        private static readonly TwinCollection ReportedProperties = new TwinCollection();
        private static DeviceClient _deviceClient = null;
        private int _messagesSent = 0;
        private readonly string _deviceId;
        private readonly string _serialNumber;
        private readonly string _ipAddress;
        private readonly IEnumerable<PumpTelemetryItem> _pumpTelemetryData;

        /// <summary>
        /// The total number of messages sent by this device to IoT Central.
        /// </summary>
        public int MessagesSent => _messagesSent;

        /// <summary>
        /// Public constructor for TurbofanDevice class.
        /// </summary>
        /// <param name="deviceNumber">Integer number of the data series device to send.</param>
        /// <param name="deviceConnectionString">Connection string for the IoT Central device.</param>
        /// <param name="serialNumber">The device's serial number property.</param>
        /// <param name="ipAddress">The device's IP address property.</param>
        /// <param name="pumpTelemetryData">Simulated device data for the pump.</param>
        public PumpDevice(int deviceNumber, string deviceConnectionString, string serialNumber, string ipAddress,
            IEnumerable<PumpTelemetryItem> pumpTelemetryData)
        {
            _deviceId = $"Client_{deviceNumber:000}";
            _serialNumber = serialNumber;
            _ipAddress = ipAddress;
            _pumpTelemetryData = pumpTelemetryData;
            _deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
        }

        /// <summary>
        /// Creates an asynchronous task for sending all data for the device.
        /// </summary>
        /// <returns>Task for asynchronous device operation</returns>
        public async Task RunDeviceAsync(CancellationToken cancellationToken)
        {
            await SendDataToHub(_pumpTelemetryData, cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// Updates the device properties.
        /// </summary>
        public async void SendDeviceProperties()
        {
            try
            {
                Console.WriteLine($"Sending device properties to {_deviceId}:");
                ReportedProperties["SerialNumber"] = _serialNumber;
                ReportedProperties["IPAddress"] = _ipAddress;
                Console.WriteLine(JsonConvert.SerializeObject(ReportedProperties));

                await _deviceClient.UpdateReportedPropertiesAsync(ReportedProperties);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error sending device properties to {_deviceId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Takes a set of PumpTelemetryItem data for a device in a dataset and sends the
        /// data to the message with a configurable delay between each message.
        /// </summary>
        /// <param name="pumpTelemetry">The set of data to send as messages to the IoT Central.</param>
        /// <returns></returns>
        private async Task SendDataToHub(IEnumerable<PumpTelemetryItem> pumpTelemetry, CancellationToken cancellationToken)
        {
            foreach (var telemetryItem in pumpTelemetry)
            {
                await SendEvent(JsonConvert.SerializeObject(telemetryItem), cancellationToken).ConfigureAwait(false);
                await Task.Delay(CycleTime, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uses the DeviceClient to send a message to the IoT Central.
        /// </summary>
        /// <param name="deviceClient">Azure Devices client for connecting to and send data to IoT Central.</param>
        /// <param name="message">JSON string representing serialized device data.</param>
        /// <returns>Task for async execution.</returns>
        private async Task SendEvent(string message, CancellationToken cancellationToken)
        {
            using (var eventMessage = new Message(Encoding.ASCII.GetBytes(message)))
            {
                await _deviceClient.SendEventAsync(eventMessage, cancellationToken).ConfigureAwait(false);

                // Keep track of messages sent and update progress periodically.
                var currCount = Interlocked.Increment(ref _messagesSent);
                if (currCount % 50 == 0)
                {
                    Console.WriteLine($"Device: {_deviceId} Message count: {currCount}");
                }
            }
        }
    }
}
