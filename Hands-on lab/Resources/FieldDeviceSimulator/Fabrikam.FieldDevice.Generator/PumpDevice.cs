using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using Message = Microsoft.Azure.Devices.Client.Message;
using TransportType = Microsoft.Azure.Devices.Client.TransportType;

namespace Fabrikam.FieldDevice.Generator
{
    public class PumpDevice
    {
        private const string CLOUDTOGGLEPOWERCOMMAND = "MotorPower";
        public event EventHandler<PumpPowerStateChangedArgs> PumpPowerStateChanged;
               
        private readonly TimeSpan CycleTime = TimeSpan.FromMilliseconds(500);
        private readonly TwinCollection ReportedProperties = new TwinCollection();
        private DeviceClient _deviceClient = null;
        private int _messagesSent = 0;
        private readonly string _deviceId;
        private readonly string _serialNumber;
        private readonly string _ipAddress;
        private readonly Location _location;
        private readonly IEnumerable<PumpTelemetryItem> _pumpTelemetryData;
        private string _pumpPowerState = Generator.PumpPowerState.OFF;
        private CancellationTokenSource _localCancellationSource = new CancellationTokenSource();
        
        
        /// <summary>
        /// The total number of messages sent by this device to IoT Central.
        /// </summary>
        public int MessagesSent => _messagesSent;

        public string DeviceId => _deviceId;

        public string PumpPowerState { get => _pumpPowerState; set => _pumpPowerState = value; }

        /// <summary>
        /// Public constructor for Rod Pump class.
        /// </summary>
        /// <param name="deviceNumber">Integer number of the data series device to send.</param>
        /// <param name="deviceConnectionString">Connection string for the IoT Central device.</param>
        /// <param name="serialNumber">The device's serial number property.</param>
        /// <param name="ipAddress">The device's IP address property.</param>
        /// <param name="pumpTelemetryData">Simulated device data for the pump.</param>
        public PumpDevice(int deviceNumber, string deviceConnectionString, string serialNumber, string ipAddress,
            Location location, IEnumerable<PumpTelemetryItem> pumpTelemetryData)
        {
            _deviceId = $"Client_{deviceNumber:000}";
            _serialNumber = serialNumber;
            _ipAddress = ipAddress;
            _location = location;
            _pumpTelemetryData = pumpTelemetryData;
            _deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
            _deviceClient.SetMethodHandlerAsync(CLOUDTOGGLEPOWERCOMMAND, TogglePowerCommandReceived, null);
            PumpPowerState = Generator.PumpPowerState.ON;
        }

        private void ResetDevice()
        {
            _localCancellationSource = new CancellationTokenSource();
            
        }

        /// <summary>
        /// Creates an asynchronous task for sending all data for the device.
        /// </summary>
        /// <returns>Task for asynchronous device operation</returns>
        public async Task RunDeviceAsync()
        {
            await SendDataToHub(_pumpTelemetryData, _localCancellationSource.Token).ConfigureAwait(false);
        }

        public void CancelCurrentRun()
        {
            _localCancellationSource.Cancel();
        }


        /// <summary>
        /// Updates the device properties.
        /// </summary>
        public async void SendDevicePropertiesAndInitialState()
        {
            try
            {
                Console.WriteLine($"Sending device properties to {DeviceId}:");
                ReportedProperties["SerialNumber"] = _serialNumber;
                ReportedProperties["IPAddress"] = _ipAddress;
                ReportedProperties["Location"] = _location;
                Console.WriteLine(JsonConvert.SerializeObject(ReportedProperties));

                await _deviceClient.UpdateReportedPropertiesAsync(ReportedProperties);
                await SendEvent(JsonConvert.SerializeObject(new { PowerState = _pumpPowerState }), _localCancellationSource.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine($"Error sending device properties to {DeviceId}: {ex.Message}");
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
                if (!_localCancellationSource.IsCancellationRequested)
                {
                    await SendEvent(JsonConvert.SerializeObject(telemetryItem), cancellationToken).ConfigureAwait(false);
                }
        
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
                    Console.WriteLine($"Device: {DeviceId} Message count: {currCount}");
                }
            }
        }

        private Task<MethodResponse> TogglePowerCommandReceived(MethodRequest methodRequest, object userContext)
        {
            
            if(_pumpPowerState == Generator.PumpPowerState.ON)
            {
                CancelCurrentRun();
                ResetDevice();
                _pumpPowerState = Generator.PumpPowerState.OFF;
            }
            else
            {
                _pumpPowerState = Generator.PumpPowerState.ON;
            }

            OnPumpPowerStateChanged(new PumpPowerStateChangedArgs() { DeviceId = DeviceId, PumpPowerState = _pumpPowerState });
            
            SendEvent(JsonConvert.SerializeObject(new { PowerState = _pumpPowerState }), _localCancellationSource.Token).ConfigureAwait(false);
            Console.WriteLine($"Device: {DeviceId} Commanded by the Cloud to Toggle Power, Power is now {_pumpPowerState}");
            return Task.FromResult(new MethodResponse(new byte[0], 200));
        }

        protected virtual void OnPumpPowerStateChanged(PumpPowerStateChangedArgs e)
        {
            PumpPowerStateChanged?.Invoke(this, e);
        }

    }
}
